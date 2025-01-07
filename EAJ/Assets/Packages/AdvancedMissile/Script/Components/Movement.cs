using UnityEngine;

namespace AVM {
    [DisallowMultipleComponent]
    public class Movement : AdvancedMissileBase {
        private Rigidbody m_rb;
        private float m_speed;
        private float m_power;
        private Vector3 m_forwardDirection;

        private void Awake() {
            parent = this.GetComponent<AdvancedMissile>();
            m_rb = this.gameObject.GetComponent<Rigidbody>();
        }

        void Start() {
            switch (parent.MoveType) {
                case AdvancedMissile.MoveTypes.Translate:
                    m_rb.constraints = RigidbodyConstraints.FreezePosition;
                    break;
                case AdvancedMissile.MoveTypes.AddForce:
                    break;
            }
            m_speed = Random.Range(parent.MinSpeed, parent.MaxSpeed);
            m_power = Random.Range(parent.MinPower, parent.MaxPower);
            switch (parent.MoveDirection) {
                case AdvancedMissile.MoveDirections.forward:
                    m_forwardDirection = Vector3.forward;
                    break;

                case AdvancedMissile.MoveDirections.back:
                    m_forwardDirection = Vector3.back;
                    break;

                case AdvancedMissile.MoveDirections.right:
                    m_forwardDirection = Vector3.right;
                    break;

                case AdvancedMissile.MoveDirections.left:
                    m_forwardDirection = Vector3.left;
                    break;

                case AdvancedMissile.MoveDirections.up:
                    m_forwardDirection = Vector3.up;
                    break;

                case AdvancedMissile.MoveDirections.down:
                    m_forwardDirection = Vector3.down;
                    break;

                default:
                    m_forwardDirection = Vector3.zero;
                    break;
            }
        }

        void FixedUpdate() {
            parent.BeforePosition = parent.AfterPosition;
            var forwardVec = this.transform.TransformDirection(m_forwardDirection);
            //Rotation
            if (!parent.IsFall && parent.IsInSight && parent.IsFollowStart && parent.FollowTarget != null) {

                //parent = null;

                var toTargetVec = parent.FollowTarget.transform.position + parent.Offset + parent.RandomOffset - this.transform.position;
                switch (parent.RotType) {
                    case AdvancedMissile.RotateTypes.PerSecond:
                        var angleDiff = Vector3.Angle(toTargetVec, forwardVec);
                        var toRotateTarget = Quaternion.LookRotation(toTargetVec);
                        var angleAdd = (parent.AnglePerSecond * Time.deltaTime);
                        var t = (angleAdd / angleDiff);
                        this.transform.rotation = Quaternion.Slerp(this.transform.rotation, toRotateTarget, t);
                        break;
                    case AdvancedMissile.RotateTypes.Torque:
                        var toTargetTorqueVec = Vector3.Cross(this.transform.TransformDirection(Vector3.forward) * m_speed, toTargetVec);
                        m_rb.AddTorque(toTargetTorqueVec * parent.TorquePower);
                        break;
                }
            }

            //Move
            switch (parent.MoveType) {
                case AdvancedMissile.MoveTypes.Translate:
                    this.transform.Translate(m_forwardDirection * m_speed);
                    break;
                case AdvancedMissile.MoveTypes.AddForce:
                    m_rb.AddForce(forwardVec * m_power, parent.ForceMode);
                    break;
            }
            parent.AfterPosition = this.transform.position;
            parent.MoveVector = parent.AfterPosition - parent.BeforePosition;
        }
    }
}