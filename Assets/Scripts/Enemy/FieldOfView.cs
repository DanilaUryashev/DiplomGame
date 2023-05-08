using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEngine.UI;
using UnityEngine.UIElements;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class FieldOfView : MonoBehaviour
{
    [SerializeField] private LayerMask layerMask;
    private Mesh mesh;
    private float fov;
    private float viewDistance;
    private Vector3 origin;
    private float startingAngle;

    private void Start()
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
        fov = 90f;
        viewDistance = 50f;
        origin = Vector3.zero;
    }

    private void Update()
    {

    }

    private void LateUpdate()
    {
        int rayCount = 50;
        float angle = startingAngle;
        float angleIncrease = fov / rayCount;

        Vector3[] vertices = new Vector3[rayCount + 1 + 1];
        Vector2[] uv = new Vector2[vertices.Length];
        int[] triangles = new int[rayCount * 3];

        vertices[0] = transform.InverseTransformPoint(origin);

        int vertexIndex = 1;
        int triangleIndex = 0;
        for (int i = 0; i <= rayCount; i++)
        {
            Vector3 vertex;
            RaycastHit2D raycastHit2D = Physics2D.Raycast(origin, GetVectorFromAngle(angle), viewDistance, layerMask);
            if (raycastHit2D.collider == null)
            {
                // No hit
                vertex = transform.InverseTransformPoint(origin + GetVectorFromAngle(angle) * viewDistance);
            }
            else
            {
                // Hit object
                vertex = transform.InverseTransformPoint(raycastHit2D.point);
            }
            vertices[vertexIndex] = vertex;

            if (i > 0)
            {
                triangles[triangleIndex + 0] = 0;
                triangles[triangleIndex + 1] = vertexIndex - 1;
                triangles[triangleIndex + 2] = vertexIndex;

                triangleIndex += 3;
            }

            vertexIndex++;
            angle -= angleIncrease;
        }


        mesh.vertices = vertices;
        mesh.uv = uv;
        mesh.triangles = triangles;
        mesh.bounds = new Bounds(origin, Vector3.one * 1000f);
    }

    public void SetOrigin(Vector3 origin)
    {
        this.origin = origin;
    }

    public void SetAimDirection(Vector3 aimDirection)
    {
        startingAngle = GetAngleFromVectorFloat(aimDirection) + fov / 2f;
    }
    public static float GetAngleFromVectorFloat(Vector3 dir)
    {
        dir= dir.normalized;
        float n = Mathf.Atan2(dir.x, dir.y)*Mathf.Rad2Deg;
        if (n < 0)
        {
            n += 360;
        }
        return n;
    }

    public void SetFoV(float fov)
    {
        this.fov = fov;
    }

    public void SetViewDistance(float viewDistance)
    {
        this.viewDistance = viewDistance;
    }
    public static Vector3 GetVectorFromAngle(float angle)
    {
        //angle = 0 -> 360
        float angleRad = angle * (Mathf.PI / 180f);
        return new Vector3(Mathf.Cos(angleRad), Mathf.Sin(angleRad));
    }
}










//// Start is called before the first frame update
//[SerializeField] private float fov;
//[SerializeField] private Vector3 origin;
//[SerializeField] private int rayCount;
//[SerializeField] private float angle;
//[SerializeField] private float angleIncrease;
//[SerializeField] private float viewDistance;
//[SerializeField] private Mesh mesh;
//[SerializeField] private LayerMask layerMask;
//private float startingAngle;
//void Start()
//{
//    mesh = new Mesh();
//    GetComponent<MeshFilter>().mesh = mesh;
//    fov = 90f;
//    viewDistance = 50f;
//    origin = Vector3.zero;

//}


//// Update is called once per frame
//void Update()
//{
//    SetOrigin(transform.position);

//}
//private void LateUpdate()
//{
//    int rayCount = 50;
//    float angle = startingAngle;
//    float angleIncrease = fov / rayCount;

//    Vector3[] vertices = new Vector3[rayCount + 1 + 1];
//    Vector2[] uv = new Vector2[vertices.Length];
//    int[] triangles = new int[rayCount * 3];

//    vertices[0] = transform.InverseTransformPoint(origin);

//    int vertexIndex = 1;
//    int triangleIndex = 0;
//    for (int i = 0; i <= rayCount; i++)
//    {
//        Vector3 vertex;
//        RaycastHit2D raycastHit2D = Physics2D.Raycast(origin, GetVectorFromAngle(angle), viewDistance);
//        if (raycastHit2D.collider == null)
//        {
//            // No hit
//            vertex = transform.InverseTransformPoint(origin + GetVectorFromAngle(angle) * viewDistance);
//        }
//        else
//        {
//            // Hit object
//            vertex = transform.InverseTransformPoint(raycastHit2D.point);
//        }
//        vertices[vertexIndex] = vertex;

//        if (i > 0)
//        {
//            triangles[triangleIndex + 0] = 0;
//            triangles[triangleIndex + 1] = vertexIndex - 1;
//            triangles[triangleIndex + 2] = vertexIndex;

//            triangleIndex += 3;
//        }

//        vertexIndex++;
//        angle -= angleIncrease;
//    }


//    mesh.vertices = vertices;
//    mesh.uv = uv;
//    mesh.triangles = triangles;
//    mesh.bounds = new Bounds(origin, Vector3.one * 1000f);
//}
//public static Vector3 GetVectorFromAngle(float angle)
//{
//    //angle = 0 -> 360
//    float angleRad = angle * (Mathf.PI / 180f);
//    return new Vector3(Mathf.Cos(angleRad), Mathf.Sin(angleRad));
//}
//public void SetOrigin(Vector3 origin)
//{
//    this.origin = origin;
//}
//public void SetDirectionAim()
//{

//}