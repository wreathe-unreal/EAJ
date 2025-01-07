using UnityEngine;

namespace AVM {
    [DisallowMultipleComponent]
    public class SearchTarget : AdvancedMissileBase {
        private float m_nowSearchInterval = 0;
        private float m_activeInterval = 0;

        private void Awake() {
            parent = this.GetComponent<AdvancedMissile>();
        }

        void Start() {
            if (parent.FollowTarget == null) {
                Search();
            }
        }

        void Update() {
            if (!parent.IsFollowStart) {
                m_activeInterval += Time.deltaTime;
                if (m_activeInterval >= parent.FollowActiveInterval) {
                    parent.IsFollowStart = true;
                }
            }

            if (parent.IsFollowStart) {
                //Research
                if (parent.CanResearch) {
                    if (m_nowSearchInterval >= parent.SearchInterval) {
                        m_nowSearchInterval = 0;
                        Search();
                    }
                    m_nowSearchInterval += Time.deltaTime;
                }

                //Sight
                if (parent.FollowTarget != null) {
                    var targeWidth = new Vector3(parent.FollowTarget.transform.position.x, 0, parent.FollowTarget.transform.position.z);
                    var targetHeight = new Vector3(0, parent.FollowTarget.transform.position.y, parent.FollowTarget.transform.position.z);
                    var thisWidth = new Vector3(this.transform.position.x, 0, this.transform.position.z);
                    var thistHeight = new Vector3(0, this.transform.position.y, this.transform.position.z);
                    var widthAngle = Vector3.Angle(targeWidth - thisWidth, this.transform.forward);
                    var heightAngle = Vector3.Angle(targetHeight - thistHeight, this.transform.forward);
                    if (widthAngle <= parent.Sight.x &&
                        heightAngle <= parent.Sight.y) {
                        parent.IsInSight = true;
                    } else {
                        parent.IsInSight = false;
                    }
                }
            }
        }

        void Search() {
            float disMin = Mathf.Infinity;
            GameObject newTargetObj = null;
            switch (parent.SearchType) {
                case AdvancedMissile.SearchTypes.Tag:
                    foreach (string str in parent.TargetTags) {
                        var objs = GameObject.FindGameObjectsWithTag(str);
                        foreach (GameObject obj in objs) {
                            if (obj != null) {
                                float newDis = Vector3.Distance(obj.transform.position, this.transform.position);
                                if (newDis < disMin) {
                                    disMin = newDis;
                                    newTargetObj = obj;
                                }
                            }
                        }
                    }
                    break;

                case AdvancedMissile.SearchTypes.Name:
                    foreach (string str in parent.TargetNames) {
                        var obj = GameObject.Find(str);
                        if (obj != null) {
                            float newDis = Vector3.Distance(obj.transform.position, this.transform.position);
                            if (newDis < disMin) {
                                disMin = newDis;
                                newTargetObj = obj;
                            }
                        }
                    }
                    break;
            }
            parent.MainFollowTarget = parent.FollowTarget = newTargetObj;
        }
    }
}