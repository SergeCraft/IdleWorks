using System;
using Main;
using UnityEngine;
using Zenject;

namespace CoinSource
{
    public class SimpleCoinSourceController : ICoinSourceController, IDisposable
    {
        private SignalBus _signalBus;
        private ProblemTypes _problemState;
        private CoinSourceStates _workingState;
        private CoinSourceView _view;

        public ProblemTypes ProblemState
        {
            get
            {
                return _problemState;
            }
            set
            {
                _problemState = value;
                _signalBus.Fire(new CoinSourceStateChangedSignal(this, _problemState));
                _workingState = value == ProblemTypes.NoProbliem 
                    ? CoinSourceStates.Working 
                    : CoinSourceStates.Idle;
                _view.SetState(_problemState);
            }
        }

        public Vector3 Position
        {
            get { return _view.transform.position; }
            set { _view.transform.position = value; }
        }
        public float MiningRate { get; set; }


        public SimpleCoinSourceController(SignalBus signalBus, CoinSourceView.Factory factory)
        {
            _view = factory.Create();
            _signalBus = signalBus;
            ProblemState = ProblemTypes.NoProbliem;
        }
        
        
        public void SetProblem(ProblemTypes problemType)
        {
            ProblemState = problemType;
        }

        public void Dispose()
        {
            GameObject.Destroy(_view.gameObject);
        }
    }
}