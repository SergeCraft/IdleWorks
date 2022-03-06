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
        Container.BindFactory<WorkerBotView.InitArgs, WorkerBotView, WorkerBotView.Factory>();
        Container.BindFactory<SkillView.InitArgs, SkillView, SkillView.Factory>();
        Container.BindFactory<CoinSourceView.InitArgs, CoinSourceView, CoinSourceView.Factory>();
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
        
        
    }

    private void SetBindings()
    {
        Container.BindInterfacesTo<HardcodeConfigManager>().AsSingle();
        Container.BindInterfacesAndSelfTo<SimpleGameController>().AsSingle();
    }
}
