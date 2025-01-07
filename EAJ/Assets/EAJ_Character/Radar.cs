using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace EAJ
{
    public class Radar : MonoBehaviour
    {
        [Header("Radar Settings")]
        [Tooltip("Radius of the radar sphere.")]
        public float RadarRadius = 5f;

        [Tooltip("Prefab for the radar object.")]
        public GameObject RadarObjectPrefab;

        private List<GameObject> radarObjects = new List<GameObject>();

        private PlayerUI PlayerUI;
        private WeaponSystem WeaponSystem;

        private void Start()
        {
            PlayerUI = FindObjectOfType<PlayerUI>();
        }
        
        private void Update()
        {
            if (WeaponSystem == null)
            {
                WeaponSystem = FindObjectOfType<WeaponSystem>();
                return;
            }
            
            ClearRadarObjects();

            foreach (Enemy enemy in EAJ_Manager.GetInstance().EnemySpawner.ActiveEnemies)
            {
                if (enemy == PlayerUI.EnemyLockedOn)
                {
                    continue;
                }
                
                Vector3 direction = enemy.transform.position - (transform.position + transform.up * 2);
                Vector3 radarPosition = CalculatePositionOnSphere(direction.normalized);
                SpawnRadarObject(radarPosition, direction, enemy.transform.position);
            }
        }

        private void ClearRadarObjects()
        {
            foreach (GameObject radarObject in radarObjects)
            {
                Destroy(radarObject);
            }

            radarObjects.Clear();
        }

        private Vector3 CalculatePositionOnSphere(Vector3 direction)
        {
            // The radar object position is calculated based on the player's position
            Vector3 localPosition = direction * RadarRadius;

            return (transform.position + transform.up * 2) + localPosition;
        }

        private void SpawnRadarObject(Vector3 position, Vector3 direction, Vector3 radarObjLocation)
        {
            Vector3 directionToEnemy = (radarObjLocation - PlayerUI.PlayerRef.transform.position).normalized;
            float dotProduct = Vector3.Dot(PlayerUI.PlayerRef.transform.forward, directionToEnemy);

            
            if (dotProduct > 0.8f && Vector3.Distance(PlayerUI.PlayerRef.transform.position, radarObjLocation) < WeaponSystem.LockOnDistance)
            {
                return;
            }

            GameObject radarObject = Instantiate(RadarObjectPrefab, position, Quaternion.LookRotation(-Camera.main.transform.forward, direction));
            radarObject.transform.SetParent(transform);
            TextMeshProUGUI distanceText = radarObject.GetComponentInChildren<Canvas>().transform.Find("DistanceText").GetComponent<TextMeshProUGUI>();
            distanceText.text = Mathf.RoundToInt(Vector3.Distance(PlayerUI.PlayerRef.transform.position, radarObjLocation)).ToString() + "m";

            // Scale the radar object based on the dot product
            float scale = Mathf.Lerp(0.1f, 1f, (dotProduct + 1f) / 2f);
            radarObject.transform.localScale = new Vector3(scale, scale, scale);

            radarObjects.Add(radarObject);
        }
    }
}