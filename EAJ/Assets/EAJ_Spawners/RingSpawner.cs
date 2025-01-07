using UnityEngine;
using System.Collections;

public class RingSpawner : MonoBehaviour
{
    public Material RingMaterial;
    public float RingPingSpeed = 4.0f;
    public float LineWidth = 0.33f;
    public int NumVerts = 80;
    public float StartRadius = 1f;
    public float EndRadius = 80f;
    public Enemy Source;
    
    private LineRenderer LR;
    private MeshCollider MeshCollider;
    

    private void Start()
    {
        LR = gameObject.AddComponent<LineRenderer>();

        LR.material = RingMaterial;
        LR.positionCount = NumVerts + 1;
        LR.useWorldSpace = true;
        LR.startWidth = LineWidth;
        LR.endWidth = LineWidth;

        MeshCollider = gameObject.AddComponent<MeshCollider>();

        StartCoroutine(SpawnRing(LR));
    }

    private IEnumerator SpawnRing(LineRenderer lr)
    {
        for (float radius = StartRadius; radius < EndRadius; radius += RingPingSpeed * Time.deltaTime)
        {
            for (int vertno = 0; vertno <= NumVerts; vertno++)
            {
                float angle = (vertno * Mathf.PI * 2) / NumVerts;
                Vector3 pos = transform.position + new Vector3(Mathf.Cos(angle) * radius, Mathf.Sin(angle) * radius, 0);
                lr.SetPosition(vertno, pos);
            }
            UpdateMeshCollider();
            yield return null;
        }
    }

    private void UpdateMeshCollider()
    {
        Camera playerCamera = FindNearestCamera();
        if (playerCamera == null)
        {
            return;
        }
        Mesh mesh = new Mesh();
        LR.BakeMesh(mesh, FindNearestCamera(), true); //only works with identity transform
        MeshCollider.sharedMesh = mesh;
    }

    private Camera FindNearestCamera()
    {
        Camera[] SceneCameras = FindObjectsOfType<Camera>();

        float nearestCameraDistance = float.MaxValue;
        Camera nearestCameraObject = null;
        
        foreach (Camera c in SceneCameras)
        {

            float cameraDistance = Vector3.Distance(this.transform.position, c.transform.position);
            if (cameraDistance < nearestCameraDistance)
            {
                nearestCameraDistance = cameraDistance;
                nearestCameraObject = c;
            }
        }

        return nearestCameraObject;
    }
    
}