using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

namespace EAJ
{
    public class EAJ_Manager : MonoBehaviour
    {
        // Singleton instance
        public static EAJ_Manager Instance { get; private set; }
        public EnemySpawner EnemySpawner;
        public float TimeScale = 1.0f;
        public SixDOFController PlayerRef;

        void Awake()
        {
            // Implement singleton pattern
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject); // Ensure this instance persists across scenes
            }
            else
            {
                Destroy(gameObject); // Destroy duplicate instances
            }
        }

        // Start is called before the first frame update
        void Start()
        {
            
            if (EnemySpawner == null)
            {
                EnemySpawner = GetComponent<EnemySpawner>();
            }
            //AdjustTimeScale(.1f);
            // Any initialization code can go here
            
            //Bullets = FindObjectsOfType<Bullet>();
        }

        // Update is called once per frame
        void Update()
        {
            if (PlayerRef == null)
            {
                print("null");
                PlayerRef = FindObjectOfType<SixDOFController>();
            }
        }

        public static EAJ_Manager GetInstance()
        {
            return Instance;
        }

        public void AdjustTimeScale(float newTimeScale)
        {
            TimeScale = newTimeScale;
            Time.timeScale = TimeScale;
        }
    }
}
