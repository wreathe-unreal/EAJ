using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;

namespace EAJ
{
    public class LaserBeam : MonoBehaviour
    {
        public int MinDamage;
        public int MaxDamage;
        private Quaternion Rotation;
        public Vector3 LeftOffset;
        public Vector3 RightOffset;
        
        public LineRenderer LR_Left;
        public LineRenderer LR_ReticleLeft;
        public LineRenderer LR_Right;
        public LineRenderer LR_ReticleRight;

        public Enemy NearestLeftLaserEnemy;
        public Enemy NearestRightLaserEnemy;
        
        public Material M_Reticle;
        public Material M_ReticleLocked;

        private PlayerUI PlayerUI;
        private WeaponSystem WeaponInput;
        private Camera MainCamera;
        
        public float ReticleSmoothTime = 0.1f; // Smoothing time for reticle movement

        private Vector2 leftReticleVelocity;
        private Vector2 rightReticleVelocity;

        [Tooltip("Length of the beam renderer")]
        public float BeamLength = 100f;

        [Tooltip("Speed of beam lerp rotation")]
        public float BeamRotateLerpSpeed = 2.5f;

        public bool bLeftLock = false;
        public bool bRightLock = false;
        public bool bAnimateUV = true;
        public float UVTime = -3f;

        private float AccumulatedDamageLeft = 0f;
        private float AccumulatedDamageRight = 0f;
        
        public float DamageInterval = 0.3f;
        private float NextDamageTime = 0f;

        public System.Action OnActiveLeft;
        public System.Action OnActiveRight;
        public System.Action OnBurnLeft;
        public System.Action OnBurnRight;
        public System.Action OnActiveLeftEnd;
        public System.Action OnActiveRightEnd;
        public System.Action OnBurnLeftEnd;
        public System.Action OnBurnRightEnd;
        


        // Start is called before the first frame update
        void Start()
        {
            PlayerUI = FindObjectOfType<PlayerUI>();
            MainCamera = FindObjectOfType<Camera>();

        }
        private float AnimateUVTime;

        // Update is called once per frame
        void Update()
        {
            if (WeaponInput == null)
            {
                SetupLasers();
                return;
            }

            LeftOffset = transform.InverseTransformPoint(WeaponInput.LaserHardPoints[0].position);
            RightOffset = transform.InverseTransformPoint(WeaponInput.LaserHardPoints[1].position);
            
            LR_Left.SetPosition(0, LeftOffset);
            LR_ReticleLeft.SetPosition(0, LeftOffset);
            LR_Right.SetPosition(0, RightOffset);
            LR_ReticleRight.SetPosition(0, RightOffset);
            
            HandleLaserInput();

            HandleLaserVisuals();

            if (Time.time >= NextDamageTime)
            {
                ApplyDamageLeft(NearestLeftLaserEnemy);
                ApplyDamageRight(NearestLeftLaserEnemy);
                NextDamageTime = Time.time + DamageInterval;
            }

        }
        
        private void SetupLasers()
        {
            if (WeaponInput == null)
            {
                WeaponInput = FindObjectOfType<WeaponSystem>();

                if (WeaponInput != null)
                {
                    
                    LR_Left.SetPosition(0, LeftOffset);
                    LR_Left.SetPosition(1, LeftOffset + transform.forward * BeamLength);
                    
                    LR_ReticleLeft.SetPosition(0, LeftOffset);
                    LR_ReticleLeft.SetPosition(1, LeftOffset + transform.forward * BeamLength);
            
                    LR_Right.SetPosition(0, RightOffset);
                    LR_Right.SetPosition(1, RightOffset + transform.forward * BeamLength);
                    
                    LR_ReticleRight.SetPosition(0, RightOffset);
                    LR_ReticleRight.SetPosition(1, RightOffset + transform.forward * BeamLength);
                }

                LR_Left.useWorldSpace = false;
                LR_Right.useWorldSpace = false;

                LR_ReticleLeft.useWorldSpace = false;
                LR_ReticleRight.useWorldSpace = false;
                return;
            }
        }

