using System.Collections;
using System.Collections.Generic;
using Main;
using UnityEngine;
using Zenject;

public class SimpleInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        SetBindings();
        SetSignals();
    }

    private void SetSignals()
    {
        SignalBusInstaller.Install(Container);
        
        
    }

    private void SetBindings()
    {
        Container.BindInterfacesAndSelfTo<SimpleGameController>().AsSingle();
    }
}
