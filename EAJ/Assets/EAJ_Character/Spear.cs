using System;
using System.Collections;
using System.Collections.Generic;
using EAJ;
using UnityEngine;

public class Spear : MonoBehaviour
{
    private WeaponSystem WeaponInputs;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (WeaponInputs == null)
        {
            WeaponInputs = FindObjectOfType<WeaponSystem>();
            if (WeaponInputs != null)
            {
            }
        }
        
    }

    private void OnTriggerStay(Collider other)
    {
        if (WeaponInputs == null || !WeaponInputs.SpearPressed)
        {
            return;
        }

        Debug.Log("Collision detected with: " + other.gameObject.name);
        if (other.gameObject.CompareTag("Enemy"))
        {
            Debug.Log("Collided with enemy: " + other.gameObject.name);
            Enemy enemy = other.gameObject.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.ModifyHealth(-enemy.HEALTH_MAX, -enemy.HEALTH_MAX);
                Debug.Log("Enemy health after damage: " + enemy.Health);
            }
            else
            {
                Debug.LogWarning("No Enemy component found on collided object");
            }
        }
    }
}
