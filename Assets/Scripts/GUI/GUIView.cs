using Main;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace GUI
{
    public class GUIView : MonoBehaviour, IGUIView
    {
        private SignalBus _signalBus;
        
        public Button AddBotButton { get; private set; }
        public Button RemoveBotButton { get; private set; }
        public Button AddCoinSourceButton { get; private set; }
        public Button RemoveCoinSourceButton { get; private set; }

        [Inject]
        public void Construct(SignalBus signalBus)
        {
            _signalBus = signalBus;

            AddBotButton = transform.Find("AddBotButton").GetComponent<Button>();
            RemoveBotButton = transform.Find("RemoveBotButton").GetComponent<Button>();
            AddCoinSourceButton = transform.Find("AddCoinSourceButton").GetComponent<Button>();
            RemoveCoinSourceButton = transform.Find("RemoveCoinSourceButton").GetComponent<Button>();
            
        }

        public void AddBotButtonClicked()
        {
            _signalBus.Fire<AddWorkerBotRequestedSignal>();
        }
        
        public void RemoveBotButtonClicked()
        {
            _signalBus.Fire<RemoveWorkerBotRequestedSignal>();
        }
        
        public void AddCoinSourceButtonClicked()
        {
            _signalBus.Fire<AddCoinSourceRequestedSignal>();
        }
        
        public void RemoveCoinSourceButtonClicked()
        {
            _signalBus.Fire<RemoveCoinSourceRequestedSignal>();
        }

        public void OnScoreChanged()
        {
            
        }
    }
}