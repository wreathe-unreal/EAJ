using UnityEngine;

namespace EAJ
{
    [RequireComponent(typeof(Rigidbody))]

    public class SixDOFController : MonoBehaviour
    {
        [Header("Project References:")]
        [SerializeField] private SixDOFMovementData _sixDofMovementData = default;
        [Header("Local References:")]
        [SerializeField] private Transform _sixDofObject = default;

        [SerializeField] public float PitchSensitivity = .1f;
        [SerializeField] public float YawSensitivity = .1f;
        
        [SerializeField] public bool InvertPitch = true;
        [SerializeField] public bool InvertRoll = true;
        [SerializeField] public bool InvertYaw = false;

        // Component renferences.
        private Rigidbody _rigidbody = default;
        private InputManager _inputManager = default;

        // Calculation values.
        private Vector3 _smoothDampToStopVelocity = default;
        
        private float _currentStrafeAmount = default;
        private float _currentStrafeAmountVelocity = default;
        
        private float _currentSurgeAmount = default;
        private float _currentSurgeAmountVelocity = default;
        
        private float _currentYawRotation = default;
        private float _targetYawRotationVelocity = default;
        
        private float _currentPitchRotation = default;
        private float _targetPitchRotationVelocity = default;
        
        private float _currentRollRotation = default;
        private float _targetRollRotationVelocity = default;
        
        private float _currentThrustForce = default;

        // Public properties.
        public float CurrentYRotation { get { return _currentYawRotation; } }
        public Vector3 Velocity { get { return _rigidbody.velocity; } }

        public InputManager InputManager;

        protected virtual void Awake()
        {
            InputManager = GetComponent<InputManager>();
            _rigidbody = GetComponent<Rigidbody>();
        }

        protected virtual void Start()
        {
            SetStartingRotation();
        }

        protected virtual void FixedUpdate()
        {
            ClampingSpeedValues();

            ThrustForce(InputManager.ThrustInput);
            StrafeForce(InputManager.StrafeInput);
            YawForce(InputManager.YawInput);
            SurgeForce(InputManager.SurgeInput);
            PitchForce(InputManager.PitchInput);
            RollForce(InputManager.RollInput);

            ApplyForces();
        }

        /// <summary>
        /// Fixes the starting rotation, sets the wanted and current rotation in the
        /// code so drone doesnt start with rotation of (0,0,0).
        /// </summary>
        private void SetStartingRotation()
        {
            _currentYawRotation = transform.eulerAngles.y;
        }

        /// <summary>
        /// Applying upForce for hovering and keeping the drone in the air.
        /// Handles rotation and applies it here.
        /// Handles tilt values and applies it, gues where? here! :)
        /// </summary>
        public void ApplyForces()
        {
            _rigidbody.AddRelativeForce(Vector3.up * _currentThrustForce);
            
            // Create quaternions for pitch, yaw, and roll rotations
            Quaternion pitchRotation = Quaternion.AngleAxis(_currentPitchRotation, transform.right);
            Quaternion yawRotation = Quaternion.AngleAxis(_currentYawRotation, transform.up);
            Quaternion rollRotation = Quaternion.AngleAxis(_currentRollRotation, transform.forward);

            // Combine the rotations
            Quaternion newRotation = _rigidbody.rotation * yawRotation * pitchRotation * rollRotation;

            // Apply the new rotation using MoveRotation to handle the physics properly
            _rigidbody.MoveRotation(newRotation);
            _rigidbody.angularVelocity = new Vector3(0, 0, 0);
            //_droneObject.localRotation = Quaternion.Euler(new Vector3(_currentSurgeAmount, 0, -_currentStrafeAmount));
        }

        /// <summary>
        /// Clamping speed values determined on what input is pressed
        /// </summary>
        public void ClampingSpeedValues()
        {
            _rigidbody.velocity = Vector3.ClampMagnitude(_rigidbody.velocity, Mathf.Lerp(_rigidbody.velocity.magnitude, _sixDofMovementData.MaximumPitchSpeed, Time.deltaTime * 5f));
            // if (InputManager.IsInputIdle())
            // {
            //     _rigidbody.velocity = Vector3.SmoothDamp(_rigidbody.velocity, Vector3.zero, ref _smoothDampToStopVelocity, _droneMovementData.SlowDownTime);
            // }
        }

        /// <summary>
        /// Handling up down movement and applying needed force.
        /// </summary>
        public void ThrustForce(float thrustInput)
        {
            float forceValue = (thrustInput > 0) ? _sixDofMovementData.ThrustUpForce : (thrustInput < 0) ? _sixDofMovementData.ThrustDownForce : 0f;
            _currentThrustForce = thrustInput * forceValue;
        }

        /// <summary>
        /// Handling left right movement and appying forces, also handling the titls
        /// </summary>
        public void StrafeForce(float strafeInput)
        {
            _rigidbody.AddRelativeForce(Vector3.right * strafeInput * _sixDofMovementData.StrafeForce);
            _currentStrafeAmount = Mathf.SmoothDamp(_currentStrafeAmount, _sixDofMovementData.MaximumStrafeAmount * strafeInput, ref _currentStrafeAmountVelocity, _sixDofMovementData.SurgeStrafeTiltSpeed);
        }

        /// <summary>
        /// Handling rotations
        /// </summary>
        public void YawForce(float yawInput)
        {
            int _InvertYaw = InvertYaw ? -1 : 1;
            _rigidbody.rotation = Quaternion.AngleAxis(_InvertYaw * yawInput * _sixDofMovementData.MaximumYawSpeed, transform.up) * _rigidbody.rotation;
            //_currentYawRotation = Mathf.SmoothDamp(_currentYawRotation, _targetYawRotation, ref _targetYawRotationVelocity, 0.25f);
        }

        /// <summary>
        /// Movement forwards and backwars and tilting
        /// </summary>
        public void SurgeForce(float surgeInput)
        {
            _rigidbody.AddRelativeForce(Vector3.forward * surgeInput * _sixDofMovementData.SurgeForce);
            _currentSurgeAmount = Mathf.SmoothDamp(_currentSurgeAmount, _sixDofMovementData.MaximumSurgeAmount * surgeInput, ref _currentSurgeAmountVelocity, _sixDofMovementData.SurgeStrafeTiltSpeed);
        }
        
        public void PitchForce(float pitchInput)
        {
            int _InvertPitch = InvertPitch ? -1 : 1;

            _rigidbody.rotation =
                Quaternion.AngleAxis(_InvertPitch * pitchInput * _sixDofMovementData.MaximumPitchSpeed, transform.right) * _rigidbody.rotation;
            //_currentPitchRotation = Mathf.SmoothDamp(_currentPitchRotation, _targetPitchRotation, ref _targetPitchRotationVelocity, 0.25f);
        }
        
        public void RollForce(float rollInput)
        {
            int _InvertRoll = InvertRoll ? -1 : 1;
            _rigidbody.rotation = Quaternion.AngleAxis(_InvertRoll * rollInput * _sixDofMovementData.MaximumRollSpeed, transform.forward) * _rigidbody.rotation;
            //_currentRollRotation = Mathf.SmoothDamp(_currentRollRotation, _targetRollRotation, ref _targetRollRotationVelocity, 0.25f);
        }
    }
}
