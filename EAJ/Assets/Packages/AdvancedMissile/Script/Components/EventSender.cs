using UnityEngine;
using System.Collections.Generic;

namespace AVM {
    [DisallowMultipleComponent]
    public class EventSender : AdvancedMissileBase {
        private void Awake() {
            parent = this.GetComponent<AdvancedMissile>();
        }

        void Start() {
            if (parent.EventValueList == null || parent.EventValueList.Count == 0) {
                parent.EventValueList = new List<object>();
                parent.EventValueList.Add(parent.IntValue);
                parent.EventValueList.Add(parent.FloatValue);
                parent.EventValueList.Add(parent.StringValue);
                parent.EventValueList.Add(parent.BoolValue);
                parent.EventValueList.Add(parent.Vector2Value);
                parent.EventValueList.Add(parent.Vector3Value);
                parent.EventValueList.Add(parent.GameObjectValue);
                parent.EventValueList.Add(parent.AudioClipValue);
            }
        }

        private void OnDestroy() {
            if (parent.HitObject != null && parent.CallMethodName != "" && parent.FollowTarget != null) {
                bool isActiveSend = false;
                switch (parent.SearchType) {
                    case AdvancedMissile.SearchTypes.Tag:
                        if (parent.HitObject.gameObject.tag == parent.FollowTarget.gameObject.tag) {
                            isActiveSend = true;
                        }
                        break;
                    case AdvancedMissile.SearchTypes.Name:
                        if (parent.HitObject.gameObject.name == parent.FollowTarget.gameObject.name) {
                            isActiveSend = true;
                        }
                        break;
                }

                if (isActiveSend && (int)parent.EventValue != 0) {
                    parent.HitObject.gameObject.SendMessage(parent.CallMethodName, parent.EventValueList[(int)parent.EventValue - 1]);
                } else {
                    parent.HitObject.gameObject.SendMessage(parent.CallMethodName);
                }
            }
        }
    }
}