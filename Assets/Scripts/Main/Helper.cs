using CoinSource;
using UnityEngine;

namespace Main
{
    public static class Helper
    {
        public static Color ConvertProblemTypeToColor(ProblemTypes type)
        {
            switch (type)
            {
                case ProblemTypes.NoProbliem:
                    return Color.grey;      
                case ProblemTypes.BlueProblem:
                    return Color.blue;
                case ProblemTypes.GreenProblem:
                    return Color.green;
                case ProblemTypes.RedProblem:
                    return Color.red;
                default: 
                    return Color.white;
            }
        }
    }
}