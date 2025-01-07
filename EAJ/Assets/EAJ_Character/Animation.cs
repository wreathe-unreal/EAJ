using System.Collections;
using System.Collections.Generic;
using EAJ;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Animation : MonoBehaviour
{
    public Animator EAJ_Animator;
    private ShieldBoost ShieldBoost;
    private InputManager MovementInputs;
    private WeaponSystem WeaponInputs;
    private string SurgeParam = "Surge";
    private string StrafeParam = "Strafe";
    private string ShieldParam = "Shield";
    private string BoostParam = "Boost";
    private string ThrustUpParam = "ThrustUp";
    private string ThrustDownParam = "ThrustDown";
    private string SpearAnimPlayingParam = "IsSpearAnimPlaying";
    private string RocketLaunchAnimPlayingParam = "IsRocketLaunchAnimPlaying";

    private bool bWasRocketLaunchAnimPlaying = false;
    private bool bWasSpearAnimPlaying = false;
    
    // Start is called before the first frame update
    void Start()
    {
        EAJ_Animator = GetComponent<Animator>();
        MovementInputs = FindObjectOfType<InputManager>();
    }

    // Update is called once per frame
    void Update()
    {

        if (EAJ_Animator.runtimeAnimatorController.name != "AC_EAJ")
        {
            return;
        }
        
        if (MovementInputs == null)
        {
            MovementInputs = FindObjectOfType<InputManager>();
        }
        
        if (ShieldBoost == null)
        {
            ShieldBoost = transform.parent.GetComponent<ShieldBoost>();
        }
        
        if (WeaponInputs == null)
        {
            WeaponInputs = FindObjectOfType<WeaponSystem>();
        }
        
        if(WeaponInputs != null)
        {
            bool isSpearAnimationPlaying = EAJ_Animator.GetBool(SpearAnimPlayingParam);

            if (WeaponInputs.SpearPressed && !isSpearAnimationPlaying && !bWasSpearAnimPlaying)
            {
                EAJ_Animator.SetBool(SpearAnimPlayingParam, true); // Set the boolean to true
            }

            bool isRocketLaunchAnimationPlaying = EAJ_Animator.GetBool(RocketLaunchAnimPlayingParam);

            if (WeaponInputs.FireRocketPressed && !isRocketLaunchAnimationPlaying && !bWasRocketLaunchAnimPlaying)
            {
                EAJ_Animator.SetBool(RocketLaunchAnimPlayingParam, true);
            }
            
            bWasSpearAnimPlaying = false;
            bWasRocketLaunchAnimPlaying = false;
        }

        if (MovementInputs != null)
        {
          
            float thrustUp = MovementInputs.ThrustInput > 0 ? MovementInputs.ThrustInput : 0f;
            EAJ_Animator.SetFloat(ThrustUpParam, thrustUp);
        
        
            float thrustDown = MovementInputs.ThrustInput < 0 ? MovementInputs.ThrustInput : 0f;
            EAJ_Animator.SetFloat(ThrustDownParam, thrustDown);
        
            EAJ_Animator.SetFloat(SurgeParam, MovementInputs.SurgeInput);
            EAJ_Animator.SetFloat(StrafeParam, MovementInputs.StrafeInput);  
        }

        if (ShieldBoost != null)
        {
            
            EAJ_Animator.SetBool(ShieldParam, ShieldBoost.bShieldActive);
            EAJ_Animator.SetBool(BoostParam, ShieldBoost.bBoostActive);
        }
        

    }
    
    
    // This function should be called by an animation event at the end of the spear animation
    public void OnSpearAnimationEnd()
    {
        EAJ_Animator.SetBool(SpearAnimPlayingParam, false); // Reset the boolean when the animation ends
        bWasSpearAnimPlaying = true;
    }
    
    // This function should be called by an animation event at the end of the spear animation
    public void OnRocketLaunchAnimationEnd()
    {
        EAJ_Animator.SetBool(RocketLaunchAnimPlayingParam, false); // Reset the boolean when the animation ends
        bWasRocketLaunchAnimPlaying = true;
    }

    public void OnFireRocket()
    {
        WeaponInputs.FireRocket();
    }
}
