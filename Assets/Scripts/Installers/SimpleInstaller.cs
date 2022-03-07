using System.Collections;
using System.Collections.Generic;
using CoinSource;
using Config;
using Main;
using UnityEngine;
using WorkerBot;
using Zenject;

public class SimpleInstaller : MonoInstaller
{
    private GameObject _workerBotPrefab;
    private GameObject _skillPrefab;
    private GameObject _coinSourcePrefab;
    
    public override void InstallBindings()
    {
        SetBindings();
        SetSignals();
        SetPrefabs();
        SetFactories();
    }

    private void SetFactories()
    {
        Container.BindFactory<WorkerBotView.InitArgs, WorkerBotView, WorkerBotView.Factory>()
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
    }

    private void SetSignals()
    {
        SignalBusInstaller.Install(Container);

        Container.DeclareSignal<CoinSourceStateChangedSignal>();
    }

    private void SetBindings()
    {
        Container.BindInterfacesTo<HardcodeConfigManager>().AsSingle();
        Container.BindInterfacesAndSelfTo<SimpleGameController>().AsSingle();
        Container.BindInterfacesTo<SimpleCoinSourceManager>().AsSingle();
        Container.Bind<ICoinSourceController>().To<SimpleCoinSourceController>().AsTransient();
    }
}
