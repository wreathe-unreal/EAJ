using UnityEngine;

namespace EAJ
{
    [CreateAssetMenu(fileName = nameof(CameraMovementData), menuName = nameof(EAJ) + "/" + nameof(CameraMovementData))]
    public class CameraMovementData : ScriptableObject
    {
        public Vector3 Offset = new Vector3(0, 2, -4);
        [Range(0.0f, 0.1f)] public float FollowSpeed = 0.1f;
        [Range(0.0f, 80f)] public float YFollowStrength = 10;
    }
}
