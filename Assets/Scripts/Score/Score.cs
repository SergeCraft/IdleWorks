using System;
using Main;
using Zenject;

namespace Score
{
    public class Score
    {
        private SignalBus _signalBus;
        
        public float CoinsPerSec { get; private set; }

        public Score(SignalBus signalBus)
        {
            _signalBus = signalBus;
            
            CoinsPerSec = 0;
        }

        public void UpdateCoinRate(float qty)
        {
            CoinsPerSec += qty;
            _signalBus.Fire(new ScoreUpdatedSignal(this));
        }
    }
}