using System;
using UnityEngine;

namespace CoinSource
{
    public interface ICoinSourceController : IDisposable
    {
        Vector3 Position { get; set; }
        float MiningRate { get; set; }
        ProblemTypes ProblemState { get; }
        
        void SetProblem(ProblemTypes problemType);
    }
}