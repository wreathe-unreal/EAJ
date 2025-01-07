using System.Collections;
using System.Collections.Generic;
using EAJ;
using UnityEngine;

public class Beam : MonoBehaviour
{
    public int Damage;
    private Quaternion Rotation;
    private LineRenderer lineRenderer;

    [Tooltip("Time interval between line renderer spawns in seconds")]
    public float spawnInterval = 5f;
    
    [Tooltip("Length of the line renderer")]
    public float lineLength = 10f;
    
    [Tooltip("Speed of the rotation in degrees per second")]
    public float rotationSpeed = 90f;

    private float timeSinceLastSpawn = 0f;

    // Start is called before the first frame update
    void Start()
    {

        // Initialize LineRenderer
        lineRenderer = gameObject.AddComponent<LineRenderer>();
        lineRenderer.positionCount = 2;
        lineRenderer.startWidth = 0.1f;
        lineRenderer.endWidth = 0.1f;
        lineRenderer.material = new Material(Shader.Find("Sprites/Default")); // Basic material
        lineRenderer.startColor = Color.red;
        lineRenderer.endColor = Color.red;
    }

    // Update is called once per frame
    void Update()
    {

            if (EAJ_Manager.GetInstance().PlayerRef == null)
            {
                return;
            }
            else
            {
                // Initialize LineRenderer
                lineRenderer = gameObject.AddComponent<LineRenderer>();
                lineRenderer.positionCount = 2;
                lineRenderer.startWidth = 0.1f;
                lineRenderer.endWidth = 0.1f;
                lineRenderer.material = new Material(Shader.Find("Sprites/Default")); // Basic material
                lineRenderer.startColor = Color.red;
                lineRenderer.endColor = Color.red;
            }

        timeSinceLastSpawn += Time.deltaTime;

        if (timeSinceLastSpawn >= spawnInterval)
        {
            timeSinceLastSpawn = 0f;
            SpawnLineRenderer();
        }

        RotateTowardsPlayer();
        CheckCollisionWithRaycast();
    }

    private void SpawnLineRenderer()
    {
        Vector3 direction = (EAJ_Manager.GetInstance().PlayerRef.transform.position - transform.position).normalized;
        lineRenderer.SetPosition(0, transform.position);
        lineRenderer.SetPosition(1, transform.position + direction * lineLength);
    }

    private void RotateTowardsPlayer()
    {
        // Calculate the direction to the player
        Vector3 direction = (EAJ_Manager.GetInstance().PlayerRef.transform.position - transform.position).normalized;
        
        // Calculate the target rotation
        Quaternion targetRotation = Quaternion.LookRotation(direction);
        
        // Rotate towards the target rotation
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }

    private void CheckCollisionWithRaycast()
    {
        Vector3 startPoint = lineRenderer.GetPosition(0);
        Vector3 endPoint = lineRenderer.GetPosition(1);
        Vector3 direction = (endPoint - startPoint).normalized;

        RaycastHit hit;
        if (Physics.Raycast(startPoint, direction, out hit, lineLength))
        {
            Debug.Log("Hit object: " + hit.collider.gameObject.name);
            // Handle collision
            // For example, apply damage if the object has a health component, etc.
        }
    }
}