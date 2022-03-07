using CoinSource;
using WorkerBot;

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
    
    public class CoinSourceFixedSignal
    {
        public ICoinSourceController CoinSourceController { get; private set; }

        public CoinSourceFixedSignal(ICoinSourceController coinSourceController)
        {
            CoinSourceController = coinSourceController;
        }
    }
    
    
    public class WorkerBotStateChangedSignal
    {
        public IWorkerBotController WorkerBotController { get; private set; }

        public WorkerBotStateChangedSignal(IWorkerBotController workerBotController)
        {
            WorkerBotController = workerBotController;
        }
    }
    
    public class WorkerBotMoveFinishedSignal
    {
        public WorkerBotView WorkerBotView { get; private set; }

        public WorkerBotMoveFinishedSignal(WorkerBotView workerBotView)
        {
            WorkerBotView = workerBotView;
        }
    }
}