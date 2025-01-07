using UnityEngine;

namespace AVM {
    [DisallowMultipleComponent]
    public class DestroyMissile : AdvancedMissileBase {
        private float m_destroyTime = -1;
        private float m_aliveTime;
        private float m_fallTime = 0;
        private Rigidbody m_rb;

        private void Awake() {
            parent = this.GetComponent<AdvancedMissile>();
            m_rb = this.gameObject.GetComponent<Rigidbody>();
        }

        void Start() {
            m_aliveTime = 0;
            if (parent.DestroyTimeMin != -1 &&
                parent.DestroyTimeMax != -1) {
                m_destroyTime = Random.Range(parent.DestroyTimeMin, parent.DestroyTimeMax);
            }
            parent.IsFall = false;
        }

        void Update() {
            if (m_destroyTime != -1) {
                m_aliveTime += Time.deltaTime;
                if (m_aliveTime >= m_destroyTime) {
                    m_aliveTime = 0;
                    DestroyExplosion();
                }
            }
            if (parent.CanFallFlag) {
                m_fallTime += Time.deltaTime;
                if (m_fallTime >= parent.FallStartTime) {
                    m_rb.useGravity = true;
                    m_rb.drag = parent.Drag;
                    m_rb.constraints = RigidbodyConstraints.None;
                    if (parent.MoveVector == Vector3.zero) {
                        parent.MoveVector = this.transform.forward;
                    }
                    this.transform.rotation = Quaternion.LookRotation(parent.MoveVector);
                    parent.IsFall = true;
                }
            }
        }

        public void DestroyExplosion() {
            //Explosion Se
            if (parent.AudioSourceSEObj != null) {
                Destroy(parent.AudioSourceSEObj, 5.0f);
                parent.AudioSourceSE.transform.SetParent(null);
                if (parent.ExplosionSE != null) {
                    parent.AudioSourceSE.PlayOneShot(parent.ExplosionSE, parent.ExplosionSEVolume);
                }
            }

            //Effect-Smoke
            if (!parent.KeepChild && parent.SmokeEffectObj != null) {
                parent.SmokeEffectObj.transform.SetParent(null);
                var comp = parent.SmokeEffectObj.GetComponent<ParticleSystem>();
                if (comp != null) {
                    comp.Stop();
                }
                Destroy(parent.SmokeEffectObj, parent.SmokeDestroyTime);
            }

            Destroy(this.gameObject);
        }
    }
}