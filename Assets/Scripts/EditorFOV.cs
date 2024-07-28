using System.Numerics;
using UnityEditor;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

[CustomEditor(typeof(FieldofView))]
public class EditorFOV : Editor
{
    private void OnSceneGUI()
    {
        FieldofView fov = (FieldofView)target;
        Handles.color = Color.black;
        Handles.DrawWireArc(fov.objectCenter, Vector3.up, Vector3.forward, 360, fov.radius );
        Vector3 viewAngle01 = DirectionFromAngle(fov.transform.eulerAngles.y, -fov.angle / 2);
        Vector3 viewAngle02 = DirectionFromAngle(fov.transform.eulerAngles.y, fov.angle / 2);
        
        Handles.color = Color.green;
        Handles.DrawLine(fov. objectCenter, fov.objectCenter + viewAngle01 * fov.radius);
        Handles.DrawLine(fov. objectCenter, fov.objectCenter + viewAngle02 * fov.radius);

        if (fov.canSeePlayer)
        {
            Handles.color = Color.red;
            Handles.DrawLine(fov.objectCenter,
                fov.playerRef.transform.position + (0.35f * new Vector3(0, 3.3f, 0)));
        }
    }
    

    private Vector3 DirectionFromAngle(float eulerY, float angleInDegrees)
    {
        angleInDegrees += eulerY;
        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Rad2Deg));
    }
}
