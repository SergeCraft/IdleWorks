namespace Config
{
    public interface IGameConfigManager
    {
        public GameConfig Config { get; }

        public void UpdateConfig(GameConfig config);
        
        public void SaveConfig();
    }
}