        private void HandleLaserInput()
        {
            // Get the target directions based on input
            Vector3 directionLeft = new Vector3(WeaponInput.BeamLeftX, WeaponInput.BeamLeftY, 0f);
            Vector3 directionRight = new Vector3(WeaponInput.BeamRightX, WeaponInput.BeamRightY, 0f);
    
            Vector3 targetPositionLeft = LeftOffset + (Vector3.forward + directionLeft) * BeamLength;
            Vector3 targetPositionRight = RightOffset + (Vector3.forward + directionRight) * BeamLength;

            // Get the current positions of the line renderer endpoints
            Vector3 currentPositionLeft = LR_ReticleLeft.GetPosition(1);
            Vector3 currentPositionRight = LR_ReticleRight.GetPosition(1);

            
            Vector3 newPositionLeft = Vector3.Lerp(currentPositionLeft, targetPositionLeft, BeamRotateLerpSpeed * Time.deltaTime);
            Vector3 newPositionRight = Vector3.Lerp(currentPositionRight, targetPositionRight, BeamRotateLerpSpeed * Time.deltaTime);

            // Set the new positions
            LR_Left.SetPosition(1, newPositionLeft);
            LR_Right.SetPosition(1, newPositionRight);
            
            // Set the new positions
            LR_ReticleLeft.SetPosition(1, newPositionLeft);
            LR_ReticleRight.SetPosition(1, newPositionRight);


            if (directionLeft.magnitude == 0f)
            {
                LR_Left.SetPosition(1, LeftOffset);
                LR_ReticleLeft.SetPosition(1, LeftOffset);
                
            }

            if (directionRight.magnitude == 0f)
            {
                LR_Right.SetPosition(1, RightOffset);
                LR_ReticleRight.SetPosition(1, RightOffset);
            }

            FindNearestEnemies();

            ValidateEnemies();
            
            GetAngleToLeftEnemy(targetPositionLeft);
            GetAngleToRightEnemy(targetPositionRight);


            TrySnapLasers(newPositionLeft, newPositionRight, targetPositionLeft, targetPositionRight);

            if (directionLeft.magnitude != 0f)
            {
                if (WeaponInput.LeftLaserPressed && bLeftLock)
                {
                    OnActiveLeftEnd?.Invoke();
                    OnBurnLeft?.Invoke();
                }
                else
                {
                    OnBurnLeftEnd?.Invoke();
                    OnActiveLeft?.Invoke();
                }
            }
            else
            {
                OnActiveLeftEnd?.Invoke();
                OnBurnLeftEnd?.Invoke();
            }
            
            
            if (directionRight.magnitude != 0f)
            {
                if (WeaponInput.RightLaserPressed && bRightLock)
                {
                    OnActiveRightEnd?.Invoke();
                    OnBurnRight?.Invoke();
                }
                else
                {
                    OnBurnRightEnd?.Invoke();
                    OnActiveRight?.Invoke();
                }
            }
            else
            {
                OnActiveRightEnd?.Invoke();
                OnBurnRightEnd?.Invoke();
            }
            
        }

        private void ValidateEnemies()
        {

            if (NearestLeftLaserEnemy != null)
            {
                
                float distanceFromPlayerToLeftEnemy = Vector3.Distance(EAJ_Manager.GetInstance().PlayerRef.transform.position, NearestLeftLaserEnemy.transform.position);
            
                if (distanceFromPlayerToLeftEnemy > 125f)
                {
                    NearestLeftLaserEnemy = null;
                    bLeftLock = false;
                }
            }

            if (NearestRightLaserEnemy != null)
            {
               
                float distanceFromPlayerToRightEnemy = Vector3.Distance(EAJ_Manager.GetInstance().PlayerRef.transform.position, NearestRightLaserEnemy.transform.position);
            
                if(distanceFromPlayerToRightEnemy > 125f)
                {
                    NearestRightLaserEnemy = null;
                    bRightLock = false;
                } 
            }

        }

