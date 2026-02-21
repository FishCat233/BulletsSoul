using UnityEngine;

namespace BulletsSoul.Projectile.Spawner
{
    public class RotateLinearSpawner : LinearSpawner
    {
        [Header("旋转设置")]
        public float rotateSpeed = 90f; // 角速度
        public float rotateAccelerateSpeed = 0f; // 角加速度
        public float maxRotateSpeed = -1f;

        protected override void Update()
        {
            base.Update();

            HandleSpeed();

            HandleRotation();
        }

        private void HandleSpeed()
        {
            // 如果 maxRotateSpeed 小于 0，表示不限速，直接加减就行
            if (maxRotateSpeed < 0f)
            {
                rotateSpeed += rotateAccelerateSpeed * Time.deltaTime;
                return;
            }

            // 关键点：使用 MoveTowards 
            // 它会根据 rotateAccelerateSpeed 的大小，让速度向目标值靠近
            // 目标值就是 maxRotateSpeed（正向）或者 -maxRotateSpeed（反向）

            float targetSpeed = rotateSpeed >= 0 ? maxRotateSpeed : -maxRotateSpeed;

            // 这里的加速度取绝对值，因为方向由 targetSpeed 决定
            rotateSpeed = Mathf.MoveTowards(rotateSpeed, targetSpeed, Mathf.Abs(rotateAccelerateSpeed) * Time.deltaTime);
        }

        private void HandleRotation()
        {
            fireAngle += rotateSpeed * Time.deltaTime;

            // 让角度循环，避免数值无限变大
            if (fireAngle > 360f) fireAngle -= 360f;
            if (fireAngle < -360f) fireAngle += 360f;
        }
    }
}
