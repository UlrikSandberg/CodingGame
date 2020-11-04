using UnityEngine;
using UnityEditor;


[CustomEditor(typeof(SphericalFOW))]
public class SphericalFieldOfViewEditor : Editor
{
    private void OnSceneGUI()
    {
        SphericalFOW sFow = (SphericalFOW)target;
        Handles.color = Color.white;

    }
}
