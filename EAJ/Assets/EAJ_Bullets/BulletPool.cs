using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EAJ
{
    public class BulletPool : MonoBehaviour
    {
        [SerializeField]
        public GameObject bulletPrefab;
        public bool bDisableBulletsOnSpawnerDeath;
        public int poolSize = 20;
        private Queue<GameObject> poolQueue = new Queue<GameObject>();
        public Enemy EnemyComponent;
        private float BulletLifetime = 0f; 

        private Coroutine DisableCoro = null;

        private void Awake()
        {
            for (int i = 0; i < poolSize; i++)
            {
                GameObject bullet = Instantiate(bulletPrefab);
                bullet.SetActive(false);
                bullet.GetComponentInChildren<Bullet>().Pool = this;
                poolQueue.Enqueue(bullet);
            }
        }

        private void Start()
        {
            if (EnemyComponent == null)
            {
                EnemyComponent = GetComponent<Enemy>();
            }

            BulletLifetime = bulletPrefab.GetComponent<Bullet>().Lifetime;
        }

        private void OnEnable()
        {
            EnableColliders<SphereCollider>();
            EnableColliders<BoxCollider>();
        }

        private void Update()
        {

            if (EnemyComponent != null)
            {

                if (bDisableBulletsOnSpawnerDeath && IsSpawnerDead())
                {
                    if (DisableCoro == null)
                    {
                        DisableCoro = StartCoroutine(DisableBulletsCoroutine());
                    }
                }
            }
        }

        private IEnumerator DisableBulletsCoroutine()
        {
                // Copy the active bullets to a list
                List<GameObject> activeBullets = new List<GameObject>();
                foreach (var bullet in poolQueue)
                {
                    if (bullet.activeSelf)
                    {
                        activeBullets.Add(bullet);
                    }
                }

                // Iterate over the list and disable bullets
                foreach (var bullet in activeBullets)
                {
                    bullet.SetActive(false);
                    yield return new WaitForSeconds(0.05f);
                }
        }

        private bool IsSpawnerDead()
        {
            if (EnemyComponent.gameObject.activeSelf == false)
            {
                    return true;
            }
            else
            {
                return false;
            }

            return false;

        }

        public GameObject GetBullet()
        {
            if (poolQueue.Count > 0)
            {
                GameObject bullet = poolQueue.Dequeue();
                bullet.SetActive(true);
                return bullet;
            }
            else
            {
                GameObject bullet = Instantiate(bulletPrefab);
                return bullet;
            }
        }

        public void ReturnBullet(GameObject bullet)
        {
            bullet.GetComponent<Bullet>().Reset();
            bullet.SetActive(false);
            poolQueue.Enqueue(bullet);
        }
        
        private void EnableColliders<T>() where T : Collider
        {
            T[] colliders = GetComponentsInChildren<T>(true); // true includes inactive children
            foreach (T collider in colliders)
            {
                collider.enabled = true;
            }
        }
    }
    
    
}