using System;
using UnityEngine;

public static class MatrixMath
{
    public static Vector3 RotateX(this Vector3 vector, float angle)
    {
        var x = vector.x;
        var y = vector.y * Mathf.Cos(angle * Mathf.Deg2Rad) + (vector.y * -Mathf.Sin(angle * Mathf.Deg2Rad));
        var z = vector.z * Mathf.Sin(angle * Mathf.Deg2Rad) + vector.z * Mathf.Cos(angle * Mathf.Deg2Rad);

        return new Vector3(
            vector.x,
            vector.y * Mathf.Cos(angle * Mathf.Deg2Rad) + (vector.y * -Mathf.Sin(angle * Mathf.Deg2Rad)),
            vector.z * Mathf.Sin(angle * Mathf.Deg2Rad) + vector.z * Mathf.Cos(angle * Mathf.Deg2Rad));
    }
}
