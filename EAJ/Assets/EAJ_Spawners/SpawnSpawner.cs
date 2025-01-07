using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EAJ
{
    public class SpawnSpawner : MonoBehaviour
    {
        [Header("Spawner Settings")] 
        
        public GameObject SpawnObj; //can be a bullet or another spawner

        public Enemy Source;

        [Header("Ring Settings")] [Tooltip("Is the spawner aimed at the player?")]
        public bool bIsAimed = true;

        [Tooltip("Velocity of the spawner.")] 
        public Vector3 Velocity = Vector3.zero;

        [Tooltip("Rotation of the spawner.")]
        public Vector3 Rotation = Vector3.zero;
        
        [Tooltip("Position offset of the ring.")]
        public Vector3 Offset = Vector3.zero;

        [Tooltip("Rotation of the ring.")] 
        public Vector3 SpawnRotation = Vector3.zero;

        [Tooltip("Number of Bullets Including Skipped Bulllets.")] 
        public int nBullets = 0;

        [Header("Advanced")]
        [Tooltip("Number of times the pattern should repeat, 0 means it will not repeat at all.")]
        public int NumRepeats = 0;

        [Tooltip("Skips spawning every n objects")]
        public int SkipInterval = 0;

        [Tooltip("When skipping, how many objects to skip")]
        public int SkipStrideAmt = 1;

        [Header("Timer Settings")] [Tooltip("Time between spawn calls.")]
        public float CooldownTime = 1f;

        [Tooltip("Delay between each spawned objects.")]
        public float SpawnDelay = 0f;

        [Tooltip("Time between repeats.")] 
        public float RepeatDelay = .1f;

        



        //*****************
        //private fields
        //*****************
        protected Coroutine SpawnCoro;
        protected bool bIsSpawning = false;
        protected float CurrentCooldown = 0f;
        protected int SkipStride;
        
        
        /// <summary>
        /// Events
        /// </summary>
        public System.Action OnSpawnFinished;


        protected virtual void Awake()
        {
            NumRepeats++; //repeats should be 1 higher than the user inputs since we need to spawn at least once
            SkipStride = SkipStrideAmt;
        }
        
        protected virtual void Start()
        {
            
        }

        protected void Update()
        {
            
            if (Source != null && Source.Health <= 0)
            {
                return;
            }
            
            HandleSpawning();
        }

        private void HandleSpawning()
        {
            if (!bIsSpawning) //if we aren't spawning
            {
                CurrentCooldown -= Time.deltaTime;

                if (CurrentCooldown <= 0f) //cooldown is complete
                {
                    StartCoroutine(nameof(Spawn));
                }
            }
        }

        protected virtual IEnumerator Spawn()
        {
            bool bSkipping = false;
            bIsSpawning = true;

            InitializeSpawner();

            for (int i = 0; i < NumRepeats; i++)
            {
                if (!bIsSpawning)
                {
                    break;
                }

                for (int j = 0; j < nBullets; j++)
                {
                    if (SkipInterval > 0 && (j + 1) % SkipInterval == 0)
                    {
                        bSkipping = true; //set skip flag
                    }

                    if (bSkipping)
                    {
                        SkipStride--;

                        if (SkipStride <= 0)
                        {
                            bSkipping = false; //reset skip flag once skip stride is over
                        }

                        continue;
                    }

                    Vector3 spawnLocation = GetBulletSpawnLocation(j);
                    Quaternion spawnRotation = Quaternion.Euler(GetBulletSpawnRotation(j));
                    
                    if (SpawnDelay > 0)
                    {
                        yield return new WaitForSeconds(SpawnDelay);
                    }
                        
                    if (!bIsSpawning)
                    {
                        continue;
                    }
                    
                    GameObject spawnedObject = Instantiate(SpawnObj, spawnLocation, spawnRotation); //what is angle?


                    HandleModifiers();
                }
                Reset();
                SkipStride = SkipStrideAmt;
                yield return new WaitForSeconds(RepeatDelay);
            }

            OnSpawnFinished?.Invoke();
            bIsSpawning = false;
            CurrentCooldown = CooldownTime;
        
        }

        protected virtual Vector3 GetBulletSpawnRotation(int currenetBulletNumber)
        {
            throw new System.NotImplementedException();
        }

        protected virtual Vector3 GetBulletSpawnLocation(int currentBulletNumber)
        {
            throw new System.NotImplementedException();
        }

        protected virtual void InitializeSpawner()
        {
        }

        protected virtual void HandleModifiers()
        {
            throw new System.NotImplementedException();
        }

        protected virtual void Reset()
        {
            throw new System.NotImplementedException();
        }

        void OnDrawGizmos()
        {
            DrawGizmos();
          
        }

        protected virtual void DrawGizmos()
        {
            
        }
    }
}