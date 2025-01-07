using System.Collections.Generic;
using System.Numerics;
using AVM;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using Vector2 = UnityEngine.Vector2;

namespace EAJ
{
    public class WeaponSystem : MonoBehaviour
    {
        public int LockOnDistance = 125;
        
        private Transform RocketLauncher;
        
        [HideInInspector, DoNotSerialize]
        public List<Transform> LaserHardPoints = new List<Transform>(); //index 0 left 1 right
        [HideInInspector, DoNotSerialize]
        public List<Transform> FlareHardPoints = new List<Transform>();//index 0 left-mid 1 right-mid 2 left-bottom 3 right-bottom
        
        public SixDOFController PlayerController;
        public GameObject RocketPrefab;
        public GameObject FlarePrefab; // New prefab for flares
        

        [Header("Cooldown Settings")]
        public float RocketFireCooldown = 1f; // Cooldown time in seconds
        public float FlareFireCooldown = 0.1f; // Cooldown time in seconds for flares


        private float RocketFireTime;

        private float FlareFireTime = 0f;
        private int CurrentFlareHardPointIndex = 0;

        
        [HideInInspector, DoNotSerialize]
        public bool MenuReadyPressed = false;
        [HideInInspector, DoNotSerialize]
        public bool SpearPressed = false;
        [HideInInspector, DoNotSerialize]
        public bool LeftLaserPressed = false;
        [HideInInspector, DoNotSerialize]
        public bool RightLaserPressed = false;
        [HideInInspector, DoNotSerialize]
        public bool FireFlarePressed = false; // New input flag for firing flares
        [HideInInspector, DoNotSerialize]
        public bool FireRocketPressed = false;
        [HideInInspector, DoNotSerialize]
        public bool ShieldPressed = false;
        
        public float BeamLeftX = 0f;
        public float BeamLeftY = 0f;
        public float BeamRightX = 0f;
        public float BeamRightY = 0f;

        public bool bLockedOn;

        private ShieldBoost ShieldBoost;
        private bool bInitialized = false;
        public System.Action OnRocketFired;
        public System.Action OnFlareFired;

        private void Start()
        {
            
        }

        private void Update()
        {
            if (SceneManager.GetActiveScene().name == "EAJ_Menu")
            {
                return;
            }
            
            if (bInitialized == false)
            {
                ShieldBoost = GetComponent<ShieldBoost>();
                PlayerController = FindObjectOfType<SixDOFController>();
                gameObject.transform.SetParent(PlayerController.transform);
                RocketFireTime = -RocketFireCooldown;
                LaserHardPoints.Add(GameObject.Find("HardpointTopLeft").transform);
                LaserHardPoints.Add(GameObject.Find("HardpointTopRight").transform);
                FlareHardPoints.Add(GameObject.Find("HardpointMidLeft").transform);
                FlareHardPoints.Add(GameObject.Find("HardpointMidRight").transform);
                FlareHardPoints.Add(GameObject.Find("HardpointBottomLeft").transform);
                FlareHardPoints.Add(GameObject.Find("HardpointBottomRight").transform);
                RocketLauncher = GameObject.Find("RocketLauncher").transform;
                bInitialized = true;
            }
            
            if (ShieldBoost != null)
            {
                if (ShieldBoost.bShieldActive || ShieldBoost.bBoostActive)
                {
                    return;
                }
            }

            FireFlare();
        }
        public void FireFlare()
        {
            
            if (FireFlarePressed && Time.time >= FlareFireTime + FlareFireCooldown)
            {
                Transform currentHardPoint = FlareHardPoints[CurrentFlareHardPointIndex];
                GameObject go = Instantiate(FlarePrefab, currentHardPoint.position, currentHardPoint.rotation);
                CurrentFlareHardPointIndex = (CurrentFlareHardPointIndex + 1) % FlareHardPoints.Count;
                AdvancedMissile missileComponent = go.GetComponent<AdvancedMissile>();
                OnFlareFired?.Invoke();
                
                if (!bLockedOn)
                {
                    missileComponent.m_sight = Vector2.zero;

                }

                FlareFireTime = Time.time; // Reset the cooldown timer
            }
        }
        
        public void FireRocket()
        {
            
            if (FireRocketPressed && Time.time >= RocketFireTime + RocketFireCooldown)
            {
                GameObject go = Instantiate(RocketPrefab, RocketLauncher.position, PlayerController.transform.rotation);
                AdvancedMissile missileComponent = go.GetComponent<AdvancedMissile>();
                OnRocketFired?.Invoke();

                if (!bLockedOn)
                {
                    missileComponent.m_sight = Vector2.zero;

                }
                
                RocketFireTime = Time.time; // Reset the cooldown timer
            }
        }

        public void OnMenuReady(InputValue value)
        {
            MenuReadyPressed = value.isPressed;
        }

        public void OnLeftLaser(InputValue value)
        {
            LeftLaserPressed = value.isPressed;
        }
        
        public void OnFireFlare(InputValue value)
        {
            FireFlarePressed = value.isPressed;
        }
        
        public void OnSpear(InputValue value)
        {
            SpearPressed = value.isPressed;
        }
        public void OnRightLaser(InputValue value)
        {
            RightLaserPressed = value.isPressed;
        }
        
        public void OnPlayerTwoShield(InputValue value)
        {
            ShieldPressed = value.isPressed;
        }
        public void OnFireRocket(InputValue value)
        {
            FireRocketPressed = value.isPressed;
        }
        
        public void OnBeamLeftX(InputValue value)
        {
            BeamLeftX = value.Get<float>();
        }
        public void OnBeamLeftY(InputValue value)
        {
            BeamLeftY = value.Get<float>();
        }
        public void OnBeamRightX(InputValue value)
        {
            BeamRightX = value.Get<float>();
        }
        public void OnBeamRightY(InputValue value)
        {
            BeamRightY = value.Get<float>();
        }
        
    }
}