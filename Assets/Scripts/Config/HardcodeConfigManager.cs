using System.Collections.Generic;
using CoinSource;
using UnityEngine;

namespace Config
{
    public class HardcodeConfigManager : IGameConfigManager
    {
        private GameConfig _config;

        public GameConfig Config
        {
            get
            {
                return _config;
            }
        }

        public HardcodeConfigManager()
        {
            SetupDefaultConfig();
        }

        
        public GameConfig GetConfig()
        {
            return _config;
        }
        
        
        private void SetupDefaultConfig()
        {
            _config = new GameConfig();

            _config.ProblemGenerationDelay = 5.0f;

            _config.CoinSources.Add(new GameConfig.CoinSourceConfig(
                1.0f,
                new Vector3(-5.0f, 1.5f, 10.0f)));
            _config.CoinSources.Add(new GameConfig.CoinSourceConfig(
                2.0f,
                new Vector3(0.0f, 1.5f, 10.0f)));
            _config.CoinSources.Add(new GameConfig.CoinSourceConfig(
                1.0f,
                new Vector3(5.0f, 1.5f, 10.0f)));
            
            _config.WorkerBots.Add(
                new GameConfig.WorkerBotConfig(
                new Vector3(-1.0f, 1.0f, 0.0f),
                new List<GameConfig.SkillConfig>()
                    {
                        new GameConfig.SkillConfig(ProblemTypes.BlueProblem),
                        new GameConfig.SkillConfig(ProblemTypes.RedProblem)
                    },
                1.5f
                ));
            _config.WorkerBots.Add(
                new GameConfig.WorkerBotConfig(
                new Vector3(1.0f, 1.0f, 0.0f),
                new List<GameConfig.SkillConfig>()
                    {
                        new GameConfig.SkillConfig(ProblemTypes.GreenProblem),
                        new GameConfig.SkillConfig(ProblemTypes.RedProblem)
                    },
                5.0f
                ));
        }

        public void UpdateConfig(GameConfig config)
        {
            Debug.Log("Config unupdateable with hardcode config manager");
        }

        public void SaveConfig()
        {
            Debug.Log("Config unsaveable with hardcode config manager");
        }
    }
}