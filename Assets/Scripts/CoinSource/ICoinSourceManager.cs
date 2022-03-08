using Config;
using Main;
using UnityEngine;

namespace CoinSource
{
    public interface ICoinSourceManager
    {
        void SpawnCoinSource(Vector3 position, float miningRate);
        
        void SpawnCoinSourcesFromConfig(GameConfig cfg);

        void DestroyCoinSource(ICoinSourceController coinSourceController);

        void OnAddCoinSourceRequested(AddCoinSourceRequestedSignal signal);
        void OnRemoveCoinSourceRequested(RemoveCoinSourceRequestedSignal signal);
    }
}