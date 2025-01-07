using System;
using System.Collections;
using System.Collections.Generic;
using EAJ;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    public Enemy EnemyLockedOn;
    public TextMeshProUGUI Score;
    public int PlayerScore = 0;
    public RectTransform LeftLaserReticle;
    public RectTransform RightLaserReticle;
    public RectTransform LockOnReticle;
    public WeaponSystem Weapons;
    public Slider EnemyHealthBar;
    public SixDOFController PlayerRef;
    public TextMeshProUGUI PlusScore;
    public TextMeshProUGUI MinusScore;
    public float LockOnDistance = 50f;
    
    private LaserBeam Lasers;
    private Enemy LastLockOn;
    private static PlayerUI Instance;
    

    public event Action OnLockOn;
    
    // Start is called before the first frame update
    void Start()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        
        Lasers = FindObjectOfType<LaserBeam>();
        PlayerRef = FindObjectOfType<SixDOFController>();
        Weapons = FindObjectOfType<WeaponSystem>();
        StartCoroutine(DecreaseScoreOverTime());
        LockOnReticle.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
        Score.text = PlayerScore.ToString();

        if (EnemyLockedOn == null)
        {
            return;
        }
        
        EnemyHealthBar.value = EnemyLockedOn.GetNormalizedHealth();


        //PlaceHealthBar();
    }

    public static PlayerUI GetInstance()
    {
        return Instance;
    }
    
    // void PlaceHealthBar()
    // {
    //     RectTransform healthBarReticleRectTransform = LockOnReticle.GetComponentInChildren<RectTransform>();
    //
    //     Vector3 targetWorldPosition = EnemyLockedOn.transform.position;
    //     Vector3 screenPosition = Camera.main.WorldToScreenPoint(targetWorldPosition);
    //
    //     RectTransform canvasRectTransform = healthBarReticleRectTransform.parent.GetComponentInParent<Canvas>().GetComponent<RectTransform>();
    //     Vector2 canvasPosition;
    //     RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRectTransform, screenPosition, Camera.main, out canvasPosition);
    //
    //     healthBarReticleRectTransform.anchoredPosition = canvasPosition;
    //     healthBarReticleRectTransform.rotation =
    //         Quaternion.AngleAxis(PlayerRef.transform.rotation.z + 90f, PlayerRef.transform.forward);
    // }

    void LateUpdate()
    {
        TryLockOn();
    }
    
    private void TryLockOn()
    {
        if (Weapons == null)
        {
            Weapons = FindObjectOfType<WeaponSystem>();
            return;
        }
        
        
        Enemy lockOnTarget = Enemy.FindLockOn();

        if (lockOnTarget != null && lockOnTarget != LastLockOn)
        {
            OnLockOn?.Invoke();
        }
        
        LastLockOn = lockOnTarget;
        
        if (lockOnTarget != null)
        {
            EnemyLockedOn = lockOnTarget;
            LockOnReticle.gameObject.SetActive(true);
            EnemyHealthBar.gameObject.SetActive(true);
            Weapons.bLockedOn = true;
            
            
            LockOnReticle.localScale = new Vector3(3f, 3f, 3f) * Mathf.Max(1f, EnemyLockedOn.Height/7f);
            LockOnReticle.transform.SetParent(lockOnTarget.transform.parent);
            LockOnReticle.transform.localPosition = Vector3.zero;


            Vector3 directionToPlayer = lockOnTarget.transform.position - PlayerRef.transform.position;
            LockOnReticle.transform.localRotation = Quaternion.Inverse(LockOnReticle.transform.parent.rotation);
            LockOnReticle.transform.rotation *= Quaternion.LookRotation(directionToPlayer.normalized, Camera.main.transform.up);
            
        }
        else
        {
            LockOnReticle.gameObject.transform.SetParent(this.transform);
            LockOnReticle.gameObject.SetActive(false);
            EnemyHealthBar.gameObject.SetActive(false);
            EnemyLockedOn = null;
            Weapons.bLockedOn = false;
        }
    }
    private IEnumerator DecreaseScoreOverTime()
    {
        while (true)
        {
            yield return new WaitForSeconds(3f);
            ModifyScore(-1);
        }
    }

    public void ModifyScore(int amount)
    {
        StartCoroutine(ShowModifiedScore(amount));
        PlayerScore += amount;
    }

    public IEnumerator ShowModifiedScore(float amount)
    {
        
        if (amount > 0)
        {
            if (PlusScore.gameObject.activeSelf)
            {
                PlusScore.text = Mathf.RoundToInt(amount).ToString() + " " + PlusScore.text;
            }
            else
            {
                MinusScore.gameObject.SetActive(false);
                PlusScore.text = "+" + Mathf.RoundToInt(amount).ToString();
                PlusScore.gameObject.SetActive(true);
            }
            yield return new WaitForSeconds(.5f);
            PlusScore.gameObject.SetActive(false);
        }
        else
        {
            if (MinusScore.gameObject.activeSelf)
            {
                MinusScore.text = Mathf.RoundToInt(amount).ToString() + " " + MinusScore.text;
            }
            else
            {
                PlusScore.gameObject.SetActive(false);
                MinusScore.text = Mathf.RoundToInt(amount).ToString();
                MinusScore.gameObject.SetActive(true);
            }

            yield return new WaitForSeconds(.5f);
            MinusScore.gameObject.SetActive(false);
        }
    }
}
