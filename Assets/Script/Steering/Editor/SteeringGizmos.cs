using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public static class SteeringGizmos  {

    [DrawGizmo(GizmoType.Active | GizmoType.Selected)]
    static void DrawNormalGizmos(Steering steering, GizmoType drawnGizmoType)
    {
        Vector3 position = steering.transform.position;
        Vector3 velocity = steering.GetComponent<Rigidbody>().velocity;
        Vector3 steeringForce = steering.giz.steeringForce;

        if (steering.giz.Velocity)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(position, position + velocity);
        }
        if (steering.giz.Steering)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(position, position + steeringForce);
        }
        if (steering.giz.Separate)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(position, position + steering.giz.separateForce);
        }
        if (steering.giz.SeparateCheckSphere)
        {
            Gizmos.color = new Color(0,1,0,0.5f);

            Gizmos.DrawSphere(position, steering.giz.separateSphereLenght);
        }
        

    }
}
//public static class NormalGizmosDrawer
//{

//    [DrawGizmo(GizmoType.Active | GizmoType.Selected)]
//    static void DrawNormalGizmos(NormalDrawer nd, GizmoType drawnGizmoType)
//    {
//        Mesh m = nd.GetComponent<MeshFilter>().sharedMesh;
//        Vector3[] vertices = m.vertices;

//        Gizmos.matrix = nd.transform.localToWorldMatrix;

//        // Affichage des normales des faces
//        if (nd.ShowFaceNormals)
//        {
//            Gizmos.color = Color.cyan;
//            int[] triangles = m.triangles;
//            for (int i = 0; i < triangles.Length; i += 3)
//            {
//                Vector3 center = vertices[triangles[i]] + vertices[triangles[i + 1]] + vertices[triangles[i + 2]];
//                center /= 3.0f;

//                Vector3 abVector = vertices[triangles[i + 1]] - vertices[triangles[i]];
//                Vector3 acVector = vertices[triangles[i + 2]] - vertices[triangles[i]];
//                Vector3 normal = Vector3.Cross(abVector, acVector).normalized;

//                Gizmos.DrawLine(center, center + normal);
//            }
//        }

//        // Affichage des normals des sommets
//        if (nd.ShowVertexNormals)
//        {
//            Gizmos.color = Color.yellow;
//            Vector3[] normals = m.normals;
//            for (int i = 0; i < vertices.Length; i++)
//                Gizmos.DrawLine(vertices[i], vertices[i] + normals[i] * 0.25f);
//        }
//    }

//}
