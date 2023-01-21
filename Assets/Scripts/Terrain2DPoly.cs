using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PolygonCollider2D))]
public class Terrain2DPoly : MonoBehaviour
{
    [SerializeField] private Terrain m_Terrain;
    [SerializeField] private float zPlane = 0f;

    private PolygonCollider2D m_collider;

    void Start()
    {
        m_collider = GetComponent<PolygonCollider2D>();
        Vector3 terrainPos = m_Terrain.GetPosition();

        transform.position = new Vector3(terrainPos.x, terrainPos.y, zPlane);

        m_collider.SetPath(0, IntersectPlane().ToArray());
    }

    /// <summary>
    /// Generates a list of Vector2s representing where the zPlane intersects the terrain.
    /// </summary>
    /// <returns>List of points</returns>
    private List<Vector2> IntersectPlane()
    {
        Vector3 terrainPos = m_Terrain.GetPosition();
        TerrainData terrainData = m_Terrain.terrainData;
        int pointCount = terrainData.heightmapResolution;
        float pointStepSize = terrainData.size.x / pointCount;
        List<Vector2> intersectPoints = new List<Vector2>(pointCount + 2);
        float minY = 0f;

        for (int x = 0; x < pointCount; x++)
        {
            float xPos = x * pointStepSize;
            float y = m_Terrain.SampleHeight(new Vector3(xPos + terrainPos.x, 0, 0));
            minY = Mathf.Min(minY, y);
            intersectPoints.Add(new Vector2(xPos, y));
        }

        minY -= 5f;

        // Add botton right and bottom left points to insure convex.
        intersectPoints.Add(new Vector2(pointCount * pointStepSize, minY));
        intersectPoints.Add(new Vector2(0, minY));

        return intersectPoints;
    }
}