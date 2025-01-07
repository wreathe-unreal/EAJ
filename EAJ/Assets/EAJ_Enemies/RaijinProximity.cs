using System.Collections;
using System.Collections.Generic;
using EAJ;
using UnityEngine;

namespace EAJ
{
    public class RaijinProximity : MonoBehaviour
    {
        public Enemy Raijin;
        public BulletSphereSpawner ProximityShield;

        private SixDOFController PlayerController;

        // Start is called before the first frame update
        void Start()
        {
            PlayerController = FindObjectOfType<SixDOFController>();

        }

        // Update is called once per frame
        void Update()
        {
            if (Vector3.Distance(PlayerController.transform.position, transform.position) < 40f)
            {
                ProximityShield.enabled = true;
            }
            else
            {
                ProximityShield.enabled = false;
            }

            if (Raijin.Health < 0)
            {
                ProximityShield.enabled = false;
            }

        }
    }
}