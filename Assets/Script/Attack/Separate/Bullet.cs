using UnityEngine;

namespace Script.Attack
{
    public class Bullet : MonoBehaviour
    {
        [SerializeField] private float bulletSpeed;
        private GameObject enemy;
        private Rigidbody[] bullets;
        

        private void Start()
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
            var dir = (enemy.transform.position - transform.position ).normalized;
            dir.y = 0;
            return dir;
        }
    
        private void BulletMove()
        {
            foreach (var bullet in bullets)
            {
                var bulletDamage = bullet.GetComponent<WeaponDealDamage>();
                bulletDamage.SetDamage(10);
                bullet.MovePosition(bullet.transform.position + (DirToTarget() * (bulletSpeed * Time.deltaTime)));
            }
        }
    }
}
