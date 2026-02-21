using BulletsSouls.Projectile;
using System.Collections.Generic;
using UnityEngine;

namespace BulletsSoul.Projectile.Spawner
{
    public class RandomSpawner : MonoBehaviour
    {
        [Header("引用")]
        [SerializeField] private GameObject bulletPrefab;

        [Header("发射频率 (秒)")]
        public float minInterval = 0.5f;

        public float maxInterval = 2.0f;

        [Header("子弹属性")]
        public float minSpeed = 2f;

        public float maxSpeed = 10f;
        [Range(0, 360)] public float minAngle = 0f;
        [Range(0, 360)] public float maxAngle = 360f;

        [Header("子弹生命周期")]
        public float lifeTime = 2f;

        public int maxBulletCount = 200;

        private float _nextSpawnTime;
        private float _timer;

        // 追踪最早生成的子弹来销毁。这里假设了弹幕的生命时间和力屏幕的距离成正相关
        private Queue<Bullet> _bulletQueue = new Queue<Bullet>();

        private void Start()
        {
            if (bulletPrefab == null)
            {
                Debug.LogError("Bullet prefab is not assigned!");
                enabled = false;
                return;
            }

            SetNextSpawnTime();
        }

        private void Update()
        {
            _timer += Time.deltaTime;

            if (_timer >= _nextSpawnTime)
            {
                SpawnBullet();
                _timer = 0f;
                SetNextSpawnTime();
            }
        }

        private void SetNextSpawnTime()
        {
            _nextSpawnTime = Random.Range(minInterval, maxInterval);
        }

        private void SpawnBullet()
        {
            float randomAngle = Random.Range(minAngle, maxAngle);
            Quaternion rotation = Quaternion.Euler(0, 0, randomAngle);

            GameObject bulletObj = Instantiate(bulletPrefab, transform.position, rotation);

            Bullet bullet = bulletObj.GetComponent<Bullet>();
            if (bullet != null)
            {
                bullet.speed = Random.Range(minSpeed, maxSpeed);
                bullet.direction = bulletObj.transform.right;
            }

            _bulletQueue.Enqueue(bullet);

            if (_bulletQueue.Count > maxBulletCount)
            {
                var oldBullet = _bulletQueue.Dequeue();
                Destroy(oldBullet.gameObject);
            }
        }
    }
}