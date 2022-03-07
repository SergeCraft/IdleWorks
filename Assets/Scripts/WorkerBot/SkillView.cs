using CoinSource;
using Main;
using UnityEngine;
using Zenject;

namespace WorkerBot
{
    public class SkillView : MonoBehaviour
    {

        [Inject]
        public void Construct(InitArgs args)
        {
            GetComponent<MeshRenderer>().material.color = 
                Helper.ConvertProblemTypeToColor(args.Type);
        }


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