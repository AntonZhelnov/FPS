using Common.UI;
using Gameplay.UI.GameOver;
using Gameplay.UI.HUD;
using UnityEngine;
using Zenject;

namespace Gameplay.UI
{
    [CreateAssetMenu(
        fileName = "New UI",
        menuName = "Installers/UI")]
    public class UIInstaller : ScriptableObjectInstaller
    {
        [SerializeField] private TmpFormattedIntSetter.Settings _playerHealthTextSettings;
        [SerializeField] private TmpFormattedIntSetter.Settings _scoreTextSettings;
        [SerializeField] private TmpFormattedIntSetter.Settings _topScoreTextSettings;
        [SerializeField] private TmpFormattedIntSetter.Settings _finalScoreTextSettings;


        public override void InstallBindings()
        {
            Container.BindInstance(_playerHealthTextSettings).WhenInjectedInto<PlayerHealthText>();
            Container.BindInstance(_scoreTextSettings).WhenInjectedInto<ScoreText>();
            Container.BindInstance(_topScoreTextSettings).WhenInjectedInto<TopScoreText>();
            Container.BindInstance(_finalScoreTextSettings).WhenInjectedInto<FinalScoreText>();
        }
    }
}