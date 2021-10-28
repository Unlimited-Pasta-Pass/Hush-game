using System.Collections;
using System.Collections.Generic;
using FieldOfView.Models;
using UnityEngine;

public class FieldOfViewController : MonoBehaviour {

    [Header("Field of View Settings")]
    [Tooltip("Radius or max distance the 'player' can see")] 
    [SerializeField] private float viewRadius = 50f;
    [Range(0, 360), Tooltip("Wideness of the field of view")] 
    [SerializeField] private float viewAngle = 90f;
    
    [Header("Edge Resolving Settings")]
    [Tooltip("Iterations of the edge resolving algorithm (higher = more precise but also more costly)")] 
    [SerializeField] private int edgeResolveIterations = 1;
    [SerializeField] private float edgeDstThreshold;

    [Header("General Settings")]
    [Range(0, 1), Tooltip("Delay between field of view updates")] 
    [SerializeField] private float delayBetweenFOVUpdates = 0.2f;

    [Header("Layermask Settings")]
    [Tooltip("Objects that are effected when entering/exiting the fov. These have a Hideable Object Controller attached.")] 
    [SerializeField] private LayerMask targetMask;
    [ Tooltip("Objects that block the field of view")] 
    [SerializeField] private LayerMask obstacleMask;

    [Header("Resolution Settings")]
    [Tooltip("Affects the amount of rays casted out when recalculating the fov. Raycast count = viewAngle * meshResolution")] 
    [SerializeField] private int meshResolution = 10;
    
    [Space(10)]
    [SerializeField] private DebugParameters debug;
    
    //variable is used in the DrawFieldOfView method (storing it here it way more efficient - GC.collect...)
    private List<Vector3> viewPoints = new List<Vector3>();

    private void Start() {
        debug.viewMesh = new Mesh { name = "FOV Mesh" };
        debug.viewMeshFilter.mesh = debug.viewMesh;
    }
    
    private void OnEnable() {
        StartCoroutine(FindTargetsCoroutine(delayBetweenFOVUpdates));
    }

    private void LateUpdate() {
        if (debug.visualizeFieldOfView) {
            debug.viewMeshFilter.mesh = debug.viewMesh;
            DrawFieldOfView();
        } else {
            debug.viewMeshFilter.mesh = null;
        }
    }

    private void DrawFieldOfView() {
        viewPoints.Clear();
        ViewCastInfo oldViewCast = new ViewCastInfo();

        /* Calculate normal field of view */
        for (int i = 0; i <= Mathf.RoundToInt(viewAngle * meshResolution); i++) {
            //float angle = transform.eulerAngles.y - viewAngle / 2 + (viewAngle / Mathf.RoundToInt(viewAngle * meshResolution)) * i;
            ViewCastInfo newViewCast = ViewCast(transform.eulerAngles.y - viewAngle / 2 + (viewAngle / Mathf.RoundToInt(viewAngle * meshResolution)) * i, viewRadius);

            if (i > 0) {
                if (oldViewCast.hit != newViewCast.hit || (oldViewCast.hit && newViewCast.hit && Mathf.Abs(oldViewCast.distance - newViewCast.distance) > edgeDstThreshold)) {
                    EdgeInfo edge = FindEdge(oldViewCast, newViewCast, viewRadius);
                    if (edge.pointA != Vector3.zero) {
                        viewPoints.Add(edge.pointA);
                    }
                    if (edge.pointB != Vector3.zero) {
                        viewPoints.Add(edge.pointB);
                    }
                }
            }

            viewPoints.Add(newViewCast.point);
            oldViewCast = newViewCast;
        }

        /* Draw mesh */
        int vertexCount = viewPoints.Count + 1;
        Vector3[] vertices = new Vector3[vertexCount];
        int[] triangles = new int[(vertexCount - 2) * 3];

        vertices[0] = Vector3.zero;
        for (int i = 0; i < vertexCount - 1; i++) {
            vertices[i + 1] = transform.InverseTransformPoint(viewPoints[i]);

            if (i < vertexCount - 2) {
                triangles[i * 3] = 0;
                triangles[i * 3 + 1] = i + 1;
                triangles[i * 3 + 2] = i + 2;
            }
        }

        debug.viewMesh.Clear();

        debug.viewMesh.vertices = vertices;
        debug.viewMesh.triangles = triangles;
        debug.viewMesh.RecalculateNormals();
    }

