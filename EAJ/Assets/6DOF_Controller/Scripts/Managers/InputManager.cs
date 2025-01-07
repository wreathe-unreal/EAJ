using UnityEngine;
using UnityEngine.InputSystem;

namespace EAJ
{
    public class InputManager : MonoBehaviour
    {

        // [SerializeField] private InputActionAsset _inputActionAsset = default;
        // [SerializeField] private InputActionReference _inputSurge = default;
        // [SerializeField] private InputActionReference _inputStrafe = default;
        // [SerializeField] private InputActionReference _inputYaw = default;
        // [SerializeField] private InputActionReference _inputThrust = default;
        // [SerializeField] private InputActionReference _inputRoll = default;
        // [SerializeField] private InputActionReference _inputPitch = default;

        [SerializeField] private bool _boostInput = default;
        [SerializeField] private bool _shieldInput = default; // Changed to bool
        [SerializeField] private float _surgeInput = default;
        [SerializeField] private float _strafeInput = default;
        [SerializeField] private float _yawInput = default;
        [SerializeField] private float _thrustInput = default;
        [SerializeField] private float _rollInput = default;
        [SerializeField] private float _pitchInput = default;
        [SerializeField] private bool _menuReadyInput = default;

        public bool MenuReadyInput
        {
            get { return _menuReadyInput; }
        }
        
        public bool BoostInput { get { return _boostInput; }}
        public bool ShieldInput { get { return _shieldInput; }}
        public float SurgeInput { get { return _surgeInput; } }
        public float StrafeInput { get { return _strafeInput; } }
        public float YawInput { get { return _yawInput; } }
        public float ThrustInput { get { return _thrustInput; } }
        public float PitchInput { get { return _pitchInput; } }
        public float RollInput { get { return _rollInput; } }
        
        private PlayerInput PlayerInput;

        private void Awake()
        {
            PlayerInput = GetComponent<PlayerInput>();
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }


        public void OnMenuReady(InputValue value)
        {
            SetInputValue(ref _menuReadyInput, value.isPressed);
        }
        
        public void OnBoost(InputValue value)
        {
            SetInputValue(ref _boostInput, value.isPressed);
        }
        
        public void OnPlayerOneShield(InputValue value)
        {
            SetInputValue(ref _shieldInput, value.isPressed);
        }
        
        public void OnYaw(InputValue value)
        {
            SetInputValue(ref _yawInput, value.Get<float>());
        }
        
        public void OnPitch(InputValue value)
        {
            SetInputValue(ref _pitchInput, value.Get<float>());
        }
        
        public void OnRoll(InputValue value)
        {
            SetInputValue(ref _rollInput, value.Get<float>());
        }
        public void OnStrafe(InputValue value)
        {
            SetInputValue(ref _strafeInput, value.Get<float>());
        }
        
        public void OnSurge(InputValue value)
        {
            SetInputValue(ref _surgeInput, value.Get<float>());
        }
        
        public void OnThrust(InputValue value)
        {
            SetInputValue(ref _thrustInput, value.Get<float>());
        }
        
        // private void OnEnable()
        // {
        //     _inputActionAsset.Enable();
        //
        //     _inputSurge.action.canceled += OnSurgeInputChanged;
        //     _inputSurge.action.performed += OnSurgeInputChanged;
        //     _inputSurge.action.started += OnSurgeInputChanged;
        //
        //     _inputStrafe.action.canceled += OnStrafeInputChanged;
        //     _inputStrafe.action.performed += OnStrafeInputChanged;
        //     _inputStrafe.action.started += OnStrafeInputChanged;
        //
        //     _inputYaw.action.canceled += OnYawInputChanged;
        //     _inputYaw.action.performed += OnYawInputChanged;
        //     _inputYaw.action.started += OnYawInputChanged;
        //
        //     _inputThrust.action.canceled += OnThrustInputchanged;
        //     _inputThrust.action.performed += OnThrustInputchanged;
        //     _inputThrust.action.started += OnThrustInputchanged;
        //     
        //     
        //     _inputPitch.action.canceled += OnPitchInputchanged;
        //     _inputPitch.action.performed += OnPitchInputchanged;
        //     _inputPitch.action.started += OnPitchInputchanged;
        //     
        //     
        //     _inputRoll.action.canceled += OnRollInputchanged;
        //     _inputRoll.action.performed += OnRollInputchanged;
        //     _inputRoll.action.started += OnRollInputchanged;
        // }
        //
        // private void OnDisable()
        // {
        //     _inputSurge.action.canceled -= OnSurgeInputChanged;
        //     _inputSurge.action.performed -= OnSurgeInputChanged;
        //     _inputSurge.action.started -= OnSurgeInputChanged;
        //
        //     _inputStrafe.action.canceled -= OnStrafeInputChanged;
        //     _inputStrafe.action.performed -= OnStrafeInputChanged;
        //     _inputStrafe.action.started -= OnStrafeInputChanged;
        //
        //     _inputYaw.action.canceled -= OnYawInputChanged;
        //     _inputYaw.action.performed -= OnYawInputChanged;
        //     _inputYaw.action.started -= OnYawInputChanged;
        //
        //     _inputThrust.action.canceled -= OnThrustInputchanged;
        //     _inputThrust.action.performed -= OnThrustInputchanged;
        //     _inputThrust.action.started -= OnThrustInputchanged;
        //     
        //     _inputPitch.action.canceled -= OnPitchInputchanged;
        //     _inputPitch.action.performed -= OnPitchInputchanged;
        //     _inputPitch.action.started -= OnPitchInputchanged;
        //     
        //     _inputRoll.action.canceled -= OnRollInputchanged;
        //     _inputRoll.action.performed -= OnRollInputchanged;
        //     _inputRoll.action.started -= OnRollInputchanged;
        //     
        //     _inputActionAsset.Disable();
        // }
        //
        //
        
        private void SetInputValue(ref bool button, bool state)
        {
            button = state;
        }
        
        private void SetInputValue(ref float axis, float value)
        {
            value = Mathf.Clamp(value, -1, 1);
            axis = value;
        }
        //
        // private void OnSurgeInputChanged(InputAction.CallbackContext eventData)
        // {
        //     SetInputValue(ref _surgeInput, eventData.ReadValue<float>());
        // }
        //
        // private void OnStrafeInputChanged(InputAction.CallbackContext eventData)
        // {
        //     SetInputValue(ref _strafeInput, eventData.ReadValue<float>());
        // }
        //
        // private void OnYawInputChanged(InputAction.CallbackContext eventData)
        // {
        //     SetInputValue(ref _yawInput, eventData.ReadValue<float>());
        // }
        //
        // private void OnThrustInputchanged(InputAction.CallbackContext eventData)
        // {
        //     SetInputValue(ref _thrustInput, eventData.ReadValue<float>());
        // }
        //
        //
        // private void OnPitchInputchanged(InputAction.CallbackContext eventData)
        // {
        //     SetInputValue(ref _pitchInput, eventData.ReadValue<float>());
        // }
        //
        // private void OnRollInputchanged(InputAction.CallbackContext eventData)
        // {
        //     SetInputValue(ref _rollInput, eventData.ReadValue<float>());
        // }
  
    }
}
