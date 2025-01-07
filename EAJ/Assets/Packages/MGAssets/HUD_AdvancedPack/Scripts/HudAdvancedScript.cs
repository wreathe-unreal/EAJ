using UnityEngine;
using UnityEngine.UI;

/////////////////////////////////////////////////////////////////////////////////////
////////////////////// Advanced HUD Script - Version 1.0.190803 - Unity 2018.3.4f1
/////////////////////////////////////////////////////////////////////////////////////

public class HudAdvancedScript : MonoBehaviour
{
    public static HudAdvancedScript current;


    //Config Variables
    [Header("Aircraft HUD")]
    public bool isActive = false;

    [Tooltip("Link your Aircraft Transform here!")] public Transform aircraft;
    [Tooltip("If your Aircraft have a RigidBody, link it here!")] public Rigidbody aircraftRB;

    [Space]
    public string activeMsg = "HUD Activated";
    public DisplayMsg consoleMsg;
    public RectTransform hudPanel;    
    //

    [Space(5)]
    [Header("Roll")]
    public bool useRoll = true;
    public float rollAmplitude = 1, rollOffSet = 0;
    [Range(0, 1)] public float rollFilterFactor = 0.25f;
    public RectTransform horizonRoll;
    public Text horizonRollTxt;

    [Space(5)]
    [Header("Pitch")]
    public bool usePitch = true;
    public float pitchAmplitude = 1, pitchOffSet = 0, pitchXOffSet = 0, pitchYOffSet = 0;
    [Range(0, 1)] public float pitchFilterFactor = 0.125f;
    public RectTransform horizonPitch;
    public Text horizonPitchTxt;

    [Space(5)]
    [Header("Heading")]
    public bool useHeading = true;
    public float headingAmplitude = 1, headingOffSet = 0;
    [Range(0, 1)] public float headingFilterFactor = 0.1f;
    public RectTransform compassHSI;
    public Text headingTxt;
    public CompassBar compassBar;


    [Space(5)]
    [Header("Altitude")]
    public bool useAltitude = true;
    public float altitudeAmplitude = 1, altitudeOffSet = 0;
    [Range(0, 1)] public float altitudeFilterFactor = 0.5f;
    public Text altitudeTxt;

    [Space(5)]
    [Header("Speed")]
    public bool useSpeed = true;
    public float speedAmplitude = 1, speedOffSet = 0;
    [Range(0, 1)] public float speedFilterFactor = 0.25f;
    public Text speedTxt;

    [Space(5)]
    [Header("Vertical Velocity")]
    public bool useVV = true;
    public float vvAmplitude = 1, vvOffSet = 0;
    [Range(0, 1)] public float vvFilterFactor = 0.1f;
    public NeedleIndicator vvNeedle;
    public ArrowIndicator vvArrow;
    public bool roundVV = true, showDecimalVV = true;
    public float roundFactorVV = 0.1f;
    public Text verticalSpeedTxt;

    [Space(5)]
    [Header("Horizontal Velocity")]
    public bool useHV = true;
    public float hvAmplitude = 1, hvOffSet = 0;
    [Range(0, 1)] public float hvFilterFactor = 0.1f;
    public NeedleIndicator hvNeedle;
    public ArrowIndicator hvArrow;
    public bool roundHV = true, showDecimalHV = true;
    public float roundFactorHV = 0.1f;
    public Text horizontalSpeedTxt;


    [Space(5)]
    [Header("G-Force")]
    public bool useGForce = true;
    public float gForceAmplitude = 1, gForceOffSet = 0;
    [Range(0, 1)] public float gForceFilterFactor = 0.25f;
    public Text gForceTxt, maxGForceTxt, minGForceTxt;



    [Space(5)]
    [Header("AOA, AOS and GlidePath")]
    public bool useAlphaBeta = true;
    public float alphaAmplitude = 1, alphaOffSet = 0;
    [Range(0, 1)] public float alphaFilterFactor = 0.25f;
    public NeedleIndicator alphaNeedle;
    public ArrowIndicator alphaArrow;
    public Text alphaTxt;

    [Space]
    public float betaAmplitude = 1;
    public float betaOffSet = 0;
    [Range(0, 1)] public float betaFilterFactor = 0.25f;
    public NeedleIndicator betaNeedle;
    public ArrowIndicator betaArrow;
    public Text betaTxt;

    [Space]
    public bool useGlidePath = true;
    [Range(0, 1)] public float glidePathFilterFactor = 0.1f;
    public float glideXDeltaClamp = 600f, glideYDeltaClamp = 700f;
    public RectTransform glidePath;


    //All Flight Variables
    [Space(10)] [Header("Flight Variables - ReadOnly!")] 
    public float speed;
    public float altitude, pitch, roll, heading, gForce, maxGForce, minGForce, alpha, beta, vv, hv;
    public Vector3 carGForce;
    //

