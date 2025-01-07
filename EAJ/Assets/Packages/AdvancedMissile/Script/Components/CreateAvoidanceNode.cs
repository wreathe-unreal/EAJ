using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
using System.Collections.Generic;

namespace AVM {
    [DisallowMultipleComponent]
    public class CreateAvoidanceNode : AdvancedMissileBase {
        private List<Vector3> m_nodePoints;
        private List<GameObject> m_nodes;
        private bool m_isCreate = false;
        private float m_interval;
        private GameObject m_parentNode;
        private List<string> m_targetList;
        private GameObject m_nearestNode;
        private List<GameObject> m_routeList;

        private void Awake() {
            parent = this.GetComponent<AdvancedMissile>();
            m_parentNode = GameObject.Find("AllAvoidNode");
            if (m_parentNode == null) {
                m_parentNode = new GameObject("AllAvoidNode");
            }
            m_routeList = new List<GameObject>();
        }

        void Start() {
            m_nodePoints = new List<Vector3>();
            m_nodes = new List<GameObject>();
            m_targetList = new List<string>();
        }

        void OnDestroy() {
            Initialize();
        }

        void Update() {
            if (parent.EnableAvid) {
                Vector3 toTargetVec = Vector3.zero;
                float toTargetDistance = 0;
                if (parent.MainFollowTarget != null) {
                    m_interval += Time.deltaTime;
                    var activeTarget = parent.MainFollowTarget;
                    toTargetVec = activeTarget.transform.position - this.transform.position;
                    toTargetDistance = Vector3.Distance(activeTarget.transform.position, this.transform.position);

                    #region CreateNode
                    if (m_interval > parent.AvoidanceRouteNodeCreateInterval) {
                        m_interval = 0;
                        RaycastHit hit;
                        if (Physics.Raycast(this.transform.position, toTargetVec, out hit, toTargetDistance)) {
                            //Hit(Target)
                            if (hit.collider.gameObject == parent.MainFollowTarget) {
                                m_isCreate = false;
                                Initialize();
                                parent.FollowTarget = parent.MainFollowTarget;
                            }

                            //Hit(Not Target) or Interval Time
                            else if (!m_isCreate) {
                                Initialize();
                                //Get Bounds
                                //Center
                                var center = hit.collider.bounds.center;
                                var heightDif = (this.transform.position.y - center.y) / 2;
                                center = new Vector3(center.x, center.y + heightDif, center.z);

                                //Standard Position
                                var extents = hit.collider.bounds.extents;
                                var forward = center.z + extents.z;
                                var back = center.z - extents.z;
                                var right = center.x + extents.x;
                                var left = center.x - extents.x;
                                //Side Route
                                m_nodePoints.Add(new Vector3(right, center.y, forward));
                                m_nodePoints.Add(new Vector3(right, center.y, back));
                                m_nodePoints.Add(new Vector3(left, center.y, forward));
                                m_nodePoints.Add(new Vector3(left, center.y, back));
                                //Up Route
                                if (parent.CanCreateUpRoute) {
                                    var y = center.y + extents.y;
                                    m_nodePoints.Add(new Vector3(right, y, center.z));
                                    m_nodePoints.Add(new Vector3(left, y, center.z));
                                    m_nodePoints.Add(new Vector3(center.x, y, forward));
                                    m_nodePoints.Add(new Vector3(center.x, y, back));
                                }
                                //Down Route
                                if (parent.CanCreateDownRoute) {
                                    var y = center.y - extents.y;
                                    m_nodePoints.Add(new Vector3(right, y, center.z));
                                    m_nodePoints.Add(new Vector3(left, y, center.z));
                                    m_nodePoints.Add(new Vector3(center.x, y, forward));
                                    m_nodePoints.Add(new Vector3(center.x, y, back));
                                }

                                //Node Shift
                                m_nodes = new List<GameObject>();
                                foreach (Vector3 point in m_nodePoints) {
                                    //TODO:Modification of vector("toOutVec") calculation
                                    var toOutVec = (point - center).normalized;
                                    var shiftPoint = point + toOutVec * parent.DistanceBetweenObstacle;

                                    //Ignore useless position
                                    var nodeToCenterVec = center - shiftPoint;
                                    var nodeToThisVec = this.transform.position - shiftPoint;
                                    var direction = (shiftPoint - this.transform.position).normalized;
                                    var distance = Vector3.Distance(shiftPoint, this.transform.position);
                                    if (Vector3.Angle(nodeToCenterVec, nodeToThisVec) < 150 &&
                                        !Physics.Raycast(this.transform.position, direction, distance)) {
                                        var newNodeObj = new GameObject("Node_");
                                        newNodeObj.name += newNodeObj.GetInstanceID();
                                        newNodeObj.transform.position = shiftPoint;
                                        newNodeObj.transform.parent = m_parentNode.transform;
                                        m_nodes.Add(newNodeObj);
                                        m_targetList.Add(newNodeObj.name);
                                    }
                                }
                                m_isCreate = true;
                            } else {
                                m_isCreate = false;
                            }

                            //Generate Route
                            m_routeList.Clear();
                            float distanceMin = Mathf.Infinity;

                            //Node nearest to target
                            GameObject routeEndNode = null;
                            foreach (GameObject node in m_nodes) {
                                float dis = Vector3.Distance(activeTarget.gameObject.transform.position, node.transform.position);
                                if (dis <= distanceMin) {
                                    distanceMin = dis;
                                    routeEndNode = node;
                                }
                            }

                            if (routeEndNode != null) {
                                //Node nearest to thisObject
                                GameObject startNode = null;
                                Vector3 direction = routeEndNode.transform.position - this.transform.position;
                                float distance = Vector3.Distance(this.transform.position, routeEndNode.transform.position);
                                if (Physics.Raycast(this.transform.position, direction, distance)) {
                                    distanceMin = Mathf.Infinity;
                                    foreach (GameObject node in m_nodes) {
                                        var dis = Vector3.Distance(this.gameObject.transform.position, node.transform.position);
                                        if (dis <= distanceMin && routeEndNode != node) {
                                            direction = routeEndNode.transform.position - node.transform.position;
                                            distance = Vector3.Distance(node.transform.position, routeEndNode.transform.position);
                                            if (!Physics.Raycast(node.transform.position, direction, distance)) {
                                                distanceMin = dis;
                                                startNode = node;
                                            }
                                        }
                                    }
                                    m_routeList.Insert(m_routeList.Count, startNode);
                                }
                                m_routeList.Insert(m_routeList.Count, routeEndNode);
                                parent.FollowTarget = m_routeList[0];
                            }
                        }
                        //Not hit
                        else {
                            m_isCreate = false;
                            m_nodePoints.Clear();
                            foreach (GameObject node in m_nodes) {
                                Destroy(node);
                            }
                            parent.FollowTarget = parent.MainFollowTarget;
                            m_nodes.Clear();
                        }
                    }
                    #endregion
                }

#if UNITY_EDITOR
                //DrawRoute
                if (parent.DrawRoute) {
                    Debug.DrawRay(this.transform.position, toTargetVec.normalized * toTargetDistance, Color.blue);
                    if (m_routeList.Count > 0 && m_routeList[0] != null) {
                        var sdir = m_routeList[0].transform.position - this.transform.position;
                        var sdistance = Vector3.Distance(this.transform.position, m_routeList[0].transform.position);
                        Debug.DrawRay(this.transform.position, sdir.normalized * sdistance, Color.red);
                        for (int i = 0; i < m_routeList.Count; i++) {
                            if (i + 1 < m_routeList.Count) {
                                var dir = m_routeList[i + 1].transform.position - m_routeList[i].transform.position;
                                var distance = Vector3.Distance(m_routeList[i].transform.position, m_routeList[i + 1].transform.position);
                                Debug.DrawRay(m_routeList[i].transform.position, dir.normalized * distance, Color.red);
                            }
                        }
                        var edir = m_routeList[m_routeList.Count - 1].transform.position - parent.FollowTarget.transform.position;
                        var edistance = Vector3.Distance(parent.FollowTarget.transform.position, m_routeList[m_routeList.Count - 1].transform.position);
                        Debug.DrawRay(parent.FollowTarget.transform.position, edir.normalized * edistance, Color.red);
                    }
                }
#endif
            }
        }

        private void Initialize() {
            m_nodePoints.Clear();
            List<string> removeNameList = new List<string>();
            foreach (GameObject node in m_nodes) {
                foreach (string targetName in m_targetList) {
                    string name = "Node_" + node.GetInstanceID();
                    if (targetName == name) {
                        removeNameList.Add(name);
                    }
                }
                Destroy(node);
            }
            foreach (string name in removeNameList) {
                m_targetList.Remove(name);
            }
            m_nodes.Clear();
            m_interval = 0;
        }

        void OnDrawGizmos() {
#if UNITY_EDITOR
            if (parent.DrawRoute) {
                Gizmos.color = Color.red;
                var size = 10f;
                if (m_routeList != null) {
                    foreach (GameObject obj in m_routeList) {
                        if (obj != null) {
                            Gizmos.DrawWireCube(obj.transform.position, new Vector3(size, size, size));
                        }
                    }
                }
                Gizmos.color = Color.white;
            }
#endif
        }
    }
}