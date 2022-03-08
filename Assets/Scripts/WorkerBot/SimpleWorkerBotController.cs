using System;
using System.Collections.Generic;
using CoinSource;
using Config;
using Main;
using UnityEngine;
using Zenject;

namespace WorkerBot
{
    public class SimpleWorkerBotController : IWorkerBotController, IDisposable
    {
        private WorkerBotStates _state;
        private SignalBus _signalBus;
        private WorkerBotView.Factory _wbFactory;
        private SkillView.Factory _skillFactory;
        private WorkerBotView _wbView;
        private ICoinSourceController _actualTarget;



        public float Movespeed
        {
            get
            {
                return _wbView.moveSpeed;
            }
            set
            {
                _wbView.moveSpeed = value;
            }
        }

        public Vector3 Position
        {
            get
            {
                return _wbView.transform.position;
            }
            set
            {
                _wbView.transform.position = value;
                BasePosition = BasePosition == Vector3.zero ? value : BasePosition;
            }
        }
        
        public Vector3 BasePosition { get; set; }
        
        public List<ProblemTypes> Skills { get; set; }
        public WorkerBotStates State
        {
            get
            {
                return _state;
            }
            set
            {
                if (value != _state)
                {
                    _state = value;
                    _signalBus.Fire(new WorkerBotStateChangedSignal(this));
                };
            }
        }

        
        public SimpleWorkerBotController(
            SignalBus signalBus,
            WorkerBotView.Factory wbFactory,
            SkillView.Factory skillFactory)
        {
            _signalBus = signalBus;
            _wbFactory = wbFactory;
            _skillFactory = skillFactory;

            Skills = new List<ProblemTypes>();

            SubscribeToSignals();
            InstantiateView();
        }


        public void AddSkills(List<GameConfig.SkillConfig> skills)
        {
            foreach (var skillCfg in skills)
            {
                Skills.Add(skillCfg.Type);
                SkillView skillView = _skillFactory.Create(new SkillView.InitArgs(skillCfg.Type));
                skillView.transform.SetParent(_wbView.transform.Find("SkillSet"));
                
                skillView.transform.position = new Vector3(
                    0 - (skills.Count - 1) * 0.5f + skills.IndexOf(skillCfg),
                    2.2f,
                    0.0f);
            }

            ;
        }

        public void AssignTask(ICoinSourceController coinSourceController)
        { 
            _actualTarget = coinSourceController;
            _wbView.MoveTo(new Vector3(
               _actualTarget.Position.x,
               1.0f,
               _actualTarget.Position.z));
            _state = WorkerBotStates.MovingToFix;
        }
        
        
        public void Dispose()
        {
            UnsubscribeFromSignals();
            GameObject.Destroy(_wbView.gameObject);
        }


        private void InstantiateView()
        {
            _wbView = _wbFactory.Create();
        }

        private void SubscribeToSignals()
        {
            _signalBus.Subscribe<WorkerBotMoveFinishedSignal>(OnMoveFinished);
        }

        private void UnsubscribeFromSignals()
        {
            _signalBus.Unsubscribe<WorkerBotMoveFinishedSignal>(OnMoveFinished);
        }

        
        private void OnMoveFinished(WorkerBotMoveFinishedSignal obj)
        {
            if (obj.WorkerBotView == _wbView)
            {
                switch (_state)
                {
                    case WorkerBotStates.MovingToFix:
                        _wbView.MoveTo(BasePosition);
                        State = WorkerBotStates.MovingToBase;
                        _signalBus.Fire(new CoinSourceFixedSignal(_actualTarget));
                        _actualTarget = null;
                        break;
                    case WorkerBotStates.MovingToBase:
                        State = WorkerBotStates.Idle;
                        break;
                };
            }
        }
    }
}