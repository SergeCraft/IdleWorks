using UnityEngine.UI;

namespace GUI
{
    public interface IGUIView
    {
        Button AddBotButton { get; }
        Button RemoveBotButton { get; }
        Button AddCoinSourceButton { get; }
        Button RemoveCoinSourceButton { get; }
        
        
        void AddBotButtonClicked();

        void RemoveBotButtonClicked();

        void AddCoinSourceButtonClicked();

        void RemoveCoinSourceButtonClicked();

        void OnScoreChanged();
    }
}