using Main;
using UnityEngine;
using Zenject;

namespace CoinSource
{
    public class SimpleCoinSourceController : ICoinSourceController
    {
        private SignalBus _signalBus;
        private ProblemTypes _state;
        private CoinSourceView _view;

        public ProblemTypes State
        {
            get
            {
                return _state;
            }
            set
            {
                _state = value;
                _signalBus.Fire(new CoinSourceStateChangedSignal(this, _state));
                _view.SetState(_state);
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
        }
        
        
        public void SetProblem(ProblemTypes problemType)
        {
            State = problemType;
        }

        public void Dispose()
        {
            
        }
    }
}