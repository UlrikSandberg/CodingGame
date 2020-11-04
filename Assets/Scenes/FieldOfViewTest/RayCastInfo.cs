using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayCastInfo
{
    // Did the ray hit anything?
    public bool IsHit { get; private set; }
    // At what point did it hit anything
    public Vector3 InterSectionPoint { get; private set; }
    // distance travelled before hitting anything
    public float Distance { get; private set; }
    // horizontal angle the ray was shot from
    public float AngleHor { get; private set; }
    // vertical angle the ray was shot from
    public float AngleVer { get; private set; }

    public RayCastInfo(bool isHit, Vector3 intersectionPoint, float distance, float angleHor, float angleVer)
    {
        IsHit = isHit;
        InterSectionPoint = intersectionPoint;
        Distance = distance;
        AngleHor = angleHor;
        AngleVer = angleVer;
    }
}
