using System.Collections;
using System.Collections.Generic;
using AVM;
using UnityEngine;

namespace EAJ
{
    public class Missile : MonoBehaviour
    {
        public int MinDamage;
        public int MaxDamage;

        private Vector3 SpawnPosition;
        // Start is called before the first frame update
        void Start()
        {
            SpawnPosition = transform.position;

        }

        // Update is called once per frame
        void Update()
        {

        }

        void OnTriggerEnter(Collider other)
        {
            Debug.Log("Collision detected with: " + other.gameObject.name);
            if (other.gameObject.CompareTag("Enemy"))
            {
                Debug.Log("Collided with enemy: " + other.gameObject.name);
                Enemy enemy = other.gameObject.GetComponentInChildren<Enemy>();
                
                if (enemy != null)
                {
                    enemy.ModifyHealth(-MinDamage, -MaxDamage);
                    Debug.Log("Enemy health after damage: " + enemy.Health);
                    Destroy(gameObject);
                }
                else
                {
                    Debug.LogWarning("No Enemy component found on collided object");
                }
            }
        }
    }
}
