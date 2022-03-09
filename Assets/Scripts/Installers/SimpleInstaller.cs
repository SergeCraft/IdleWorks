using System.Collections;
using System.Collections.Generic;
using CoinSource;
using Config;
using GUI;
using Main;
using UnityEngine;
using WorkerBot;
using Zenject;
using Score = Score.Score;

public class SimpleInstaller : MonoInstaller
{
    private GameObject _workerBotPrefab;
    private GameObject _skillPrefab;
    private GameObject _coinSourcePrefab;
    private GameObject _guiPrefab;
    
    public override void InstallBindings()
    {
        SetPrefabs();
        SetBindings();
        SetSignals();
        SetFactories();
    }

    private void SetFactories()
    {
        Container.BindFactory<WorkerBotView, WorkerBotView.Factory>()
            .FromComponentInNewPrefab(_workerBotPrefab);
        Container.BindFactory<SkillView.InitArgs, SkillView, SkillView.Factory>()
            .FromComponentInNewPrefab(_skillPrefab);
        Container.BindFactory<CoinSourceView, CoinSourceView.Factory>()
            .FromComponentInNewPrefab(_coinSourcePrefab);
    }

    private void SetPrefabs()
    {
        _workerBotPrefab = Resources.Load<GameObject>("Prefabs/SergeCraft/WorkerBot");
        _skillPrefab = Resources.Load<GameObject>("Prefabs/SergeCraft/Skill");
        _coinSourcePrefab = Resources.Load<GameObject>("Prefabs/SergeCraft/CoinSource");
        _guiPrefab = Resources.Load<GameObject>("Prefabs/SergeCraft/GUI");
    }

    private void SetSignals()
    {
        SignalBusInstaller.Install(Container);

        Container.DeclareSignal<CoinSourceStateChangedSignal>();
        Container.DeclareSignal<WorkerBotStateChangedSignal>();
        Container.DeclareSignal<WorkerBotMoveFinishedSignal>();
        Container.DeclareSignal<CoinSourceFixedSignal>();
        Container.DeclareSignal<AddWorkerBotRequestedSignal>();
        Container.DeclareSignal<RemoveWorkerBotRequestedSignal>();
        Container.DeclareSignal<AddCoinSourceRequestedSignal>();
        Container.DeclareSignal<RemoveCoinSourceRequestedSignal>();
        Container.DeclareSignal<ScoreUpdatedSignal>();
    }

    private void SetBindings()
    {
        Container.BindInterfacesTo<HardcodeConfigManager>().AsSingle();
        Container.BindInterfacesAndSelfTo<SimpleGameController>().AsSingle();
        Container.BindInterfacesTo<SimpleCoinSourceManager>().AsSingle();
        Container.Bind<ICoinSourceController>().To<SimpleCoinSourceController>().AsTransient();
        Container.BindInterfacesTo<SimpleWorkerBotManager>().AsSingle();
        Container.Bind<IWorkerBotController>().To<SimpleWorkerBotController>().AsTransient();
        Container.Bind<GUIView>().FromComponentInNewPrefab(_guiPrefab).AsSingle();
        Container.Bind<global::Score.Score>().AsSingle();
    }
}
