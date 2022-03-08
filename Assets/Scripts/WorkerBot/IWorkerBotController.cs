using System.Collections.Generic;
using CoinSource;
using Config;
using UnityEngine;

namespace WorkerBot
{
    public interface IWorkerBotController
    {
        Vector3 Position { get; set; }
        Vector3 BasePosition { get; set; }
        float Movespeed { get; set; }
        List<ProblemTypes> Skills { get; set; }
        WorkerBotStates State { get; set; }


        void Dispose();
        void AddSkills(List<GameConfig.SkillConfig> skills);
        void AssignTask(ICoinSourceController coinSourceController);
    }
}