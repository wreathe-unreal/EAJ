using UnityEngine;

namespace AVM {
    [DisallowMultipleComponent]
    public class CreateEffect : AdvancedMissileBase {
        private void Awake() {
            parent = this.GetComponent<AdvancedMissile>();
            if (parent.SmokeEffectObj != null) {
                GameObject smoke = Instantiate(parent.SmokeEffectObj, this.transform.position, this.transform.rotation);
                smoke.transform.parent = this.transform;
                smoke.transform.localPosition = parent.SmokeOffset;
                parent.SmokeEffectObj = smoke;
            }
        }

        private void OnDestroy() {
            //Explosion
            if (parent.ExplosionEffect != null) {
                GameObject inst;
                if (parent.HitPosition == Vector3.zero) {
                    inst = Instantiate(parent.ExplosionEffect, this.transform.position, this.transform.rotation) as GameObject;
                } else {
                    Vector3 emitPos = (Vector3)parent.HitPosition;
                    inst = Instantiate(parent.ExplosionEffect, emitPos, this.transform.rotation) as GameObject;
                }
                inst.transform.localScale = new Vector3(parent.ExplosionScale, parent.ExplosionScale, parent.ExplosionScale);
                Destroy(inst, 5.0f);
            }
        }
    }
}