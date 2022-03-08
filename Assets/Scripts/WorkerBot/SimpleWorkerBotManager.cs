using System;
using System.Collections.Generic;
using System.Linq;
using CoinSource;
using Config;
using Main;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace WorkerBot
{
    public class SimpleWorkerBotManager : IWorkerBotmanager, IDisposable
    {
        private GameConfig _config;
        private DiContainer _container;
        private SignalBus _signalBus;
        private List<IWorkerBotController> _workerBots;
        private List<CoinSourceStateChangedSignal> _tasks;

        public List<CoinSourceStateChangedSignal> Tasks { get; private set; }



        public SimpleWorkerBotManager(
            SignalBus signalBus,
            IGameConfigManager configMgr,
            DiContainer container)
        {
            _signalBus = signalBus;
            _config = configMgr.Config;
            _container = container;
            _workerBots = new List<IWorkerBotController>();
            _tasks = new List<CoinSourceStateChangedSignal>();

            SpawnWorkerBotsFromConfig(_config);
            SubscribeToSignals();
        }


        public void SpawnWorkerBotsFromConfig(GameConfig config)
        {
            foreach (var configWorkerBot in config.WorkerBots)
            {
                SpawnWorkerBot(
                    configWorkerBot.BasePosition,
                    configWorkerBot.Skills,
                    configWorkerBot.Movespeed);
            }
        }

        public void SpawnWorkerBot(
            Vector3 position, 
            List<GameConfig.SkillConfig> skills, 
            float moveSpeed)
        {
            IWorkerBotController bot = _container.Resolve<IWorkerBotController>();
            bot.AddSkills(skills);
            bot.Position = position;
            bot.Movespeed = moveSpeed;
            
            _workerBots.Add(bot);
        }



        public void Dispose()
        {
            UnsubscribeFromSignals();
        }

        public void OnAddWorkerBotRequested(AddWorkerBotRequestedSignal signal)
        {
            AddRandomWorkerBot();
        }
        

        private void SubscribeToSignals()
        {
            _signalBus.Subscribe<CoinSourceStateChangedSignal>(OnCoinSourceStateChanged);
            _signalBus.Subscribe<WorkerBotStateChangedSignal>(OnWorkerBotStateChanged);
            _signalBus.Subscribe<AddWorkerBotRequestedSignal>(OnAddWorkerBotRequested);
            _signalBus.Subscribe<RemoveWorkerBotRequestedSignal>(OnRemoveWorkerBotRequested);
        }

        private void UnsubscribeFromSignals()
        {
            _signalBus.Unsubscribe<CoinSourceStateChangedSignal>(OnCoinSourceStateChanged);
            _signalBus.Unsubscribe<WorkerBotStateChangedSignal>(OnWorkerBotStateChanged);
            _signalBus.Unsubscribe<AddWorkerBotRequestedSignal>(OnAddWorkerBotRequested);
            _signalBus.Unsubscribe<RemoveWorkerBotRequestedSignal>(OnRemoveWorkerBotRequested);
        }


        private void AddRandomWorkerBot()
        {
            var skillSet = new List<GameConfig.SkillConfig>(Random.Range(1,4));
            for (int i = 0; i < skillSet.Capacity; i++)
            {
                GameConfig.SkillConfig tempSkill = null;
                do
                {
                    tempSkill = new GameConfig.SkillConfig((ProblemTypes) Random.Range(1, 4));
                } while (skillSet.Any(skill => skill == tempSkill));
                
                skillSet.Add(tempSkill);
            }
            
            SpawnWorkerBot(
                Vector3.zero, 
                skillSet,
                Random.Range(2.0f, 10.0f));
            RecalculateWorkerBotPositions();
            if(_tasks.Count > 0) 
                TryAssignWorkerBotToTask(_tasks.FirstOrDefault());
        }
        
        
        private void RemoveRandomWorkerBot()
        {
            var removableBots = 
                _workerBots.Where(x => x.State == WorkerBotStates.Idle).ToList();
            if (removableBots.Count > 0)
            {
                int id = Random.Range(0, removableBots.Count);removableBots[id].Dispose();
                _workerBots.Remove(removableBots[id]);
                RecalculateWorkerBotPositions();
            }
            
        }

        private void RecalculateWorkerBotPositions()
        {
            float minPosX = -5.0f;
            float maxPosX = 5.0f;
            float spaceX = 1.0f;
            if (_workerBots.Count > 1)
            {
                spaceX = (maxPosX - minPosX) / (_workerBots.Count - 1);
            }
            else
            {
                spaceX = (maxPosX - minPosX) / 2;
                minPosX = 0.0f;
            }
            foreach (var bot in _workerBots)
            {
                bot.BasePosition = new Vector3(
                    minPosX + spaceX * _workerBots.IndexOf(bot),
                    1.0f,
                    bot.BasePosition.z
                );
                if(bot.State == WorkerBotStates.Idle) bot.Position = bot.BasePosition;
            }

            ;
        }

        private void TryAssignWorkerBotToTask(CoinSourceStateChangedSignal coinSourceStateChangedSignal)
        {
            IWorkerBotController bot = _workerBots 
                .FirstOrDefault(x => 
                    Enumerable.Any<ProblemTypes>(
                        x.Skills,
                        y => y == coinSourceStateChangedSignal.Type)
                                     && x.State == WorkerBotStates.Idle);
            if (bot != null)
            {
                AssignWorkerBotToTask(coinSourceStateChangedSignal, bot);
            }
        }

        private void AssignWorkerBotToTask(
            CoinSourceStateChangedSignal coinSourceStateChangedSignal,
            IWorkerBotController bot)
        {
            bot.AssignTask(coinSourceStateChangedSignal.CoinSourceController);
            _tasks.Remove(coinSourceStateChangedSignal);
        }


        private void OnCoinSourceStateChanged(CoinSourceStateChangedSignal obj)
        {
            if (obj.Type != ProblemTypes.NoProbliem)
            {
                _tasks.Add(obj);
                TryAssignWorkerBotToTask(obj);
            }
            
        }
        
        private void OnWorkerBotStateChanged(WorkerBotStateChangedSignal obj)
        {
            switch (obj.WorkerBotController.State)
            {
                case WorkerBotStates.Idle:
                    var task = _tasks.Where(
                        x => obj.WorkerBotController.Skills
                            .Any(skill => skill == x.Type)).FirstOrDefault();
                    if (task != null) AssignWorkerBotToTask(task, obj.WorkerBotController);
                    break;
            }
        }
        
        public void OnRemoveWorkerBotRequested(RemoveWorkerBotRequestedSignal signal)
        {
            RemoveRandomWorkerBot();
        }
        
    }
}