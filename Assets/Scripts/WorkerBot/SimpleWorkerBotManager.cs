using System;
using System.Collections.Generic;
using System.Linq;
using CoinSource;
using Config;
using Main;
using UnityEngine;
using Zenject;

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



        private void SubscribeToSignals()
        {
            _signalBus.Subscribe<CoinSourceStateChangedSignal>(OnCoinSourceStateChanged);
            _signalBus.Subscribe<WorkerBotStateChangedSignal>(OnWorkerBotStateChanged);
        }

        private void UnsubscribeFromSignals()
        {
            _signalBus.Unsubscribe<CoinSourceStateChangedSignal>(OnCoinSourceStateChanged);
            _signalBus.Unsubscribe<WorkerBotStateChangedSignal>(OnWorkerBotStateChanged);
        }

        private void TryAssignWorkerBotToTask(CoinSourceStateChangedSignal coinSourceStateChangedSignal)
        {
            // IWorkerBotController bot = _workerBots.Where(x => 
            //     x.Skills.Any(y => y == coinSourceStateChangedSignal.Type)
            //     && x.State == WorkerBotStates.Idle) 
            //     .FirstOrDefault();
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
    }
}