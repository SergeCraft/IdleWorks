using System;
using GUI;
using UnityEngine;
using Zenject;

namespace Main
{
    public class SimpleGameController: IDisposable
    {
        private SignalBus _signalBus;


        public SimpleGameController(SignalBus signalBus, GUIView gui)
        {
            _signalBus = signalBus;


            SubscribeToSignals();
            Restart();
        }
        
        
        public void Dispose()
        {
            UnsubscribeFromSignals();
        }

        
        private void SubscribeToSignals()
        {
            
        }
        
        private void UnsubscribeFromSignals()
        {
            
        }

        private void Restart()
        {
            Debug.Log("Simple game controller restart");
        }

    }
}