        private float GetAngleToLeftEnemy(Vector3 beamPos)
        {
            if (NearestLeftLaserEnemy == null)
            {
                return float.MaxValue;
            }
            
            Vector3 localLeftEnemyPos = transform.InverseTransformPoint(NearestLeftLaserEnemy.transform.position);

            Vector3 localEnemyPositionLeft = localLeftEnemyPos + LeftOffset;

            // Calculate directions from current positions to new target positions in local space
            Vector3 localDirectionToNewPositionLeft = beamPos + LeftOffset;

            // Calculate directions from current positions to enemy position in local space
            Vector3 directionToEnemyLeft = (localEnemyPositionLeft - LeftOffset).normalized;

            // Calculate angles between the directions
            float angleLeft = Vector3.Angle(localDirectionToNewPositionLeft.normalized, directionToEnemyLeft);
            
            return angleLeft;
        }

        private float GetAngleToRightEnemy(Vector3 beamPos)
        {
            
            if (NearestRightLaserEnemy == null)
            {
                bRightLock = false;
                return float.MaxValue;
            }
            
            Vector3 localRightEnemyPos = transform.InverseTransformPoint(NearestRightLaserEnemy.transform.position);

            Vector3 localEnemyPositionRight = localRightEnemyPos + RightOffset;

            Vector3 localDirectionToNewPositionRight = beamPos + RightOffset;

            Vector3 directionToEnemyRight = (localEnemyPositionRight - RightOffset).normalized;

            float angleRight = Vector3.Angle(localDirectionToNewPositionRight.normalized, directionToEnemyRight);
            
            return angleRight;
        }

        private void FindNearestEnemies()
        {
            NearestLeftLaserEnemy = null;
            NearestRightLaserEnemy = null;
            
            foreach (Enemy e in EAJ_Manager.GetInstance().EnemySpawner.ActiveEnemies)
            {
                if (NearestLeftLaserEnemy == null)
                {
                    NearestLeftLaserEnemy = e;
                }

                if (NearestRightLaserEnemy == null)
                {
                    NearestRightLaserEnemy = e;
                }


                float distanceFromLaserEndpointToNewEnemy = Vector3.Distance(transform.InverseTransformPoint(LR_Left.GetPosition(1)), e.transform.position);
                float distanceFromLaserEndpointToLeftLockedEnemy = Vector3.Distance(transform.InverseTransformPoint(LR_Left.GetPosition(1)), NearestLeftLaserEnemy.transform.position);
                float distanceFromLaserEndpointToRightLockedEnemy = Vector3.Distance(transform.InverseTransformPoint(LR_Right.GetPosition(1)), NearestRightLaserEnemy.transform.position);
                
                if ( distanceFromLaserEndpointToNewEnemy < distanceFromLaserEndpointToLeftLockedEnemy)
                {
                    NearestLeftLaserEnemy = e;
                }
                    
                if (distanceFromLaserEndpointToNewEnemy < distanceFromLaserEndpointToRightLockedEnemy)
                {
                    NearestRightLaserEnemy = e;
                }
                    
            }
        }
        
        

        private void TrySnapLasers(Vector3 newPosLeft, Vector3 newPosRight, Vector3 targetPositionLeft, Vector3 targetPositionRight)
        {
            if (NearestLeftLaserEnemy != null)
            {
                float currentAngle = GetAngleToLeftEnemy(newPosLeft);
                float targetAngle = GetAngleToLeftEnemy(targetPositionLeft);
                Vector3 localLeftEnemyPos = transform.InverseTransformPoint(NearestLeftLaserEnemy.transform.position);
                TrySnapLeftLaser(Mathf.Max(currentAngle, targetAngle), localLeftEnemyPos, NearestLeftLaserEnemy);

            }

            if (NearestRightLaserEnemy != null)
            {
                float currentAngle = GetAngleToRightEnemy(newPosRight);
                float targetAngle = GetAngleToRightEnemy(targetPositionRight);
                Vector3 localRightEnemyPos = transform.InverseTransformPoint(NearestRightLaserEnemy.transform.position);
                TrySnapRightLaser(Mathf.Max(currentAngle, targetAngle), localRightEnemyPos, NearestRightLaserEnemy);
            }
        }

