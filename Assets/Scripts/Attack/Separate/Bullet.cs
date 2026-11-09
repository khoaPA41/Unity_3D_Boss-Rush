using UnityEngine;

namespace Attack.Separate
{
    public class Bullet : MonoBehaviour
    {
        [SerializeField] private float bulletSpeed;
        private GameObject enemy;
        private Rigidbody[] bullets;


        private void Awake()
        {
            bullets = GetComponentsInChildren<Rigidbody>();
        }

        private void Update()
        {
            BulletMove();
        }

        public void SetTarget(GameObject target)
        {
            enemy = target;
        }

        private Vector3 DirToTarget()
        {
            if (enemy == null) return Vector3.forward;
            var dir = (enemy.transform.position - transform.position).normalized;
            dir.y = 0;
            return dir;
        }

        private void BulletMove()
        {
            foreach (var bullet in bullets)
            {
                bullet.MovePosition(bullet.transform.position + (DirToTarget() * (bulletSpeed * Time.deltaTime)));
            }
        }
    }
}
