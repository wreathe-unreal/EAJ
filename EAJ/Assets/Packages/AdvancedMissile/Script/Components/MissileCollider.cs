using UnityEngine;

namespace AVM {
    [DisallowMultipleComponent]
    public class MissileCollider : AdvancedMissileBase {
        private float m_collisionInterval = 0;
        private BoxCollider m_boxCollider;
        private bool m_isActiveCollider;
        private DestroyMissile m_destroyComp;

        void Awake() {
            parent = this.GetComponent<AdvancedMissile>();
            m_destroyComp = this.gameObject.GetComponent<DestroyMissile>();
            m_boxCollider = this.gameObject.GetComponent<BoxCollider>();
        }

        void Start() {
            m_boxCollider.enabled = false;
            m_boxCollider.isTrigger = true;
        }

        void Update() {
            if (parent.EnableCollision && !m_isActiveCollider) {
                m_collisionInterval += Time.deltaTime;
                if (m_collisionInterval >= parent.EnableCollisionInterval) {
                    m_collisionInterval = 0;
                    m_boxCollider.enabled = true;
                    m_isActiveCollider = true;
                }
            }

            if (m_isActiveCollider && parent.RayCollider != AdvancedMissile.RayColliders.None) {
                RaycastHit hit = new RaycastHit();
                bool isRayHit = false;
                switch (parent.RayCollider) {
                    case AdvancedMissile.RayColliders.Line:
                        isRayHit = Physics.Raycast(parent.BeforePosition, parent.MoveVector, out hit, parent.MoveVector.magnitude);
                        break;
                    case AdvancedMissile.RayColliders.Capsule:
                        isRayHit = Physics.CapsuleCast(parent.BeforePosition, parent.AfterPosition, parent.CapsuleLinerRadius, parent.MoveVector, out hit, parent.MoveVector.magnitude);
                        break;
                }

                if (isRayHit && hit.collider.gameObject.GetInstanceID() != this.gameObject.GetInstanceID()) {
                    if (parent.CollideEachOther || hit.collider.GetComponent<AdvancedMissile>() == null) {
                        parent.HitPosition = hit.point;
                        parent.HitObject = hit.collider.gameObject;
                        m_destroyComp.DestroyExplosion();
                    }
                }
            }
        }

        private void OnTriggerEnter(Collider collider) {
            if (parent.EnableCollision) {
                if (parent.CollideEachOther || collider.GetComponent<AdvancedMissile>() == null) {
                    parent.HitObject = collider.gameObject;
                    m_destroyComp.DestroyExplosion();
                }
            }
        }
    }
}