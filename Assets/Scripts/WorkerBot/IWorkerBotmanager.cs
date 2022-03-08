using System.Collections.Generic;
using Config;
using Main;
using UnityEngine;

namespace WorkerBot
{
    public interface IWorkerBotmanager
    {
        public List<CoinSourceStateChangedSignal> Tasks { get; }
        
        public void SpawnWorkerBotsFromConfig(GameConfig config);

        public void SpawnWorkerBot(Vector3 position, List<GameConfig.SkillConfig> skills, float moveSpeed);

        public void OnAddWorkerBotRequested(AddWorkerBotRequestedSignal signal);
        public void OnRemoveWorkerBotRequested(RemoveWorkerBotRequestedSignal signal);
    }
}