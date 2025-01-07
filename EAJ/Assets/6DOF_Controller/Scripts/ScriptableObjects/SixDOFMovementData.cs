using UnityEngine;

namespace EAJ
{
    [CreateAssetMenu(fileName = nameof(SixDOFMovementData), menuName = nameof(EAJ) + "/" + nameof(SixDOFMovementData))]
    public class SixDOFMovementData : ScriptableObject
    {
        public float MaximumPitchSpeed = 1f;
        public float MaximumRollSpeed = 1f;
        public float MaximumYawSpeed = 1f;
        [Space]
        public float IdleUpForce = 0f;
        public float SurgeForce = 750f;
        public float StrafeForce = 450f;
        public float ThrustUpForce = 450f;
        public float ThrustDownForce = 250f;
        [Space]
        [Range(0, 90)] public float MaximumSurgeAmount = 30f;
        [Range(0, 90)] public float MaximumStrafeAmount = 30f;
        [Space]
        [Range(0.0f, 1.0f)] public float SurgeStrafeTiltSpeed = 0.1f;
        [Space]
        [Range(0.0f, 2.0f)] public float SlowDownTime = 0.95f;
    }
}
