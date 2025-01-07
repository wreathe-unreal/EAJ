using System.Collections;
using System.Collections.Generic;
using EAJ;
using UnityEngine;
using UnityEngine.Audio;

public class SFX : MonoBehaviour
{
    public int poolSize = 10; // Number of audio sources in the pool
    private List<AudioSource> AudioSources;
    private AudioSource SixDOFThrustAudioSource;
    [SerializeField] public AudioMixerGroup EffectsGroup;

    
    private SixDOFController PlayerController;
    private WeaponSystem WeaponController;
    private PlayerUI GUI;
    private Radar RadarGUI;
    private ShieldBoost ShieldBoost;
    private LaserBeam Lasers;

    [SerializeField] public AudioClip RocketLaunchSFX;
    [SerializeField] public AudioClip RocketExplodeSFX;
    [SerializeField] public AudioClip FlareSFX;
    [SerializeField] public AudioClip FlareCollideSFX;
    [SerializeField] public AudioClip LockOnSFX;
    [SerializeField] public AudioClip LockOffSFX;
    [SerializeField] public AudioClip EnemySpawnSFX;
    [SerializeField] public AudioClip LaserLeftSFX;
    [SerializeField] public AudioClip LaserLeftBurnSFX;
    [SerializeField] public AudioClip LaserRightSFX;
    [SerializeField] public AudioClip LaserRightBurnSFX;
    [SerializeField] public AudioClip BulletHumSFX;
    [SerializeField] public AudioClip EnemyVanquishedSFX;
    [SerializeField] public AudioClip PointsLostSFX;
    [SerializeField] public AudioClip PointsGainedSFX;
    [SerializeField] public AudioClip SpearMissSFX;
    [SerializeField] public AudioClip SpearCollideSFX;
    [SerializeField] public AudioClip JetThrustSFX;
    [SerializeField] public AudioClip SixDOFThrustSFX;
    [SerializeField] public AudioClip RadarObjectAddedSFX;
    [SerializeField] public AudioClip BulletCollisionSFX;
    [SerializeField] public AudioClip OtherCollisionSFX;
    [SerializeField] public AudioClip BulletNearWarningSFX;
    [SerializeField] public AudioClip ShieldSFX;
    [SerializeField] public AudioClip BoostSFX;
    [SerializeField] public AudioClip NearLockSFX;

