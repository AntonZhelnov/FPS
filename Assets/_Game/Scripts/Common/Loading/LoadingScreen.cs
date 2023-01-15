using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Common.Loading
{
    public class LoadingScreen : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _descriptionLabel;
        [SerializeField] private Image _progressBar;
        [SerializeField] private float _progressBarFillDuration = .5f;
        [SerializeField] private Ease _progressBarFillEase = Ease.OutQuint;

        private bool _isProgressBarFilled;
        private Tweener _tween;


        private void Start()
        {
            DontDestroyOnLoad(gameObject);
        }

        public void TrackLoadingStage(ILoadingStage loadingStage)
        {
            ResetProgressBar();
            loadingStage.Loaded += UpdateTargetProgress;
            _descriptionLabel.text = loadingStage.Description;
        }

        public async UniTask WaitForProgressBarFill()
        {
            while (!_isProgressBarFilled)
                await UniTask.Delay(
                    500,
                    DelayType.DeltaTime,
                    PlayerLoopTiming.Update,
                    this.GetCancellationTokenOnDestroy());
        }

        private void CheckProgressBarFill()
        {
            if (Math.Abs(_progressBar.fillAmount - 1) < .001f)
                _isProgressBarFilled = true;
        }

        private void FillProgressBar(float value)
        {
            _progressBar.fillAmount = value;
        }

        private void ResetProgressBar()
        {
            FillProgressBar(0f);
        }

        private void UpdateTargetProgress(float progress)
        {
            _tween.Kill();

            _tween = DOVirtual.Float(
                    _progressBar.fillAmount,
                    progress,
                    _progressBarFillDuration,
                    FillProgressBar)
                .SetEase(_progressBarFillEase)
                .SetLink(gameObject)
                .OnComplete(CheckProgressBarFill);
        }
    }
}