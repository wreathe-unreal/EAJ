using UnityEngine;

namespace AVM {
    [DisallowMultipleComponent]
    public class Offset : AdvancedMissileBase {
        private float m_initDistance;
        private float m_timeCount = 0;

        private void Awake() {
            parent = this.GetComponent<AdvancedMissile>();
        }

        void Start() {
            SetNewRandomOffset();
        }

        void Update() {
            m_timeCount += Time.deltaTime;
            if (m_timeCount > parent.RandomOffsetInterval) {
                m_timeCount = 0;
                SetNewRandomOffset();
            }
        }

        void SetNewRandomOffset() {
            if (parent.FollowTarget != null) {
                m_initDistance = Vector3.Distance(parent.FollowTarget.transform.position, this.transform.position);
                var distance = m_initDistance;

                var xCurve = parent.RandomOffsetX.Evaluate(distance / m_initDistance) * parent.RandomAmplitude;
                var yCurve = parent.RandomOffsetY.Evaluate(distance / m_initDistance) * parent.RandomAmplitude;
                var zCurve = parent.RandomOffsetZ.Evaluate(distance / m_initDistance) * parent.RandomAmplitude;
                var x = Random.Range(-xCurve, xCurve);
                var y = Random.Range(-yCurve, yCurve);
                var z = Random.Range(-zCurve, zCurve);
                parent.RandomOffset = new Vector3(x, y, z);
            }
            parent.RandomOffsetInterval = Random.Range(parent.RandomOffsetIntervalMin, parent.RandomOffsetIntervalMax);
        }
    }
}