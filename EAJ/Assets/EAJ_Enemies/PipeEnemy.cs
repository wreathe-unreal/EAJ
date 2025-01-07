using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PipeEnemy : MonoBehaviour
{
    public float speed = 5f;
    public float colliderInterval = 1f;
    public LayerMask environmentLayer = LayerMask.NameToLayer("Default");
    public GameObject pipeColliderPrefab;

    private Vector3 direction;
    private float distanceTraveled = 0f;

    void Start()
    {
        direction = GetRandomDirection();
        InvokeRepeating(nameof(CreateCollider), 0, colliderInterval);
    }

    void Update()
    {
        MoveEnemy();
    }

    void MoveEnemy()
    {
        Vector3 newPosition = transform.position + direction * speed * Time.deltaTime;
        RaycastHit hit;

        // Check if the enemy hits the environment or bounds
        if (Physics.Raycast(transform.position, direction, out hit, speed * Time.deltaTime, environmentLayer))
        {
            direction = Vector3.Reflect(direction, hit.normal);
            newPosition = transform.position + direction * speed * Time.deltaTime;
        }

        transform.position = newPosition;
        distanceTraveled += speed * Time.deltaTime;

        if (distanceTraveled >= colliderInterval)
        {
            distanceTraveled = 0f;
            CreateCollider();
        }
    }

    Vector3 GetRandomDirection()
    {
        return Random.onUnitSphere;
    }

    void CreateCollider()
    {
        Instantiate(pipeColliderPrefab, transform.position, Quaternion.identity);
    }
}
