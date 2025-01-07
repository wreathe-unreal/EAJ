using System;
using System.Collections;
using System.Collections.Generic;
using AVM;
using DamageNumbersPro;
using EAJ;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public enum EEnemySpawnGroup
{
    NONE,
    ALPHA,
    BETA,
    GAMMA,
    DELTA
}
public class Enemy : MonoBehaviour
{

    public EEnemySpawnGroup EnemySpawnGroup;
    public DamageNumber DNP;
    public RectTransform GUI;
    public float Height;
    public bool bVibrateOnDeath = true;
    public static SixDOFController PlayerController;
    public Slider HealthBar;
    public float HEALTH_MAX = 1000;
    public float Health;
    public int ScoreValue;
    public int CollisionScorePenalty = 25;
    public GameObject DeathPrefab;

    public float TimeAlive = 0f;
    private bool bVibrate = false;
    private float VibrateTime = .5f; // Duration for which the vibration will last
    private float VibrateTimeRemaining;
    private PlayerUI PlayerUI;

    public System.Action OnDeath;
    
    
    // Start is called before the first frame update
    void Start()
    {
        PlayerUI = FindObjectOfType<PlayerUI>();
        HealthBar = PlayerUI.EnemyHealthBar;
        PlayerController = FindObjectOfType<SixDOFController>();
        Health = HEALTH_MAX;
        GUI = HealthBar.gameObject.GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Health > 0f)
        {
            TimeAlive += Time.deltaTime;
        }

        if (bVibrate)
        {
            //VibrateControllersBasedOnProximity();
        }
    }

    public bool ModifyHealth(float MinDamage, float MaxDamage) //true if still alive
    {
        float Damage = Random.Range(MinDamage, MaxDamage);
        Health = Mathf.Clamp(Health + Damage, 0, HEALTH_MAX);

        
        Vector3 targetWorldPosition = transform.position + (transform.up * (Height * 1.5f));
        Vector3 screenPosition = Camera.main.WorldToScreenPoint(targetWorldPosition);

        RectTransform canvasRectTransform = GUI.GetComponent<RectTransform>();
        Vector2 canvasPosition;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRectTransform, screenPosition, Camera.main, out canvasPosition);

        DamageNumber damageNumber = DNP.SpawnGUI(GUI, canvasPosition, Mathf.Abs(Damage));
        
        if (Health <= 0f)
        {
            bool isMeleeKill = Mathf.Approximately(Damage, HEALTH_MAX);
            Death(isMeleeKill);
            
            return false;
        }
        
        return true;
    }

    void Death(bool bMeleeKill)
    {
        if (bMeleeKill)
        {
            PlayerUI.ModifyScore(ScoreValue * 2);
        }
        else
        {
            PlayerUI.ModifyScore(ScoreValue);
        }

        EAJ_Manager.GetInstance().EnemySpawner.ActiveEnemies.Remove(this);
        
        GameObject deathPrefab = Instantiate(DeathPrefab, transform.position, transform.rotation);
        Destroy(deathPrefab, 1.5f);
        
        gameObject.SetActive(false);
        
        AdvancedMissile MissileController = transform.parent.GetComponent<AdvancedMissile>();
            
        if( MissileController != null)
        {
            MissileController.enabled = false;
        }

        BoxCollider[] boxColliders = transform.parent.GetComponents<BoxCollider>();
        foreach (BoxCollider bc in boxColliders)
        {
            bc.enabled = false;
        }
        
        SphereCollider[] sphereColliders = transform.parent.GetComponents<SphereCollider>();
        foreach (SphereCollider sc in sphereColliders)
        {
            sc.enabled = false;
        }
        
        // if (bVibrateOnDeath)
        // {
        //     StartCoroutine(VibrateForDuration(0.25f));
        // }

    }

    private IEnumerator VibrateForDuration(float duration)
    {
        bVibrate = true;
        yield return new WaitForSeconds(duration);
        bVibrate = false;
    }

    public float GetNormalizedHealth()
    {
        return Mathf.Clamp(Health / HEALTH_MAX, 0f, 1f);
    }

    public static Enemy FindLockOn()
    {
        
        List<Enemy> nearEnemies = new List<Enemy>();

        foreach (Enemy e in EAJ_Manager.GetInstance().EnemySpawner.ActiveEnemies)
        {
            if (Vector3.Distance(PlayerController.transform.position, e.transform.position) < PlayerUI.GetInstance().Weapons.LockOnDistance)
            {
                nearEnemies.Add(e);
            }
        }

        Enemy lockOnEnemy = null;
        float highestDot = 0.72f; // Threshold for lock-on

        foreach (Enemy e in nearEnemies)
        {
            Vector3 directionToEnemy = (e.transform.position - PlayerController.transform.position).normalized;
            float dotProduct = Vector3.Dot(PlayerController.transform.forward, directionToEnemy);

            if (dotProduct > highestDot)
            {
                highestDot = dotProduct;
                lockOnEnemy = e;
            }
        }

        return lockOnEnemy;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, Height / 2f);
    }
    
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.GetComponent<SixDOFController>() != null)
        {
            PlayerUI.ModifyScore(-CollisionScorePenalty);
        }
    }

    private void VibrateControllersBasedOnProximity()
    {
        if (PlayerController != null)
        {
            float distance = Vector3.Distance(PlayerController.transform.position, transform.position);
            float initialIntensity = Mathf.Clamp01(1.0f - (distance / 75f));

            if (initialIntensity > 0)
            {
                VibrateTimeRemaining = VibrateTime;
            }

            if (VibrateTimeRemaining > 0)
            {
                VibrateTimeRemaining -= Time.deltaTime;
                float vibrationIntensity = Mathf.Lerp(0, initialIntensity, VibrateTimeRemaining / VibrateTime);

                foreach (Gamepad gamepad in Gamepad.all)
                {
                    if (gamepad != null)
                    {
                        gamepad.SetMotorSpeeds(vibrationIntensity, vibrationIntensity);
                    }
                }
            }
            else
            {
                print("vibration over");
                foreach (Gamepad gamepad in Gamepad.all)
                {
                    if (gamepad != null)
                    {
                        print("reset haptics");
                        gamepad.SetMotorSpeeds(0, 0);
                        gamepad.ResetHaptics();
                    }
                }
            }
        }
    }

}