    // Cast out a ray at a given angle and return a ViewCastInfo struct as a result.
    private ViewCastInfo ViewCast(float globalAngle, float viewRadius) {
        Vector3 dir = DirFromAngle(globalAngle, true);

        Physics.autoSyncTransforms = false;

        Debug.DrawRay(transform.position, dir * viewRadius, Color.red);

        if (Physics.Raycast(transform.position, dir, out var hit, viewRadius, obstacleMask)) {
            Physics.autoSyncTransforms = true;
            return new ViewCastInfo()
            {
                hit = true, 
                point = hit.point, 
                distance = hit.distance, 
                angle = globalAngle
            };
        }

        Physics.autoSyncTransforms = true;
        return new ViewCastInfo
        {
            hit = false, 
            point = transform.position + dir * viewRadius, 
            distance = viewRadius, 
            angle = globalAngle
        };
    }
    
    // Finds the edge of a collider
    private EdgeInfo FindEdge(ViewCastInfo minViewCast, ViewCastInfo maxViewCast, float viewRadius) {
        float minAngle = minViewCast.angle;
        float maxAngle = maxViewCast.angle;
        Vector3 minPoint = Vector3.zero;
        Vector3 maxPoint = Vector3.zero;

        for (int i = 0; i < edgeResolveIterations; i++) {
            float angle = (minAngle + maxAngle) / 2;
            ViewCastInfo newViewCast = ViewCast(angle, viewRadius);

            bool edgeDstThresholdExceeded = Mathf.Abs(minViewCast.distance - newViewCast.distance) > edgeDstThreshold;
            if (newViewCast.hit == minViewCast.hit && !edgeDstThresholdExceeded) {
                minAngle = angle;
                minPoint = newViewCast.point;
            } else {
                maxAngle = angle;
                maxPoint = newViewCast.point;
            }
        }

        return new EdgeInfo() { pointA = minPoint, pointB = maxPoint };
    }
    
    // Finds all visible targets and adds them to the visibleTargets list.
    private void FindVisibleTargets() {
        Collider[] targetsInViewRadius = Physics.OverlapSphere(transform.position, viewRadius, targetMask);

        Physics.autoSyncTransforms = false;

        /* check normal field of view */
        for (int i = 0; i < targetsInViewRadius.Length; i++) {
            Transform target = targetsInViewRadius[i].transform;
            bool isInFOV = false;

            //check if hideable should be hidden or not
            Vector3 dirToTarget = (target.position - transform.position).normalized;
            if (Vector3.Angle(transform.forward, dirToTarget) < viewAngle / 2) {
                float dstToTarget = Vector3.Distance(transform.position, target.position);
                if (!Physics.Raycast(transform.position, dirToTarget, dstToTarget, obstacleMask)) {
                    isInFOV = true;
                }
            }

            //apply effect to IHideable
            HideableObjectController hideable = target.GetComponent<HideableObjectController>();
            if (hideable != null) {
                if (isInFOV) {
                    target.GetComponent<HideableObjectController>().OnFOVEnter();
                } else {
                    target.GetComponent<HideableObjectController>().OnFOVLeave();
                }
            }
        }

        Physics.autoSyncTransforms = true;
    }

    // Convert an angle to a direction vector.
    private Vector3 DirFromAngle(float angleInDegrees, bool IsAngleGlobal) {
        if (!IsAngleGlobal) {
            angleInDegrees += transform.eulerAngles.y;
        }
        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
    }

    // Run the find visible targets method every x seconds/ms
    private IEnumerator FindTargetsCoroutine(float delay) {
        while (true) {
            FindVisibleTargets();
            yield return new WaitForSeconds(delay);
        }
    }
}
