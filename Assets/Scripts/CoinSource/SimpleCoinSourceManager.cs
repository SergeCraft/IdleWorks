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
        private Score.Score _score;


        public SimpleCoinSourceManager(
            IGameConfigManager configMgr,
            SignalBus signalBus,
            DiContainer container,
            Score.Score score)
        {
            _signalBus = signalBus;
            _config = configMgr.Config;
            _container = container;
            _coinSources = new List<ICoinSourceController>();
            _lastProblemTime = Time.time;
            _score = score;

            SubscribeToSignals();
            SpawnCoinSourcesFromConfig(_config);
        }



        public void SpawnCoinSource(Vector3 position, float miningRate)
        {
             ICoinSourceController ctlr = _container.Resolve<ICoinSourceController>();
             ctlr.Position = position;
             ctlr.MiningRate = miningRate;
             _coinSources.Add(ctlr);
             _score.UpdateCoinRate(miningRate);
        }

        public void SpawnCoinSourcesFromConfig(GameConfig cfg)
        {
            foreach (var csConfig in cfg.CoinSources)
            {
                SpawnCoinSource(
                    csConfig.Position,
                    csConfig.MiningRate);
            }
            RecalculatePositions();
        }

        public void DestroyCoinSource(ICoinSourceController coinSourceController)
        {
            _coinSources.Remove(coinSourceController);
            coinSourceController.Dispose();
            _score.UpdateCoinRate(- coinSourceController.MiningRate);
        }

        public void Dispose()
        {
            UnsubscribeFromSignals();
        }


        public void Tick()
        {
            switch (_config.Mode)
            {
                case GameModes.TestTask:
                    if (Time.time - _lastProblemTime >= _config.ProblemGenerationDelay &&
                    _coinSources.All(x => x.ProblemState == ProblemTypes.NoProbliem))
                        GenerateProblem();
                    break;
                case GameModes.Multiproblem:
                    if (Time.time - _lastProblemTime >= _config.ProblemGenerationDelay)
                        GenerateProblem();
                    break;
            }
            
            
        }

        
        private void GenerateProblem()
        {
            var problemType = (ProblemTypes)Random.Range(
                1, 
                ProblemTypes.GetValues(typeof(ProblemTypes)).Length);
            // var breakableCoinSources = _coinSources
            //     .Where(x => x.ProblemState == ProblemTypes.NoProbliem).ToList();
            var breakableCoinSources = _coinSources;
            var coinSource = breakableCoinSources[ Random.Range(0, breakableCoinSources.Count)];
            coinSource.SetProblem(problemType);
            
            _score.UpdateCoinRate(-coinSource.MiningRate);
            if (_config.Mode == GameModes.Multiproblem)_lastProblemTime = Time.time;
        }
        
        private void SubscribeToSignals()
        {
            _signalBus.Subscribe<CoinSourceFixedSignal>(OnCoinSourceFixed);
            _signalBus.Subscribe<AddCoinSourceRequestedSignal>(OnAddCoinSourceRequested);
            _signalBus.Subscribe<RemoveCoinSourceRequestedSignal>(OnRemoveCoinSourceRequested);
        }
        
        private void UnsubscribeFromSignals()
        {
            _signalBus.Unsubscribe<CoinSourceFixedSignal>(OnCoinSourceFixed);
            _signalBus.Unsubscribe<AddCoinSourceRequestedSignal>(OnAddCoinSourceRequested);
            _signalBus.Unsubscribe<RemoveCoinSourceRequestedSignal>(OnRemoveCoinSourceRequested);
        }
        
        
        private void SpawnRandomCoinSource()
        {
            SpawnCoinSource(Vector3.zero, Random.Range(1, 5));
            RecalculatePositions();
        }

        private void RemoveRandomCoinSource()
        {
            var coinSourceToRemove = _coinSources[Random.Range(0, _coinSources.Count)];
            coinSourceToRemove.Dispose();
            _coinSources.Remove(coinSourceToRemove);
            RecalculatePositions();
        }

        private void RecalculatePositions()
        {
            float minPosX = -5.0f;
            float maxPosX = 5.0f;
            float spaceX;
                
            if (_coinSources.Count > 1)
            {
                spaceX = (maxPosX - minPosX)/(_coinSources.Count - 1);
            }
            else
            {
                spaceX = maxPosX - minPosX / 2;
            }

            foreach (var coinSource in _coinSources)
            {
                coinSource.Position = new Vector3(
                    minPosX + spaceX * _coinSources.IndexOf(coinSource),
                    1.5f,
                    10.0f
                );
            }
            
        }

        
        private void OnCoinSourceFixed(CoinSourceFixedSignal obj)
        {
            obj.CoinSourceController.SetProblem(ProblemTypes.NoProbliem);
            _score.UpdateCoinRate(obj.CoinSourceController.MiningRate);
            if (_config.Mode == GameModes.TestTask) _lastProblemTime = Time.time;
        }

        public void OnAddCoinSourceRequested(AddCoinSourceRequestedSignal signal)
        {
            SpawnRandomCoinSource();
        }


        public void OnRemoveCoinSourceRequested(RemoveCoinSourceRequestedSignal signal)
        {
            RemoveRandomCoinSource();
        }

    }
}