using BulletsSoul.Player;
using BulletsSoul.Projectile;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace BulletsSouls.Projectile
{
    public class Bullet : MonoBehaviour
    {
        public CollisionDamager damager;

        [Header("移动")]
        public float speed = 1.0f;
        public Vector3 direction = Vector3.zero;

        [Header("生命周期")]
        public float maxLifetime = 20.0f;

        private float _lifeTimer = 0f;

        private void Start()
        {
            damager = GetComponent<CollisionDamager>();
        }

        private void Update()
        {
            HandleTimer();

            HandleLifeTime();

            HandleMovement();
        }

        private void HandleTimer()
        {
            _lifeTimer += Time.deltaTime;
        }

        private void HandleLifeTime()
        {
            if (_lifeTimer > maxLifetime) Destroy(gameObject);
        }

        private void HandleMovement()
        {
            var deltaPosition = speed * Time.deltaTime * direction.normalized;
            transform.position += deltaPosition;
        }

    }

}