    //Internal Calculation Variables
    Vector3 currentPosition, lastPosition, relativeSpeed, absoluteSpeed, lastSpeed, relativeAccel;

    Vector3 angularSpeed;
    Quaternion currentRotation, lastRotation, deltaTemp;
    float angleTemp = 0.0f;
    Vector3 axisTemp = Vector3.zero;

    int waitInit = 6;
    //

    //Set Default Values via Editor -> This will be implemented in future updates
    //[ContextMenu("Default Simulation")] void setDefaultSimulation() { Debug.Log("Default Simulation!"); }
    //

    //////////////////////////////////////////////////////////////////////////////////////////////////////////////// Inicialization
    void Awake()
    {
        if (aircraft == null && aircraftRB == null) aircraft = Camera.main.transform;   //If there is no reference set, then it gets the MainCamera
        if (aircraft == null && aircraftRB != null) aircraft = aircraftRB.transform;
    }
    void OnEnable()
    {
        if (aircraft == null && aircraftRB == null) aircraft = Camera.main.transform;
        ResetHud();
    }
    public void ResetHud()
    {
        current = this;
        if (aircraft == null && aircraftRB != null) aircraft = aircraftRB.transform;

        waitInit = 6;
        maxGForce = 1f; minGForce = 1f;
        if (useGForce) { if (maxGForceTxt != null) maxGForceTxt.text = "0.0"; if (minGForceTxt != null) minGForceTxt.text = "0.0"; }


        isActive = true;
        if (consoleMsg != null) DisplayMsg.current = consoleMsg;
        if (activeMsg != "") DisplayMsg.show(activeMsg, 5);

    }
    public void toogleHud()
    {
        SndPlayer.playClick();
        hudPanel.gameObject.SetActive(!hudPanel.gameObject.activeSelf);


        if (!hudPanel.gameObject.activeSelf)
        {
            isActive = false; current = null;
            DisplayMsg.show("Hud Disabled", 5);
        }
        else { if (!isActive) ResetHud(); }
    }
    //////////////////////////////////////////////////////////////////////////////////////////////////////////////// Inicialization



