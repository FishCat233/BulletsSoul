using BulletsSoul.Player;
using R3;
using System.Threading;
using UnityEngine;

namespace BulletsSoul.UI.PlayerUI
{
    public class PlayerUI : MonoBehaviour
    {
        public static PlayerUI Instance { get; private set; }

        public PlayerController pc;
        [SerializeField] private AnimatedProgressBar staminaBar;

        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(Instance.gameObject);
                return;
            }

            Instance = this;
        }

        private void Start()
        {
            if (pc == null || staminaBar == null) return;

            var d = Disposable.CreateBuilder();

            pc.CurrentStamina.Subscribe(stamina =>
            {
                staminaBar.CurrentValue.Value = stamina;
            }).AddTo(ref d);

            pc.MaxStamina.Subscribe(maxStamina =>
            {
                staminaBar.TotalValue.Value = maxStamina;
            }).AddTo(ref d);

            d.RegisterTo(destroyCancellationToken);
        }
    }
}
