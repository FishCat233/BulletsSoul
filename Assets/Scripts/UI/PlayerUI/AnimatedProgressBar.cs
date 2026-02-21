using DG.Tweening;
using R3;
using UnityEngine;

namespace BulletsSoul.UI.PlayerUI
{
    [RequireComponent(typeof(RectTransform))]
    public class AnimatedProgressBar : MonoBehaviour
    {
        private RectTransform _rectTransform;

        [SerializeField] private RectTransform background;
        [SerializeField] private RectTransform delta;
        [SerializeField] private RectTransform current;

        public ReactiveProperty<float> CurrentValue = new ReactiveProperty<float>(0f);
        public ReactiveProperty<float> TotalValue = new ReactiveProperty<float>(0f);

        [SerializeField] private float animationDuration = 0.3f;
        [SerializeField] private float deltaAnimationDuration = 0.5f;

        private float _previousValue;
        private Tween _currentTween;
        private Tween _deltaTween;

        private void Start()
        {
            _rectTransform = GetComponent<RectTransform>();

            _previousValue = CurrentValue.Value;

            var d = Disposable.CreateBuilder();

            CurrentValue.Subscribe(v =>
            {
                UpdateProgressBar(v);
            }).AddTo(ref d);

            TotalValue.Subscribe(v =>
            {
                UpdateProgressBar(CurrentValue.Value);
            }).AddTo(ref d);

            d.RegisterTo(destroyCancellationToken);
        }

        private void UpdateProgressBar(float newValue)
        {
            if (TotalValue.Value <= 0) return;

            float previousRatio = _previousValue / TotalValue.Value;
            float newRatio = newValue / TotalValue.Value;

            _currentTween?.Kill();
            _currentTween = current.DOSizeDelta(new Vector2((1 - newRatio) * -_rectTransform.sizeDelta.x, current.sizeDelta.y), animationDuration)
                .SetEase(Ease.OutQuad);

            if (newValue < _previousValue)
            {
                _deltaTween?.Kill();
                delta.sizeDelta = new Vector2((1 - previousRatio) * -_rectTransform.sizeDelta.x, delta.sizeDelta.y);
                _deltaTween = delta.DOSizeDelta(new Vector2((1 - newRatio) * -_rectTransform.sizeDelta.x, delta.sizeDelta.y), deltaAnimationDuration)
                    .SetEase(Ease.OutQuad);
            }

            _previousValue = newValue;
        }
    }
}


