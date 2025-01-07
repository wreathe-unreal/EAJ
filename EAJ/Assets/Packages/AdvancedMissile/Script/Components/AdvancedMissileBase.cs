using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AVM {
    public class AdvancedMissileBase : MonoBehaviour {
        protected AdvancedMissile parent;

        private void OnEnable() {
            this.hideFlags = HideFlags.HideInInspector;
        }
    }
}