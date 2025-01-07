using UnityEngine;

namespace AVM {
    public class Fuse : AdvancedMissileBase {
        private DestroyMissile m_destroyComp;
        private float m_timeCount = 0;

        private void Awake() {
            parent = this.GetComponent<AdvancedMissile>();
            m_destroyComp = this.gameObject.GetComponent<DestroyMissile>();
        }

        void Start() {

        }

        void Update() {
            switch (parent.FuseType) {
                case AdvancedMissile.FuseTypes.SuperQuick:
                    //Default "OnTriggerEnter" by collider(AVM)
                    break;
                case AdvancedMissile.FuseTypes.Time:
                    m_timeCount += Time.deltaTime;
                    if (m_timeCount > parent.Time) {
                        m_destroyComp.DestroyExplosion();
                    }
                    break;
                case AdvancedMissile.FuseTypes.Proximity:
                    if (parent.FollowTarget != null) {
                        if (Vector3.Distance(parent.FollowTarget.transform.position, this.transform.position) < parent.TargetDistance) {
                            m_destroyComp.DestroyExplosion();
                        }
                    }
                    break;
                case AdvancedMissile.FuseTypes.Height:
                    if (parent.MoveVector.y < 0) {
                        switch (parent.HeightType) {
                            case AdvancedMissile.HeightTypes.Position:
                                if (this.transform.position.y < parent.Height) {
                                    m_destroyComp.DestroyExplosion();
                                }
                                break;
                            case AdvancedMissile.HeightTypes.Raycast:
                                if (Physics.Raycast(this.transform.position, Vector3.down, parent.Height, parent.LayerMask)) {
                                    m_destroyComp.DestroyExplosion();
                                }
                                break;
                        }
                    }
                    break;
            }
        }
    }
}