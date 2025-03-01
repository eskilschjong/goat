using UnityEngine;

public class DynamicNetMesh : MonoBehaviour
{
    public Transform[] circlePoints; // Assign 8 points (4 left, 4 right in order)
    public Material netMaterial;     // Assign a material with your net texture
    private Mesh mesh;
    private Vector3[] vertices;

    void Start()
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
        GetComponent<MeshRenderer>().material = netMaterial;
        InitializeMesh();
    }

    void InitializeMesh()
    {
        // Vertices (8 points: 4 left, 4 right)
        vertices = new Vector3[circlePoints.Length];
        for (int i = 0; i < circlePoints.Length; i++)
        {
            vertices[i] = transform.InverseTransformPoint(circlePoints[i].position);
        }

        // Triangles for full net grid (vertical and horizontal connections)
        int[] triangles = {
            // Vertical strips
            0,4,1, 4,5,1,    // Leftmost vertical strip
            1,5,2, 5,6,2,    // Middle vertical strip
            2,6,3, 6,7,3,    // Rightmost vertical strip

            // Horizontal connections
            0,1,4, 1,5,4,    // Bottom horizontal
            1,2,5, 2,6,5,    // Middle horizontal
            2,3,6, 3,7,6     // Top horizontal
        };

        mesh.vertices = vertices;
        mesh.triangles = triangles;
        InitializeUVs();
        mesh.RecalculateNormals();
        mesh.RecalculateBounds();
    }

    void InitializeUVs()
{
    Vector2[] uvs = new Vector2[vertices.Length];
    
    // Assuming 4 vertical points per side (0-3 = left, 4-7 = right)
    for (int i = 0; i < uvs.Length; i++)
    {
        // Determine if it's a left or right point
        bool isLeftSide = (i < 4);
        
        // Horizontal UV: 0 for left, 1 for right
        float u = isLeftSide ? 0f : 1f;
        
        // Vertical UV: 0 (bottom) to 1 (top) based on vertical position
        float v = (float)(i % 4) / 3f; // 4 vertical points = 3 segments
        
        uvs[i] = new Vector2(u, v);
    }
    
    mesh.uv = uvs;
}

    void LateUpdate()
    {
        // Update vertices from physics points
        for (int i = 0; i < circlePoints.Length; i++)
        {
            vertices[i] = transform.InverseTransformPoint(circlePoints[i].position);
        }
        mesh.vertices = vertices;
        mesh.RecalculateNormals();
        mesh.RecalculateBounds();
    }
}