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
        [SerializeField] private AnimatedProgressBar healthBar;

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
            if (pc == null) return;

            var d = Disposable.CreateBuilder();

            if (staminaBar != null)
            {
                pc.CurrentStamina.Subscribe(stamina =>
                {
                    staminaBar.CurrentValue.Value = stamina;
                }).AddTo(ref d);

                pc.MaxStamina.Subscribe(maxStamina =>
                {
                    staminaBar.TotalValue.Value = maxStamina;
                }).AddTo(ref d);
            }

            if (healthBar != null)
            {
                pc.CurrentHealth.Subscribe(health =>
                {
                    healthBar.CurrentValue.Value = health;
                }).AddTo(ref d);

                pc.MaxHealth.Subscribe(maxHealth =>
                {
                    healthBar.TotalValue.Value = maxHealth;
                }).AddTo(ref d);
            }

            d.RegisterTo(destroyCancellationToken);
        }
    }
}
