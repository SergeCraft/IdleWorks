using System.Collections.Generic;
using CoinSource;
using UnityEngine;
using WorkerBot;

namespace Config
{
    public class GameConfig
    {
        public List<WorkerBotConfig> WorkerBots { get; private set; }
        public List<CoinSourceConfig> CoinSources { get; private set; }

        public GameModes Mode { get; private set; }
        
        public float ProblemGenerationDelay { get; set; }

        public GameConfig()
        {
            WorkerBots = new List<WorkerBotConfig>();
            CoinSources = new List<CoinSourceConfig>();
            Mode = GameModes.Multiproblem;
        }

        public void AddCoinSourceConfig(CoinSourceConfig cfg)
        {
            CoinSources.Add(cfg);
        } 
        
        public void AddWorkerBotConfig(CoinSourceConfig cfg)
        {
            CoinSources.Add(cfg);
        }

        public class CoinSourceConfig
        {
            public float MiningRate { get; private set; }
            public Vector3 Position { get; private set; }
            
            
            public CoinSourceConfig(float miningRate, Vector3 position)
            {
                MiningRate = miningRate;
                Position = position;
            }
        }
        
        public class WorkerBotConfig
        {
            public Vector3 BasePosition { get; private set; }
            public List<SkillConfig> Skills { get; private set; }
            public float Movespeed { get; private set; }

            
            public WorkerBotConfig(Vector3 basePosition, List<SkillConfig> skills, float movespeed)
            {
                BasePosition = basePosition;
                Skills = skills;
                Movespeed = movespeed;
            }
        }

        public class SkillConfig
        {
            public ProblemTypes Type { get; private set; }

            
            public SkillConfig(ProblemTypes type)
            {
                Type = type;
            }
        }
    }
}