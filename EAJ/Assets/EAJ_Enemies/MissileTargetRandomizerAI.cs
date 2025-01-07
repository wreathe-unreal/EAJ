using System.Collections;
using System.Collections.Generic;
using AVM;
using EAJ;
using UnityEngine;

public class MissileTargetRandomizerAI : MonoBehaviour
{
    private AdvancedMissile MissileComponent;
    private bool bIsRunning = true;
    private string CurrentDirectionTag;

    private static readonly string[] RandomDirectionTags = { "North", "East", "South", "West" };

    private Coroutine RandomizeTagsCoro;

    private void Start()
    {
        MissileComponent = GetComponent<AdvancedMissile>();
        if (MissileComponent == null)
        {
            Debug.LogError("AdvancedMissile component not found on the GameObject.");
            return;
        }
        MissileComponent.CanResearch = true;
        bIsRunning = true;
    }

    private void OnEnable()
    {
        if (MissileComponent != null)
        {
            RandomizeTagsCoro = StartCoroutine(ManageTargetTags());
        }
    }

    private void OnDisable()
    {
        if (RandomizeTagsCoro != null)
        {
            StopCoroutine(RandomizeTagsCoro);
        }
    }

    private IEnumerator ManageTargetTags()
    {
        while (bIsRunning)
        {
            float distanceToPlayer = Vector3.Distance(transform.position, EAJ_Manager.GetInstance().PlayerRef.transform.position);
            if (distanceToPlayer <= 40f)
            {
                // If within 40 units of the player, remove random directions and re-add "Ghost"
                ClearRandomDirectionTags();
                if (!MissileComponent.TargetTags.Contains("Ghost"))
                {
                    MissileComponent.TargetTags.Add("Ghost");
                }
                
                // Wait for 5 seconds before reevaluating
                yield return new WaitForSeconds(5);
                continue; // Skip the rest of the loop and reevaluate
            }

            // Wait for 1-5 seconds before removing "Ghost"
            yield return new WaitForSeconds(Random.Range(1, 5));
            if (MissileComponent.TargetTags.Contains("Ghost"))
            {
                MissileComponent.TargetTags.Remove("Ghost");

                // Assign a random direction tag
                CurrentDirectionTag = RandomDirectionTags[Random.Range(0, RandomDirectionTags.Length)];
                if (!MissileComponent.TargetTags.Contains(CurrentDirectionTag))
                {
                    MissileComponent.TargetTags.Add(CurrentDirectionTag);
                }
            }

            // Wait for 5-15 seconds before adding "Ghost" back
            yield return new WaitForSeconds(Random.Range(5, 15));
            if (!MissileComponent.TargetTags.Contains("Ghost"))
            {
                MissileComponent.TargetTags.Add("Ghost");

                // Remove the current direction tag
                if (MissileComponent.TargetTags.Contains(CurrentDirectionTag))
                {
                    MissileComponent.TargetTags.Remove(CurrentDirectionTag);
                }
            }
        }
    }

    private void ClearRandomDirectionTags()
    {
        foreach (var tag in RandomDirectionTags)
        {
            if (MissileComponent.TargetTags.Contains(tag))
            {
                MissileComponent.TargetTags.Remove(tag);
            }
        }
    }
}
