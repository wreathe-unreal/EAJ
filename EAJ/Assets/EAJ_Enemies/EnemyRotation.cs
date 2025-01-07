using System.Collections;
using System.Collections.Generic;
using EAJ;
using UnityEngine;

public class EnemyRotation : MonoBehaviour
{
    private SixDOFController PlayerRef;
    public bool bShouldPredict = false;
    private float Speed = 0f;

    // Start is called before the first frame update
    void Start()
    {
        PlayerRef = FindObjectOfType<SixDOFController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (PlayerRef == null)
        {
            return;
        }

        if (bShouldPredict)
        {
            if (Speed == 0f)
            {
                if (GetComponent<BulletPool>() != null)
                {
                    Speed = GetComponent<BulletPool>().bulletPrefab.GetComponent<Bullet>().Speed;
                }
            }

            Rigidbody PlayerRB = PlayerRef.GetComponent<Rigidbody>();
            if (PlayerRB != null)
            {
                // Calculate the distance to the player
                float distanceToPlayer = Vector3.Distance(transform.position, PlayerRef.transform.position);

                // Calculate the time to reach the player (assuming constant speed)
                float timeToReachPlayer = distanceToPlayer / Speed;

                // Predict the future position of the player
                Vector3 futurePlayerPosition = PlayerRef.transform.position + PlayerRB.velocity * timeToReachPlayer;

                // Calculate the direction to the player's future position
                Vector3 directionToPlayerInFuture = futurePlayerPosition - transform.position;

                if (directionToPlayerInFuture.sqrMagnitude > 0.0001f)
                {
                    // Normalize the direction vector
                    Vector3 normalizedDirection = directionToPlayerInFuture.normalized;
                    // Check again to avoid normalizing a zero vector
                    if (normalizedDirection.sqrMagnitude > 0.0001f)
                    {
                        transform.rotation = Quaternion.LookRotation(normalizedDirection, PlayerRef.transform.up);
                    }
                }
            }
        }
        else
        {
            
            Vector3 directionToPlayer = (PlayerRef.transform.position - transform.position);
            
            // Ensure the direction is not zero
            if (!Mathf.Approximately(directionToPlayer.sqrMagnitude, 0f))
            {
                transform.rotation = Quaternion.LookRotation(directionToPlayer.normalized, transform.up);
            }
        }
    }
}