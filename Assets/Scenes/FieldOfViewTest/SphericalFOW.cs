using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphericalFOW : MonoBehaviour
{
    [Range(0, 360)]
    public float viewAngle = 180;
    public float viewRadius = 20;
    public float searchResolutionHor = 0.2f;
    public float searchResolutionVer = 0.2f;
    public bool showFieldOfView = false;

    // Access to obstacles layer
    public LayerMask obstacleMask;
    public LayerMask player;

    [HideInInspector]
    public Transform turretHead;

    [HideInInspector]
    public List<Transform> visibleTargets = new List<Transform>();

    // Start is called before the first frame update
    void Start()
    {
        turretHead = transform.GetChild(1).GetChild(0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void LateUpdate()
    {
        if(showFieldOfView)
        {
            DrawFieldOfView();
        }
    }

    public void FindVisibleTargets()
    {
        visibleTargets.Clear();

        Collider[] targetsInViewRadius = Physics.OverlapSphere(turretHead.position, viewRadius, player);

        for(int i = 0; i < targetsInViewRadius.Length; i++)
        {
            Transform target = targetsInViewRadius[i].transform;

            Vector3 dirToTarget = (target.position - transform.position).normalized;
            if(Vector3.Angle(transform.forward, dirToTarget) < viewAngle)
            {
                float dstToTarget = Vector3.Distance(transform.position, target.position);

                // If raycasy does not hit any obstacles we are good
                if(!Physics.Raycast(transform.position, dirToTarget, dstToTarget, obstacleMask))
                {
                    visibleTargets.Add(target);
                }
            }
        }
    }

    // Shoot out lines in a sphere according to viewAngle
    public void DrawFieldOfView()
    {
        // Find the number of rays to cast with respect to given searchResolution
        int stepCountHor = Mathf.RoundToInt(viewAngle * searchResolutionHor);
        int stepCountVer = Mathf.RoundToInt(viewAngle * searchResolutionVer);
        // Find the respective angular step size
        float stepAngleSizeHor = viewAngle / stepCountHor;
        float stepAngleSizeVer = viewAngle / stepCountVer;

        // Store list of all castedRay's intersection points
        var intersectionPoints = new List<RayCastInfo>();

        // Gather all the respective endpoints for the field of view rayCast
        for(int i = 0; i <= stepCountVer; i++)
        {
            // Take turrets current xAngle and subtract the viewAngle / 2 to start rayCasting from the bottom up
            var angleVer = turretHead.transform.eulerAngles.x - viewAngle / 2 + stepAngleSizeVer * i;
            
            for(int j = 0; j <= stepCountHor; j++)
            {
                // Take turrets current yAngle and subtract the viewAngle / 2 to start rayCasting from the left, add the respective stepSize times index
                var angleHor = turretHead.transform.eulerAngles.y - viewAngle / 2 + stepAngleSizeHor * j;

                // angleIsLocal has been set to false because we have already taken it into account in the above angle.
                intersectionPoints.Add(CastRay(angleHor, angleVer, false));
            }
        }

        // Now draw a line for each rayCasted
        for(int i = 0; i < intersectionPoints.Count; i++)
        {
            Debug.DrawLine(turretHead.position, intersectionPoints[i].InterSectionPoint);
        }
    }

    public RayCastInfo CastRay(float angleHor, float angleVer, bool angleIsLocal)
    {
        var direction = DirectionVectorFromAngle(angleHor, angleVer, angleIsLocal);
//        Vector3 direction = transform.TransformDirection(0, angleHor, angleVer);
        //Quaternion r = (transform.rotation * Quaternion.Euler(new Vector3(angleVer, angleHor, 0)));
        //Vector3 direction = r * Vector3.forward;
                            
        // Variable to store rayCast information
        RaycastHit rayCastHit;

        //if the ray cast hits a target
        if(Physics.Raycast(turretHead.position, direction, out rayCastHit, viewRadius, obstacleMask))
        {
            // The ray hit something.
            return new RayCastInfo(true, rayCastHit.point, rayCastHit.distance, angleHor, angleVer);
        }
        // The ray didn't hit anything
        return new RayCastInfo(false, turretHead.position + direction * viewRadius, viewRadius, angleHor, angleVer);
    }

    public Vector3 DirectionVectorFromAngle(float angleInDegreeHor, float angleInDegreeVer, bool angleIsLocal)
    {
        // With respect to the unit circle Sin(V) = Y, Cos(V) = X
        // However in unity the circle is rotated 90 degress, and seeing as Sin = Cos(V-90) and Cos = Sin(v-90) we swap the trig function in unity
        // so Sin(V) = X, Cos(V) = Y. Also because Mathf.Sin takes radians we have to convert our degrees to radians

        // If angleIsLocal then we should take into account the orientation of the respective object
        if (angleIsLocal)
        {
            angleInDegreeHor += turretHead.transform.eulerAngles.y;
            angleInDegreeVer += turretHead.transform.eulerAngles.x;
        }

        var vec = new Vector3(Mathf.Sin(angleInDegreeHor * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegreeHor * Mathf.Deg2Rad));
        //vec = vec.RotateX(angleInDegreeVer);

        return vec;
    }
}
