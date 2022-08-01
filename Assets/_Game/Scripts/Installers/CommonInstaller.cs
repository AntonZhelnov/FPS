using System;
using System.Collections.Generic;
using Common;
using Common.Pausing;
using Common.States;
using Configs;
using Gameplay.Player;
using Gameplay.Player.Actions;
using Gameplay.States;
using Gameplay.Weapons.Factories;
using UnityEngine;
using Zenject;

namespace Installers
{
    public class CommonInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesTo<PoolCleanupChecker>().AsSingle();

            Container.Bind<Pauser>().AsSingle();
            Container.Bind<SceneReloader>().AsSingle();
            Container.BindFactory<int, Health, Health.Factory>();
            Container.Bind<Counter>().AsSingle();
            Container.Bind<WeaponFactory>().AsSingle();
            Container.Bind<PlayerActions>().AsSingle();
            Container.BindInstance(DamageGroup.Player).WhenInjectedInto<PlayerCharacter>();
            Container.BindFactory<List<State>, StateMachine, StateMachine.Factory>();
            Container.BindFactory<AnimationConfig, Animator, Action, AnimationPlayer, AnimationPlayer.Factory>();
            Container.BindFactory<PlayingGameState, PlayingGameState.Factory>();
            Container.BindFactory<PausedGameState, PausedGameState.Factory>();
            Container.BindFactory<LostGameState, LostGameState.Factory>();
        }
    }
}