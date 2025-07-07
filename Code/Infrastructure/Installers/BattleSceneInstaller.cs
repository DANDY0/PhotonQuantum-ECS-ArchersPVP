using System;
using Code.UI.Gameplay;
using Code.UI.Windows;
using UnityEngine;
using Zenject;

namespace Code.Infrastructure.Installers
{
    public class BattleSceneInstaller : MonoInstaller
    {
        [Inject] private IWindowFactory _windowFactory;
        
        public override void InstallBindings()
        {
            _windowFactory.CreateBattle();
        }
    }
}