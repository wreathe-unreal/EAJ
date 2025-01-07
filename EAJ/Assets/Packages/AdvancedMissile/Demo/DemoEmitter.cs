using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class DemoEmitter : MonoBehaviour
{
    private List<GameObject> missiles = new List<GameObject>();
    private int select = 0;
    private Text infomation;

    void Awake()
    {
        missiles.Add(Resources.Load("MissilePrefab/Rocket") as GameObject);
        missiles.Add(Resources.Load("MissilePrefab/SimpleMissile(Rotate-Torque)") as GameObject);
        missiles.Add(Resources.Load("MissilePrefab/MultiMissileEmitter") as GameObject);
        missiles.Add(Resources.Load("MissilePrefab/LowPowerFallMissile") as GameObject);
        missiles.Add(Resources.Load("MissilePrefab/HighSpeedRocket(LineCollider)") as GameObject);
        missiles.Add(Resources.Load("MissilePrefab/AvoidanceMissile") as GameObject);
        missiles.Add(Resources.Load("MissilePrefab/RandomMoveBeam") as GameObject);
        missiles.Add(Resources.Load("MissilePrefab/SmoothMoveBeam") as GameObject);
        infomation = GameObject.Find("Infomation").GetComponent<Text>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            select++;
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            select--;
        }
        select = select > (missiles.Count - 1) ? 0 : select;
        select = select < 0 ? (missiles.Count - 1) : select;
        if (Input.GetMouseButtonDown(0))
        {
            Instantiate(missiles[select], this.transform.position, this.transform.rotation);
        }
        infomation.text = "[AdvancedMissile Demo]\n\n";
        infomation.text += "[W][A][S][D][SPACE][LEFT SHIFT]:Move\n\n";
        infomation.text += "[E]:Next\n";
        infomation.text += "[Q]:Prev\n";
        infomation.text += "[LeftClick]:Shot\n\n";
        infomation.text += "[" + select + "]：" + missiles[select].gameObject.name + "\n\n";
        infomation.text += "[Alt]+[F4]Quit";
    }
}