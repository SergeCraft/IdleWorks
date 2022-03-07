using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Config;
using Main;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace CoinSource
{
    public class SimpleCoinSourceManager : ICoinSourceManager, ITickable, IDisposable
    {
        private SignalBus _signalBus;
        private DiContainer _container;
        private GameConfig _config;
        private List<ICoinSourceController> _coinSources;
        private IEnumerator _problemsCoroutine;
        private float _lastProblemTime;


        public SimpleCoinSourceManager(IGameConfigManager configMgr, SignalBus signalBus, DiContainer container)
        {
            _signalBus = signalBus;
            _config = configMgr.Config;
            _container = container;
            _coinSources = new List<ICoinSourceController>();
            _lastProblemTime = Time.time;

            SubscribeToSignals();
            SpawnCoinSourcesFromConfig(_config);
        }



        public void SpawnCoinSource(Vector3 position, float miningRate)
        {
             ICoinSourceController ctlr = _container.Resolve<ICoinSourceController>();
             ctlr.Position = position;
             ctlr.MiningRate = miningRate;
             _coinSources.Add(ctlr);
        }

        public void SpawnCoinSourcesFromConfig(GameConfig cfg)
        {
            foreach (var csConfig in cfg.CoinSources)
            {
                SpawnCoinSource(
                    csConfig.Position,
                    csConfig.MiningRate);
            }
        }

        public void DestroyCoinSource(ICoinSourceController coinSourceController)
        {
            _coinSources.Remove(coinSourceController);
            coinSourceController.Dispose();
        }
        
        public void Dispose()
        {
            UnsubscribeFromSignals();
        }


        public void Tick()
        {
            if (Time.time - _lastProblemTime >= _config.ProblemGenerationDelay &&
                _coinSources.All(x => x.ProblemState == ProblemTypes.NoProbliem))
                GenerateProblem();
        }

        
        private void GenerateProblem()
        {
            var problemType = (ProblemTypes)Random.Range(
                1, 
                ProblemTypes.GetValues(typeof(ProblemTypes)).Length);
            var coinSource = _coinSources[ Random.Range(0, _coinSources.Count)];
            coinSource.SetProblem(problemType);
        }
        
        private void SubscribeToSignals()
        {
            _signalBus.Subscribe<CoinSourceFixedSignal>(OnCoinSourceFixed);
        }
        
        private void UnsubscribeFromSignals()
        {
            _signalBus.Unsubscribe<CoinSourceFixedSignal>(OnCoinSourceFixed);
        }

        private void OnCoinSourceFixed(CoinSourceFixedSignal obj)
        {
            obj.CoinSourceController.SetProblem(ProblemTypes.NoProbliem);
            _lastProblemTime = Time.time;
        }
    }
}