using System;
using UnityEngine;
using Zenject;

namespace CoinSource
{
    public class CoinSourceView : MonoBehaviour
    {
        private Material material;
       

        public void SetState(ProblemTypes state)
        {
            switch (state)
            {
                case ProblemTypes.NoProbliem:
                    material.color = Color.grey;      
                    break;
                case ProblemTypes.BlueProblem:
                    material.color = Color.blue;
                    break;
                case ProblemTypes.GreenProblem:
                    material.color = Color.green;
                    break;
                case ProblemTypes.RedProblem:
                    material.color = Color.red;
                    break;
            }
        }
        
        
        private void Awake()
        {
            material = GetComponent<MeshRenderer>().material;
        }

        
        public class Factory : PlaceholderFactory<CoinSourceView>
        {
            
        }

    }
}