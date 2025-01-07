using System.Collections.Generic;
using UnityEngine;

namespace AVM {
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(BoxCollider))]
    [DisallowMultipleComponent]
    public class AdvancedMissile : MonoBehaviour {
        //--------------------------------------------------------------
        //Destroy
        //--------------------------------------------------------------
        [SerializeField]
        private bool m_isActiveDestroyComp = false;
        [SerializeField]
        private float m_destroyTimeMin = -1;
        [SerializeField]
        private float m_destroyTimeMax = -1;
        [SerializeField]
        private bool m_canFallFlag = false;
        [SerializeField]
        private float m_fallStartTime = 0;
        [SerializeField]
        private float m_drag = 0;

        //--------------------------------------------------------------
        //Fuse
        //--------------------------------------------------------------
        public enum FuseTypes {
            SuperQuick,
            Time,
            Proximity,
            Height
        }
        public enum HeightTypes {
            Position,
            Raycast
        }
        [SerializeField]
        private bool m_isActiveFuseComp = false;
        [SerializeField]
        private FuseTypes m_fuseType;
        [SerializeField]
        private float m_time;
        [SerializeField]
        private float m_targetDistance;
        [SerializeField]
        private float m_height;
        [SerializeField]
        private HeightTypes m_heightType;
        [SerializeField]
        private LayerMask m_layerMask;

        //--------------------------------------------------------------
        //Collision
        //--------------------------------------------------------------
        [SerializeField]
        private bool m_isActiveCollisionComp = false;
        [SerializeField]
        private bool m_enableCollision = true;
        [SerializeField]
        private bool m_collideEachOther = true;
        [SerializeField]
        private float m_enableCollisionInterval = 0;
        public enum RayColliders {
            None,
            Line,
            Capsule,
        }
        [SerializeField]
        private RayColliders m_rayCollider;
        [SerializeField]
        private float m_capsuleLinerRadius = 1;

        //--------------------------------------------------------------
        //Search
        //--------------------------------------------------------------
        public enum SearchTypes {
            Tag,
            Name,
        }
        [SerializeField]
        private bool m_isActiveSearchComp = false;
        [SerializeField]
        private SearchTypes m_searchType;
        [SerializeField]
        private List<string> m_targetTags = new List<string>() { "Player" };
        [SerializeField]
        private List<string> m_targetNames = new List<string>() { "Player" };
        [SerializeField]
        private bool m_canResearch = false;
        [SerializeField]
        private float m_searchInterval = 0;
        [SerializeField]
        public Vector2 m_sight = new Vector2(360f, 360f);
        [SerializeField]
        private float m_activeFollowInterval = 0;

        //--------------------------------------------------------------
        //Avoidance
        //--------------------------------------------------------------
        [SerializeField]
        private bool m_isActiveAvoidComp = false;
        [SerializeField]
        private bool m_enableAvoid = false;
        [SerializeField]
        private float m_distanceBetweenObstacle = 10;
        [SerializeField]
        private float m_createInterval = 0.5f;
        [SerializeField]
        private bool m_canCreateUpRoute = false;
        [SerializeField]
        private bool m_canCreateDownRoute = false;
        [SerializeField]
        private bool m_drawRoute = false;

        //--------------------------------------------------------------
        //Offset
        //--------------------------------------------------------------
        [SerializeField]
        private bool m_isActiveOffsetComp = false;
        [SerializeField]
        private Vector3 m_offset = Vector3.zero;
        [SerializeField]
        private bool m_activeRandomOffset = false;
        [SerializeField]
        private float m_randomAmplitude;
        [SerializeField]
        private AnimationCurve m_randomOffsetX = new AnimationCurve();
        [SerializeField]
        private AnimationCurve m_randomOffsetY = new AnimationCurve();
        [SerializeField]
        private AnimationCurve m_randomOffsetZ = new AnimationCurve();
        [SerializeField]
        private float m_randomOffsetInterval;
        [SerializeField]
        private float m_randomOffsetIntervalMin;
        [SerializeField]
        private float m_randomOffsetIntervalMax;

        //--------------------------------------------------------------
        //Movement
        //--------------------------------------------------------------
        public enum MoveTypes {
            Translate,
            AddForce,
        }
        public enum RotateTypes {
            NoRotate,
            PerSecond,
            Torque,
        }
        public enum MoveDirections {
            forward,
            back,
            right,
            left,
            up,
            down
        }
        [SerializeField]
        private bool m_isActiveMovementComp = false;
        [SerializeField]
        private MoveTypes m_moveType;
        [SerializeField]
        private RotateTypes m_rotType;
        [SerializeField]
        private MoveDirections m_direction = MoveDirections.forward;
        [SerializeField]
        private ForceMode m_forceMode;
        [SerializeField]
        private float m_minPower = 1;
        [SerializeField]
        private float m_maxPower = 1;
        [SerializeField]
        private float m_minSpeed = 1;
        [SerializeField]
        private float m_maxSpeed = 1;
        [SerializeField]
        private float m_anglePerSecond = 90;
        [SerializeField]
        private float m_torquePower = 1;

        //--------------------------------------------------------------
        //Audios
        //--------------------------------------------------------------
        [SerializeField]
        private bool m_isActiveAudioComp = false;
        [SerializeField]
        private AudioClip m_shotSE;
        [SerializeField]
        private float m_shotSEVolume = 1;
        [SerializeField]
        private AudioClip m_moveSE;
        [SerializeField]
        private float m_moveSEVolume = 1;
        [SerializeField]
        private AudioClip m_explosionSE;
        [SerializeField]
        private float m_explosionSEVolume = 1;

        //--------------------------------------------------------------
        //Effects
        //--------------------------------------------------------------
        [SerializeField]
        private bool m_isActiveEffectComp = false;
        [SerializeField]
        private Vector3 m_smokeOffset = new Vector3(0, 0, 0);
        [SerializeField]
        private bool m_keepChild = false;
        [SerializeField]
        private float m_smokeDestroyTime = 0;
        [SerializeField]
        private GameObject m_smokeEffect;
        [SerializeField]
        private GameObject m_explosionEffect;
        [SerializeField]
        private float m_explosionScale = 1;

        //--------------------------------------------------------------
        //Events
        //--------------------------------------------------------------
        public enum Values {
            None,
            Int,
            Float,
            String,
            Bool,
            Vector2,
            Vector3,
            Gameobject,
            AudioClip
        }
        [SerializeField]
        private bool m_isActiveEventComp = false;
        [SerializeField]
        private string m_callMethodName;
        [SerializeField]
        private Values m_eventValues;
        [SerializeField]
        private int m_intValue;
        [SerializeField]
        private float m_floatValue;
        [SerializeField]
        private string m_stringValue;
        [SerializeField]
        private bool m_boolValue;
        [SerializeField]
        private Vector2 m_vector2Value;
        [SerializeField]
        private Vector3 m_vector3Value;
        [SerializeField]
        private GameObject m_gameObjectValue;
        [SerializeField]
        private AudioClip m_audioClipValue;
        [SerializeField]
        private List<object> m_valueList;

        //--------------------------------------------------------------
        //Property
        //--------------------------------------------------------------
        //Destroy
        public float DestroyTimeMin { get { return m_destroyTimeMin; } }
        public float DestroyTimeMax { get { return m_destroyTimeMax; } }
        public float Drag { get { return m_drag; } }
        public bool CanFallFlag { get { return m_canFallFlag; } }
        public bool IsFall { set; get; }
        public float FallStartTime { get { return m_fallStartTime; } }

        //Fuse
        public FuseTypes FuseType { get { return m_fuseType; } }
        public float Time { get { return m_time; } }
        public float TargetDistance { get { return m_targetDistance; } }
        public float Height { get { return m_height; } }
        public HeightTypes HeightType { get { return m_heightType; } }
        public LayerMask LayerMask { get { return m_layerMask; } }

        //Collision
        public bool EnableCollision { get { return m_enableCollision; } }
        public bool CollideEachOther { get { return m_collideEachOther; } }
        public float EnableCollisionInterval { get { return m_enableCollisionInterval; } }
        public RayColliders RayCollider { get { return m_rayCollider; } }
        public float CapsuleLinerRadius { get { return m_capsuleLinerRadius; } }
        public Vector3 HitPosition { set; get; }
        public GameObject HitObject { set; get; }

        //Search
        public SearchTypes SearchType { get { return m_searchType; } }
        public List<string> TargetTags { get { return m_targetTags; } }
        public List<string> TargetNames { get { return m_targetNames; } }
        public bool CanResearch
        {
            get { return m_canResearch; }
            set { m_canResearch = value; }
        }
        public Vector2 Sight { 
            get { return m_sight; }
            set { m_sight = value; }
        }
        public float SearchInterval { get { return m_searchInterval; } }
        public float FollowActiveInterval { get { return m_activeFollowInterval; } }
        public GameObject FollowTarget { set; get; }
        public GameObject MainFollowTarget { set; get; }
        public bool IsInSight { set; get; }
        public bool IsFollowStart { set; get; }

        //Avoidance
        public bool EnableAvid { get { return m_enableAvoid; } }
        public float DistanceBetweenObstacle { get { return m_distanceBetweenObstacle; } }
        public float AvoidanceRouteNodeCreateInterval { get { return m_createInterval; } }
        public bool CanCreateUpRoute { get { return m_canCreateUpRoute; } }
        public bool CanCreateDownRoute { get { return m_canCreateDownRoute; } }
        public bool DrawRoute { get { return m_drawRoute; } }

        //Offset
        public Vector3 Offset { get { return m_offset; } }
        public bool ActiveRandomOffset { get { return m_activeRandomOffset; } }
        public float RandomAmplitude { get { return m_randomAmplitude; } }
        public AnimationCurve RandomOffsetX { get { return m_randomOffsetX; } }
        public AnimationCurve RandomOffsetY { get { return m_randomOffsetY; } }
        public AnimationCurve RandomOffsetZ { get { return m_randomOffsetZ; } }
        public float RandomOffsetInterval { set { m_randomOffsetInterval = value; } get { return m_randomOffsetInterval; } }
        public float RandomOffsetIntervalMin { get { return m_randomOffsetIntervalMin; } }
        public float RandomOffsetIntervalMax { get { return m_randomOffsetIntervalMax; } }
        public Vector3 RandomOffset { set; get; }

        //Movement
        public MoveTypes MoveType { get { return m_moveType; } }
        public RotateTypes RotType { get { return m_rotType; } }
        public MoveDirections MoveDirection { get { return m_direction; } }
        public ForceMode ForceMode { get { return m_forceMode; } }
        public float MinPower { get { return m_minPower; } }
        public float MaxPower { get { return m_maxPower; } }
        public float MinSpeed { get { return m_minSpeed; } }
        public float MaxSpeed { get { return m_maxSpeed; } }
        public float AnglePerSecond { get { return m_anglePerSecond; } }
        public float TorquePower { get { return m_torquePower; } }
        public Vector3 MoveVector { set; get; }
        public Vector3 BeforePosition { set; get; }
        public Vector3 AfterPosition { set; get; }

        //Audio
        public AudioClip ExplosionSE { get { return m_explosionSE; } }
        public float ExplosionSEVolume { get { return m_explosionSEVolume; } }
        public AudioClip MoveSe { get { return m_moveSE; } }
        public float MoveSEVolume { get { return m_moveSEVolume; } }
        public AudioClip ShotSe { get { return m_shotSE; } }
        public float ShotSEVolume { get { return m_shotSEVolume; } }
        public GameObject AudioSourceSEObj { set; get; }
        public AudioSource AudioSourceSE { set; get; }

        //Effect
        public GameObject ExplosionEffect { get { return m_explosionEffect; } }
        public float ExplosionScale { get { return m_explosionScale; } }
        public bool KeepChild { get { return m_keepChild; } }
        public GameObject SmokeEffectObj { set { m_smokeEffect = value; } get { return m_smokeEffect; } }
        public Vector3 SmokeOffset { get { return m_smokeOffset; } }
        public float SmokeDestroyTime { get { return m_smokeDestroyTime; } }

        //Event
        public string CallMethodName { get { return m_callMethodName; } }
        public Values EventValue { get { return m_eventValues; } }
        public List<object> EventValueList { set { m_valueList = value; } get { return m_valueList; } }
        public int IntValue { get { return m_intValue; } }
        public float FloatValue { get { return m_floatValue; } }
        public string StringValue { get { return m_stringValue; } }
        public bool BoolValue { get { return m_boolValue; } }
        public Vector2 Vector2Value { get { return m_vector2Value; } }
        public Vector3 Vector3Value { get { return m_vector3Value; } }
        public GameObject GameObjectValue { get { return m_gameObjectValue; } }
        public AudioClip AudioClipValue { get { return m_audioClipValue; } }

        private void Awake() {
            if (m_isActiveDestroyComp) {
                this.gameObject.AddComponent<DestroyMissile>();
            }
            if (m_isActiveFuseComp) {
                this.gameObject.AddComponent<Fuse>();
            }
            if (m_isActiveAudioComp) {
                this.gameObject.AddComponent<CreateAudioObject>();
            }
            if (m_isActiveCollisionComp) {
                this.gameObject.AddComponent<MissileCollider>();
            }
            if (m_isActiveSearchComp) {
                this.gameObject.AddComponent<SearchTarget>();
            }
            if (m_isActiveAvoidComp) {
                this.gameObject.AddComponent<CreateAvoidanceNode>();
            }
            if (m_isActiveOffsetComp) {
                this.gameObject.AddComponent<Offset>();
            }
            if (m_isActiveMovementComp) {
                this.gameObject.AddComponent<Movement>();
            }
            if (m_isActiveEffectComp) {
                this.gameObject.AddComponent<CreateEffect>();
            }
            if (m_isActiveEventComp) {
                this.gameObject.AddComponent<EventSender>();
            }
        }
    }
}