        void TrySnapLeftLaser(float angle, Vector3 localEnemyPosition, Enemy e)
        {
            if (e == null)
            {
                bLeftLock = false;
                return;
            }
            // Snap to target if within angle threshold
            if (angle < 15f)
            {
                LR_Left.SetPosition(1, localEnemyPosition);
                LR_ReticleLeft.SetPosition(1, localEnemyPosition);
                
                bLeftLock = true;

                if (WeaponInput.LeftLaserPressed)
                {
                    AccumulatedDamageLeft += Random.Range(-MinDamage * Time.deltaTime, -MaxDamage * Time.deltaTime);
                }
            }
            else
            {
                bLeftLock = false;
            }
        }
        
        void TrySnapRightLaser(float angle, Vector3 localEnemyPosition, Enemy e)
        {
            if (e == null)
            {
                bRightLock = false;
                return;
            }
            // Snap to target if within angle threshold
            if (angle < 15f)
            {
                LR_Right.SetPosition(1, localEnemyPosition);
                LR_ReticleRight.SetPosition(1, localEnemyPosition);
                
                bRightLock = true;
                
                if (WeaponInput.RightLaserPressed)
                {
                    AccumulatedDamageRight += Random.Range(-MinDamage * Time.deltaTime, -MaxDamage * Time.deltaTime);
                }
            }
            else
            {
                bRightLock = false;
            }
        }

        private void ApplyDamageLeft(Enemy e)
        {
            if (e != null && AccumulatedDamageLeft < 0f)
            {
                e.ModifyHealth(AccumulatedDamageLeft, AccumulatedDamageLeft);
            }

            AccumulatedDamageLeft = 0f;
        }
        
        private void ApplyDamageRight(Enemy e)
        {
            if (e != null  && AccumulatedDamageRight < 0f)
            {
                e.ModifyHealth(AccumulatedDamageRight, AccumulatedDamageRight);
            }

            AccumulatedDamageRight = 0f;
        }
        
        

        private void HandleLaserVisuals()
        {
            if (bLeftLock)
            {
                LR_ReticleLeft.material = M_ReticleLocked;
            }
            else
            {
                LR_ReticleLeft.material = M_Reticle;
            }

            if (bRightLock)
            {
                LR_ReticleRight.material = M_ReticleLocked;
            }
            else
            {
                LR_ReticleRight.material = M_Reticle;
            }
            
            if (WeaponInput.LeftLaserPressed && bLeftLock)
            {
                LR_Left.gameObject.SetActive(true);
                LR_ReticleLeft.gameObject.SetActive(false);   
            }
            else
            {
                LR_Left.gameObject.SetActive(false);
                LR_ReticleLeft.gameObject.SetActive(true);
            }

            if (WeaponInput.RightLaserPressed && bRightLock)
            {
                LR_Right.gameObject.SetActive(true);
                LR_ReticleRight.gameObject.SetActive(false);
            }
            else
            {
                LR_Right.gameObject.SetActive(false);
                LR_ReticleRight.gameObject.SetActive(true);
            }
            
            
            
            // Animate texture UV
            if (bAnimateUV)
            {
                AnimateUVTime += Time.deltaTime;

                if (AnimateUVTime > 1.0f)
                {
                    AnimateUVTime = 0f;
                }
                
                var v = AnimateUVTime * UVTime; // + initialBeamOffset;
                
                LR_Left.material.SetVector("_Offset", new Vector2(v, 0));
                LR_Right.material.SetVector("_Offset", new Vector2(v, 0));

            }
        }
    }
}
