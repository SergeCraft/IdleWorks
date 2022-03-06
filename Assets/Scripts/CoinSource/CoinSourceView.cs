using UnityEngine;
using Zenject;

namespace CoinSource
{
    public class CoinSourceView : MonoBehaviour
    {

        public class Factory : PlaceholderFactory<InitArgs, CoinSourceView>
        {
            
        }

        public class InitArgs
        {
            public Vector3 Position { get; private set; }
            public float MiningRate { get; private set; }
            public float ProblemDelay { get; private set; }

            
            public InitArgs(Vector3 position, float miningRate, float problemDelay)
            {
                Position = position;
                MiningRate = miningRate;
                ProblemDelay = problemDelay;
            }
        }
    }
}