    /////////////////////////////////////////////////////// Updates and Calculations
    void FixedUpdate() //Update()
    {
        // Return if not active
        if (!isActive || !hudPanel.gameObject.activeSelf) return;
        if (aircraft == null) { isActive = false; return; }

        //////////////////////////////////////////// Frame Calculations
        lastPosition = currentPosition;
        lastSpeed = relativeSpeed;
        lastRotation = currentRotation;

        if (aircraft != null && aircraftRB == null) //Mode Transform
        {
            currentPosition = aircraft.transform.position;
            absoluteSpeed = (currentPosition - lastPosition) / Time.fixedDeltaTime;
            relativeSpeed = aircraft.transform.InverseTransformDirection((currentPosition - lastPosition) / Time.fixedDeltaTime);
            relativeAccel = (relativeSpeed - lastSpeed) / Time.fixedDeltaTime;
            currentRotation = aircraft.transform.rotation;

            //angular speed
            deltaTemp = currentRotation * Quaternion.Inverse(lastRotation);
            angleTemp = 0.0f;
            axisTemp = Vector3.zero;
            deltaTemp.ToAngleAxis(out angleTemp, out axisTemp);
            //
            angularSpeed = aircraft.InverseTransformDirection(angleTemp * axisTemp) * Mathf.Deg2Rad / Time.fixedDeltaTime;
            //
        }
        else if (aircraft != null && aircraftRB != null)  //Mode RB
        {
            currentPosition = aircraftRB.transform.position;
            absoluteSpeed = (currentPosition - lastPosition) / Time.fixedDeltaTime;
            relativeSpeed = aircraftRB.transform.InverseTransformDirection(aircraftRB.velocity);
            relativeAccel = (relativeSpeed - lastSpeed) / Time.fixedDeltaTime;

            angularSpeed = aircraftRB.angularVelocity;
        }
        else //Zero all values
        {
            currentPosition = Vector3.zero;
            relativeSpeed = Vector3.zero;
            relativeAccel = Vector3.zero;
            angularSpeed = Vector3.zero;

            lastPosition = currentPosition;
            lastSpeed = relativeSpeed;
            lastRotation = currentRotation;
        }
        //
        if (waitInit > 0) { waitInit--; return; } //Wait some frames for stablization before starting calculating
        //
        //////////////////////////////////////////// Frame Calculations


        //////////////////////////////////////////// Compass, Heading and/or HSI
        if (useHeading)
        {
            heading = Mathf.LerpAngle(heading, aircraft.eulerAngles.y + headingOffSet, headingFilterFactor) % 360f;

            //Send values to Gui and Instruments
            if (compassHSI != null) compassHSI.localRotation = Quaternion.Euler(0, 0, headingAmplitude * heading);
            if (compassBar != null) compassBar.setValue(heading);
            if (headingTxt != null) { if (heading < 0) headingTxt.text = (heading + 360f).ToString("000"); else headingTxt.text = heading.ToString("000"); }

        }
        //////////////////////////////////////////// Compass, Heading and/or HSI


        //////////////////////////////////////////// Roll
        if (useRoll)
        {
            roll = Mathf.LerpAngle(roll, aircraft.rotation.eulerAngles.z + rollOffSet, rollFilterFactor) % 360;

            //Send values to Gui and Instruments
            if (horizonRoll != null) horizonRoll.localRotation = Quaternion.Euler(0, 0, rollAmplitude * roll);
            if (horizonRollTxt != null)
            {
                //horizonRollTxt.text = roll.ToString("##");
                if (roll > 180) horizonRollTxt.text = (roll - 360).ToString("00");
                else if (roll < -180) horizonRollTxt.text = (roll + 360).ToString("00");
                else horizonRollTxt.text = roll.ToString("00");
            }
            //
        }
        //////////////////////////////////////////// Roll


        //////////////////////////////////////////// Pitch
        if (usePitch)
        {
            pitch = Mathf.LerpAngle(pitch, -aircraft.eulerAngles.x + pitchOffSet, pitchFilterFactor);

            //Send values to Gui and Instruments
            if (horizonPitch != null) horizonPitch.localPosition = new Vector3(-pitchAmplitude * pitch * Mathf.Sin(horizonPitch.transform.localEulerAngles.z * Mathf.Deg2Rad) + pitchXOffSet, pitchAmplitude * pitch * Mathf.Cos(horizonPitch.transform.localEulerAngles.z * Mathf.Deg2Rad) + pitchYOffSet, 0);
            if (horizonPitchTxt != null) horizonPitchTxt.text = pitch.ToString("0");
        }
        //////////////////////////////////////////// Pitch


        //////////////////////////////////////////// Altitude
        if (useAltitude)
        {
            altitude = Mathf.Lerp(altitude, altitudeOffSet + altitudeAmplitude * currentPosition.y, speedFilterFactor);

            //Send values to Gui and Instruments
            if (altitudeTxt != null) altitudeTxt.text = altitude.ToString("0").PadLeft(5);
        }
        //////////////////////////////////////////// Altitude


        //////////////////////////////////////////// Speed
        if (useSpeed)
        {
            speed = Mathf.Lerp(speed, speedOffSet + speedAmplitude * relativeSpeed.z, speedFilterFactor);

            //Send values to Gui and Instruments
            if (speedTxt != null) speedTxt.text = speed.ToString("0").PadLeft(5);//.ToString("##0");
        }
        //////////////////////////////////////////// Speed


        //////////////////////////////////////////// Vertical Velocity - VV
        if (useVV)
        {
            vv = Mathf.Lerp(vv, vvOffSet + vvAmplitude * absoluteSpeed.y, vvFilterFactor);

            //Send values to Gui and Instruments
            if (vvNeedle != null) vvNeedle.setValue(vv);
            if (vvArrow != null) vvArrow.setValue(vv);
            if (verticalSpeedTxt != null)
            {
                if (roundVV)
                {
                    if (showDecimalVV) verticalSpeedTxt.text = (System.Math.Round(vv / roundFactorVV, System.MidpointRounding.AwayFromZero) * roundFactorVV).ToString("0.0").PadLeft(4);
                    else verticalSpeedTxt.text = (System.Math.Round(vv / roundFactorVV, System.MidpointRounding.AwayFromZero) * roundFactorVV).ToString("0").PadLeft(3);
                }
                else
                {
                    if (showDecimalVV) verticalSpeedTxt.text = (vv).ToString("0.0").PadLeft(4);
                    else verticalSpeedTxt.text = (vv).ToString("0").PadLeft(3);
                }

            }
        }
        //////////////////////////////////////////// Vertical Velocity - VV


        //////////////////////////////////////////// Horizontal Velocity - HV
        if (useHV)
        {
            hv = Mathf.Lerp(hv, hvOffSet + hvAmplitude * relativeSpeed.x, hvFilterFactor);

            //Send values to Gui and Instruments
            if (hvNeedle != null) hvNeedle.setValue(hv);
            if (hvArrow != null) hvArrow.setValue(hv);
            if (horizontalSpeedTxt != null)
            {
                if (roundHV)
                {
                    if (showDecimalHV) horizontalSpeedTxt.text = (System.Math.Round(hv / roundFactorHV, System.MidpointRounding.AwayFromZero) * roundFactorHV).ToString("0.0").PadLeft(4);
                    else horizontalSpeedTxt.text = (System.Math.Round(hv / roundFactorHV, System.MidpointRounding.AwayFromZero) * roundFactorHV).ToString("0").PadLeft(3);
                }
                else
                {
                    if (showDecimalHV) horizontalSpeedTxt.text = (hv).ToString("0.0").PadLeft(4);
                    else horizontalSpeedTxt.text = (hv).ToString("0").PadLeft(3);
                }
            }
        }
        //////////////////////////////////////////// Horizontal Velocity - HV


        //////////////////////////////////////////// Vertical G-Force 
        if (useGForce)
        {
            //G-FORCE -> Gravity + Vertical Acceleration + Centripetal Acceleration (v * w) radians
            float gTotal =
                ((-aircraft.transform.InverseTransformDirection(Physics.gravity).y +
                gForceAmplitude * (relativeAccel.y - angularSpeed.x * Mathf.Abs(relativeSpeed.z)
                )) / Physics.gravity.magnitude);

            if (float.IsNaN(gTotal)) gTotal = 0;

            gForce = Mathf.Lerp(gForce, gForceOffSet + gTotal, gForceFilterFactor);
            //


            //Send values to Gui and Instruments
            if (gForceTxt != null) gForceTxt.text = gForce.ToString("0.0").PadLeft(3);
            if (gForce > maxGForce)
            {
                maxGForce = gForce;
                if (maxGForceTxt != null) maxGForceTxt.text = maxGForce.ToString("0.0").PadLeft(3);
            }
            if (gForce < minGForce)
            {
                minGForce = gForce;
                if (minGForceTxt != null) minGForceTxt.text = minGForce.ToString("0.0").PadLeft(3);
            }
            //
        }
        ////////////////////////////////////////////  Vertical G-Force 





        //////////////////////////////////////////////// AOA (Alpha) + AOS (Beta) + GlidePath (Velocity Vector)
        if (useAlphaBeta || useGlidePath)
        {
            //Calculate both Angles
            alpha = Mathf.Lerp(alpha, alphaOffSet + alphaAmplitude * Vector2.SignedAngle(new Vector2(relativeSpeed.z, relativeSpeed.y), Vector2.right), alphaFilterFactor);
            beta  = Mathf.Lerp(beta, betaOffSet + betaAmplitude * Vector2.SignedAngle(new Vector2(relativeSpeed.x, relativeSpeed.z), Vector2.up), betaFilterFactor);

            //Apply angle values to the glidePath UI element
            if (useGlidePath && glidePath != null) glidePath.localPosition = Vector3.Lerp(glidePath.localPosition, new Vector3(Mathf.Clamp(-beta * pitchAmplitude, -glideXDeltaClamp, glideXDeltaClamp), Mathf.Clamp(alpha * pitchAmplitude, -glideYDeltaClamp, glideYDeltaClamp), 0), glidePathFilterFactor);


            //Send values to Instruments
            if (useAlphaBeta)
            {
                if (alphaNeedle != null) alphaNeedle.setValue(alpha);
                if (alphaArrow != null) alphaArrow.setValue(alpha);
                if (betaNeedle != null) betaNeedle.setValue(beta);
                if (betaArrow != null) betaArrow.setValue(beta);

                //Send values to Gui Text
                if (alphaTxt != null) alphaTxt.text = alpha.ToString("0").PadLeft(3);
                if (betaTxt != null) betaTxt.text = beta.ToString("0").PadLeft(3);
            }
            //
        }
        //////////////////////////////////////////////// AOA (Alpha) + AOS (Beta)

    }
    /////////////////////////////////////////////////////// Updates and Calculations
}





//Backup formulas
//if (hvIndicator != null) hvIndicator.localPosition = new Vector3( Mathf.Clamp(hv, -hvDeltaClamp, hvDeltaClamp), hvIndicator.localPosition.y , hvIndicator.localPosition.z);
//if (roundHV) horizontalSpeedTxt.text = (System.Math.Round(hv / roundFactorHV, System.MidpointRounding.AwayFromZero) * roundFactorHV).ToString((showDecimalHV) ? "0.0" : "0").PadLeft(4);
//else horizontalSpeedTxt.text = (hv).ToString((showDecimalHV) ? "0.0" : "0").PadLeft(4);
//if (roundVV) verticalSpeedTxt.text = (System.Math.Round(vv / roundFactorVV, System.MidpointRounding.AwayFromZero) * roundFactorVV).ToString((showDecimalVV) ? "0.0" : "0").PadLeft(4);
//else verticalSpeedTxt.text = (vv).ToString((showDecimalVV) ? "0.0" : "0").PadLeft(4);
