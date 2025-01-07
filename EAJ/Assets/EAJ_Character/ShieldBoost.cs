using System.Collections;
using System.Collections.Generic;
using EAJ;
using TMPro;
using UnityEngine;

public class ShieldBoost : MonoBehaviour
{
    [Header("Project References:")]
    [SerializeField] private SixDOFMovementData SixDOFMovementValues = default;

    public GameObject ShieldObject;

    public float BoostSurgeBonus = 300f;

    public bool bBoostActive;
    public bool bShieldActive;

    public float ShieldTime = 2f;

    public float ShieldCooldown = 30f;

    private float OriginalSurge;
    private float OriginalStrafe;
    private float OriginalThrustDown;
    private float OriginalThrustUp;
    private float OriginalRoll;
    private float OriginalPitch;
    private float OriginalYaw;

    private Coroutine ShieldCoroutine;

    public InputManager PlayerOneInputs;
    public WeaponSystem PlayerTwoInputs;

    public TextMeshProUGUI ShieldCanvasText;
    public TextMeshProUGUI BoostCanvasText;

    public System.Action OnBoost;
    public System.Action OnBoostEnd;
    public System.Action OnShield;
    public System.Action OnShieldEnd;

    // Start is called before the first frame update
    void Start()
    {
        // Initialize the original movement values
        OriginalSurge = SixDOFMovementValues.SurgeForce;
        OriginalStrafe = SixDOFMovementValues.StrafeForce;
        OriginalThrustDown = SixDOFMovementValues.ThrustDownForce;
        OriginalThrustUp = SixDOFMovementValues.ThrustUpForce;
        OriginalRoll = SixDOFMovementValues.MaximumRollSpeed;
        OriginalPitch = SixDOFMovementValues.MaximumPitchSpeed;
        OriginalYaw = SixDOFMovementValues.MaximumYawSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        HandleBoost();

        if (bBoostActive)
        {
            return;
        }
        

        if (PlayerTwoInputs == null)
        {
            PlayerTwoInputs = FindObjectOfType<WeaponSystem>();
            return;
        }
        
        TryActivateShield();

        if (bShieldActive)
        {
            if (ShieldObject != null)
            {
                ShieldObject.transform.Rotate(Vector3.up, 180 * Time.deltaTime, Space.Self);
            }
        }
    }

    private void HandleBoost()
    {
        if (PlayerOneInputs.BoostInput && PlayerOneInputs.SurgeInput > 0f)
        {
            bBoostActive = true;
            BoostCanvasText.gameObject.SetActive(false);
            SixDOFMovementValues.SurgeForce += BoostSurgeBonus;
            
            SixDOFMovementValues.StrafeForce = 0f;
            SixDOFMovementValues.ThrustDownForce = 0;
            SixDOFMovementValues.ThrustUpForce = 0;
            SixDOFMovementValues.MaximumRollSpeed = 4f;
            SixDOFMovementValues.MaximumPitchSpeed = 0;
            SixDOFMovementValues.MaximumYawSpeed = 2f;
            OnBoost?.Invoke();
        }
        else
        {
            
            bBoostActive = false;
            BoostCanvasText.gameObject.SetActive(true);

            
            SixDOFMovementValues.SurgeForce = OriginalSurge;
            SixDOFMovementValues.StrafeForce = OriginalStrafe;
            SixDOFMovementValues.ThrustDownForce = OriginalThrustDown;
            SixDOFMovementValues.ThrustUpForce = OriginalThrustUp;
            SixDOFMovementValues.MaximumRollSpeed = OriginalRoll;
            SixDOFMovementValues.MaximumPitchSpeed = OriginalPitch;
            SixDOFMovementValues.MaximumYawSpeed = OriginalYaw;
            OnBoostEnd?.Invoke();

        }
        
    }

    private void TryActivateShield()
    {
        if (PlayerOneInputs.ShieldInput && PlayerTwoInputs.ShieldPressed == true && bShieldActive == false && ShieldCoroutine == null)
        {
            bShieldActive = true;
            ShieldCoroutine = StartCoroutine(ShieldState());
        }
    }

    private IEnumerator ShieldState()
    {
        // Set all values to zero
        SixDOFMovementValues.SurgeForce = 0;
        SixDOFMovementValues.StrafeForce = 0;
        SixDOFMovementValues.ThrustDownForce = 0;
        SixDOFMovementValues.ThrustUpForce = 0;
        SixDOFMovementValues.MaximumRollSpeed = 0;
        SixDOFMovementValues.MaximumPitchSpeed = 0;
        SixDOFMovementValues.MaximumYawSpeed = 0;

        // Activate shield game object
        if (ShieldObject != null)
        {
            ShieldObject.SetActive(true);
            ShieldCanvasText.gameObject.SetActive(false);
        }

        OnShield?.Invoke();
        
        yield return new WaitForSeconds(ShieldTime);

        // Deactivate shield game object
        if (ShieldObject != null)
        {
            ShieldObject.SetActive(false);
        }

        bShieldActive = false;

        // Restore original values
        SixDOFMovementValues.SurgeForce = OriginalSurge;
        SixDOFMovementValues.StrafeForce = OriginalStrafe;
        SixDOFMovementValues.ThrustDownForce = OriginalThrustDown;
        SixDOFMovementValues.ThrustUpForce = OriginalThrustUp;
        SixDOFMovementValues.MaximumRollSpeed = OriginalRoll;
        SixDOFMovementValues.MaximumPitchSpeed = OriginalPitch;
        SixDOFMovementValues.MaximumYawSpeed = OriginalYaw;

        yield return new WaitForSeconds(ShieldCooldown);
        ShieldCoroutine = null; // Allow reactivation after cooldown
        ShieldCanvasText.gameObject.SetActive(true);
        OnShieldEnd?.Invoke();
    }
}