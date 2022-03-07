using System;
using Main;
using UnityEngine;
using Zenject;

namespace CoinSource
{
    public class CoinSourceView : MonoBehaviour
    {
        private Material material;
       

        public void SetState(ProblemTypes state)
        {
            material.color = Helper.ConvertProblemTypeToColor(state);
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