    public void Update()
    {
        if (PlayerController == null)
        {
            PlayerController = FindObjectOfType<SixDOFController>();
        }
        else
        {
            float thrustNormalized = NormalizeValue(2f, 17f, PlayerController.Velocity.magnitude);
            
            float rollNormalized = NormalizeValue(0f, 1f, Mathf.Abs(PlayerController.InputManager.RollInput));
            
            float upNormalized = NormalizeValue(0f, 1f, Mathf.Abs(PlayerController.InputManager.ThrustInput));
            
            float pitchNormalized = NormalizeValue(0f, 1f, Mathf.Abs(PlayerController.InputManager.PitchInput));
            
            float yawNormalized = NormalizeValue(0f, 1f, Mathf.Abs(PlayerController.InputManager.YawInput));
            
            // Get the maximum value of the normalized inputs
            float maxNormalized = Mathf.Max(thrustNormalized, rollNormalized, upNormalized, pitchNormalized, yawNormalized);
            
            if (!SixDOFThrustAudioSource.isPlaying)
            {
                SixDOFThrustAudioSource.Play();
            }
            SixDOFThrustAudioSource.volume = maxNormalized * .2f;
            
            SixDOFThrustAudioSource.pitch = .85f;
        }

        

        if (WeaponController == null)
        {
            WeaponController = FindObjectOfType<WeaponSystem>();
            
            if(WeaponController != null)
            {
                WeaponController.OnRocketFired += PlayRocketLaunchSFX;
                WeaponController.OnFlareFired += PlayFlareSFX;
            }
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        AudioSources = new List<AudioSource>();
        for (int i = 0; i < poolSize; i++)
        {
            AudioSource audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.outputAudioMixerGroup = EffectsGroup;
            AudioSources.Add(audioSource);
        }
        
        // Initialize the AudioSource for SixDOFThrustSFX
        SixDOFThrustAudioSource = gameObject.AddComponent<AudioSource>();
        SixDOFThrustAudioSource.outputAudioMixerGroup = EffectsGroup;
        SixDOFThrustAudioSource.clip = SixDOFThrustSFX;
        SixDOFThrustAudioSource.loop = true; // Make it loop since it's a continuous thrust sound


        Bullet.OnPlayerCollide += PlayBulletCollisionsFX;
        if (GUI == null)
        {
            GUI = FindObjectOfType<PlayerUI>();
            if (GUI != null)
            {
                GUI.OnLockOn += PlayLockOnSFX;
            }
            else
            {
                print("PlayerUI not found for SFX binding.");
            }
        }

        if (RadarGUI == null)
        {
            RadarGUI = FindObjectOfType<Radar>();
            if (RadarGUI != null)
            {
                
            }
            else
            {
                print("Radar not found for SFX binding.");
            };
        }

        if (ShieldBoost == null)
        {
            ShieldBoost = FindObjectOfType<ShieldBoost>();
            if (ShieldBoost != null)
            {
                ShieldBoost.OnBoost += PlayBoostSFX;
                ShieldBoost.OnShield += PlayShieldSFX;
                ShieldBoost.OnBoostEnd += StopBoostSFX;
                ShieldBoost.OnShieldEnd += StopShieldSFX;
            }
            else
            {
                print("ShieldBoost not found for SFX binding.");
            }
        }

        if (Lasers == null)
        {
            Lasers = FindObjectOfType<LaserBeam>();
            if (Lasers != null)
            {
                Lasers.OnActiveLeft += PlayLeftLaserSFX;
                Lasers.OnActiveLeftEnd += StopLeftLaserSFX;
                Lasers.OnBurnLeft += PlayLeftLaserBurnSFX;
                Lasers.OnBurnLeftEnd += StopLeftLaserBurnSFX;
                
                
                Lasers.OnActiveRight += PlayRightLaserSFX;
                Lasers.OnActiveRightEnd += StopRightLaserSFX;
                Lasers.OnBurnRight += PlayRightLaserBurnSFX;
                Lasers.OnBurnRightEnd += StopRightLaserBurnSFX;
            }
            else
            {
                print("LaserBeam not found for SFX binding.");
            }
        }

    }

    private AudioSource GetAvailableAudioSource()
    {
        foreach (var audioSource in AudioSources)
        {
            if (!audioSource.isPlaying)
            {
                return audioSource;
            }
        }
        
        AudioSource newAudioSource = gameObject.AddComponent<AudioSource>();
        AudioSources.Add(newAudioSource);
        return newAudioSource;
    }

    private void PlaySound(AudioClip clip, float volume, float delay)
    {
        // Check if the clip is already playing
        if (IsClipPlaying(clip))
        {
            return;
        }

        AudioSource audioSource = GetAvailableAudioSource();
        audioSource.volume = volume;
        audioSource.clip = clip;
        audioSource.PlayDelayed(delay);
    }

    private void StopSound(AudioClip clip)
    {
        foreach (var audioSource in AudioSources)
        {
            if (audioSource.isPlaying && audioSource.clip == clip)
            {
                audioSource.Stop();
            }
        }
    }

    // Checks if the specified audio clip is already playing
         private bool IsClipPlaying(AudioClip clip)
         {
             foreach (var audioSource in AudioSources)
             {
                 if (audioSource.isPlaying && audioSource.clip == clip)
                 {
                     return true;
                 }
             }
             return false;
         }
    
    

    public void PlaySixDOFThrustSFX()
    {
        float volume = 1.0f;
        float delay = 0f;
        
        PlaySound(SixDOFThrustSFX, volume, delay);
    }
    
    // Functions for each SFX
    public void PlayRocketLaunchSFX()
    {
        float volume = .4f;
        float delay = 0f;
        
        PlaySound(RocketLaunchSFX, volume, delay);
    }

    public void PlayRocketExplodeSFX()
    {
        float volume = 1.0f;
        float delay = 0f;
        
        PlaySound(RocketExplodeSFX, volume, delay);
    }

    public void PlayFlareSFX()
    {
        float volume = .4f;
        float delay = 0f;
        
        PlaySound(FlareSFX, volume, delay);
    }

    public void PlayFlareCollideSFX()
    {
        float volume = 1.0f;
        float delay = 0f;
        
        PlaySound(FlareCollideSFX, volume, delay);
    }

    public void PlayLockOnSFX()
    {
        float volume = .8f;
        float delay = 0f;
        
        PlaySound(LockOnSFX, volume, delay);
    }

    public void PlayLockOffSFX()
    {
        float volume = 1.0f;
        float delay = 0f;
        
        PlaySound(LockOffSFX, volume, delay);
    }

    public void PlayEnemySpawnSFX()
    {
        float volume = 1.0f;
        float delay = 0f;
        
        PlaySound(EnemySpawnSFX, volume, delay);
    }

    public void PlayLeftLaserSFX()
    {
        float volume = .2f;
        float delay = 0f;
        
        PlaySound(LaserLeftSFX, volume, delay);
    }

    public void PlayLeftLaserBurnSFX()
    {
        float volume = .5f;
        float delay = 0f;
        
        PlaySound(LaserLeftBurnSFX, volume, delay);
    }
    
    public void PlayRightLaserSFX()
    {
        float volume = .2f;
        float delay = 0f;
        
        PlaySound(LaserRightSFX, volume, delay);
    }

    public void PlayRightLaserBurnSFX()
    {
        float volume = .5f;
        float delay = 0f;
        
        PlaySound(LaserRightBurnSFX, volume, delay);
    }
    
    
    public void StopLeftLaserSFX()
    {
        
        StopSound(LaserLeftSFX);
    }

    public void StopLeftLaserBurnSFX()
    {
        
        StopSound(LaserLeftBurnSFX);
    }
    
    public void StopRightLaserSFX()
    {
        
        StopSound(LaserRightSFX);
    }

    public void StopRightLaserBurnSFX()
    {
        
        StopSound(LaserRightBurnSFX);
    }

    public void PlayBulletHumSFX()
    {
        float volume = 1.0f;
        float delay = 0f;
        
        PlaySound(BulletHumSFX, volume, delay);
    }

    public void PlayEnemyVanquishedSFX()
    {
        float volume = 1.0f;
        float delay = 0f;
        
        PlaySound(EnemyVanquishedSFX, volume, delay);
    }

    public void PlayPointsLostSFX()
    {
        float volume = 1.0f;
        float delay = 0f;
        
        PlaySound(PointsLostSFX, volume, delay);
    }

    public void PlayPointsGainedSFX()
    {
        float volume = 1.0f;
        float delay = 0f;
        
        PlaySound(PointsGainedSFX, volume, delay);
    }

    public void PlaySpearMissSFX()
    {
        float volume = 1.0f;
        float delay = 0f;
        
        PlaySound(SpearMissSFX, volume, delay);
    }

    public void PlaySpearCollideSFX()
    {
        float volume = 1.0f;
        float delay = 0f;
        
        PlaySound(SpearCollideSFX, volume, delay);
    }

    public void PlayJetThrustSFX()
    {
        float volume = 1.0f;
        float delay = 0f;
        
        PlaySound(JetThrustSFX, volume, delay);
    }
    
    public void PlayRadarObjectAddedSFX()
    {
        float volume = 1.0f;
        float delay = 0f;
        
        PlaySound(RadarObjectAddedSFX, volume, delay);
    }
    
    public void PlayBulletCollisionsFX(Vector3 impulse)
    {
        float volume = NormalizeValue(0f, 15f, impulse.magnitude) * .8f;
        float delay = 0f;
        
        PlaySound(BulletCollisionSFX, volume, delay);
    }
    
    public void PlayOtherCollisionSFX()
    {
        float volume = 1.0f;
        float delay = 0f;
        
        PlaySound(OtherCollisionSFX, volume, delay);
    }
    
    public void PlayBulletNearWarningSFX()
    {
        float volume = 1.0f;
        float delay = 0f;
        
        PlaySound(BulletNearWarningSFX, volume, delay);
    }
    
    public void PlayShieldSFX()
    {
        float volume = 1.0f;
        float delay = 0f;
        
        PlaySound(ShieldSFX, volume, delay);
    }
    
    public void PlayBoostSFX()
    {
        float volume = 1.0f;
        float delay = 0f;
        
        PlaySound(BoostSFX, volume, delay);
    }
    public void StopShieldSFX()
    {
        
        StopSound(ShieldSFX);
    }
    public void StopBoostSFX()
    {
        
        StopSound(BoostSFX);
    }
    
    public void PlayNearLockSFX()
    {
        float volume = 1.0f;
        float delay = 0f;
        
        PlaySound(NearLockSFX, volume, delay);
    }
    
    // Converts a value from the range [2, 17] to the range [0, 1]
    public float NormalizeValue(float min, float max, float value)
    {

        // Clamp the value to ensure it's within the expected range
        value = Mathf.Clamp(value, min, max);

        // Normalize the value to the range [0, 1]
        return (value - min) / (max - min);
    }
}