using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ghost : MonoBehaviour
{
    public Transform Player; // Assign the player object in the inspector
    private Queue<MovementData> MovementQueue = new Queue<MovementData>();
    public float RecordInterval = 1.0f; // Duration to keep the history (in seconds)

    private void Start()
    {
        // Start the recording coroutine
        StartCoroutine(RecordMovement());
    }

    private IEnumerator<WaitForSeconds> RecordMovement()
    {
        while (true)
        {
            // Record the current position and rotation
            MovementQueue.Enqueue(new MovementData(Player.position, Player.rotation));
            
            // If the queue exceeds the record duration, dequeue the oldest entry
            if (MovementQueue.Count > Mathf.Round(RecordInterval / Time.fixedDeltaTime))
            {
                MovementQueue.Dequeue();
            }

            // Wait for the next fixed update
            yield return new WaitForSeconds(Time.fixedDeltaTime);
        }
    }

    private void Update()
    {
        // Check if the queue contains enough data
        if (MovementQueue.Count > 0)
        {
            // Apply the position and rotation from 1 second ago
            MovementData dataFromOneSecondAgo = MovementQueue.Peek();
            transform.position = dataFromOneSecondAgo.Position;
            transform.rotation = dataFromOneSecondAgo.Rotation;
        }
    }

    private struct MovementData
    {
        public Vector3 Position;
        public Quaternion Rotation;

        public MovementData(Vector3 position, Quaternion rotation)
        {
            Position = position;
            Rotation = rotation * Quaternion.Euler(-90f, 0f, 0f);
        }
    }
}
