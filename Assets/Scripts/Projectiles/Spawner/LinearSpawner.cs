using BulletsSouls.Projectile;
using System.Collections.Generic;
using UnityEngine;

namespace BulletsSoul.Projectile.Spawner
{
    public class LinearSpawner : MonoBehaviour
    {
        [Header("引用")]
        [SerializeField] private GameObject bulletPrefab;

        [Header("线性发射设置")]
        public float spawnInterval = 0.2f; // 固定频率

        public float bulletSpeed = 5f;
        [Range(0, 360)] public float fireAngle = 0f; // 固定角度

        [Header("性能控制")]
        public int maxBulletCount = 100;

        private Queue<Bullet> _bulletQueue = new Queue<Bullet>();
        private float _timer;

        protected virtual void Update()
        {
            _timer += Time.deltaTime;

            if (_timer >= spawnInterval)
            {
                SpawnBullet();
                _timer = 0f;
            }
        }

        private void SpawnBullet()
        {
            if (bulletPrefab == null) return;

            // 1. 根据固定角度生成旋转
            Quaternion rotation = Quaternion.Euler(0, 0, fireAngle);
            GameObject bulletObj = Instantiate(bulletPrefab, transform.position, rotation);

            Bullet bullet = bulletObj.GetComponent<Bullet>();
            if (bullet != null)
            {
                bullet.speed = bulletSpeed;
                // 统一使用右方向作为发射轴
                bullet.direction = bulletObj.transform.right;

                // 2. 数量管控逻辑 (复用你之前的 Queue 技巧)
                _bulletQueue.Enqueue(bullet);
                if (_bulletQueue.Count > maxBulletCount)
                {
                    Bullet oldest = _bulletQueue.Dequeue();
                    if (oldest != null) Destroy(oldest.gameObject);
                }
            }
        }

        // 辅助线：在场景视图中画一条线，方便你调试射击方向
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Vector3 dir = Quaternion.Euler(0, 0, fireAngle) * Vector3.right;
            Gizmos.DrawRay(transform.position, dir * 2f);
        }
    }
}