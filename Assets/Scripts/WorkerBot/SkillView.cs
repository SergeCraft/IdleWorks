using CoinSource;
using UnityEngine;
using Zenject;

namespace WorkerBot
{
    public class SkillView : MonoBehaviour
    {


        public class Factory : PlaceholderFactory<InitArgs, SkillView>
        {
            
        }

        public class InitArgs
        {
            public ProblemTypes Type { get; private set; }

            public InitArgs(ProblemTypes type)
            {
                Type = type;
            }
        }
    }
}