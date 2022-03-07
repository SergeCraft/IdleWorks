using CoinSource;

namespace Main
{
    public class CoinSourceStateChangedSignal
    {
        public ICoinSourceController CoinSourceController { get; private set; }
        public ProblemTypes Type { get; private set; }

        public CoinSourceStateChangedSignal(ICoinSourceController coinSourceController, ProblemTypes type)
        {
            CoinSourceController = coinSourceController;
            Type = type;
        }
    }
}