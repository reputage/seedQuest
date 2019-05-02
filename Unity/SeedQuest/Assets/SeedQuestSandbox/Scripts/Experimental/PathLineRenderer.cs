using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class PathLineRenderer : MonoBehaviour {

    public float lineWidth = 0.5f;
    public Vector2[] path = null;
    public bool debug = false;

    private int xSize;
    private int ySize = 1;
    private Mesh mesh;
    private Vector3[] vertices = null;
    private Vector2[] uv = null;
    private Vector4[] tangents = null;
    private int[] triangles = null;

    /* OnStart, generate mesh and set vertices based on path vector array */
    public void Start() {
        if(path.Length == 0)
            setDefaultPath();
        
        createLineMesh();
        setLineVertices();
    } 

    /* Create mesh for PathLine */
    public void createLineMesh() {
        
        mesh = GetComponent<MeshFilter>().mesh = new Mesh();
        mesh.name = "LineMesh";

        xSize = path.Length - 1; // Number of x segments
        vertices = new Vector3[(xSize+1) * (ySize+1)];
        uv = new Vector2[vertices.Length];
        tangents = new Vector4[vertices.Length];
        Vector4 tangent = new Vector4(1f, 0f, 0f, -1f);

        for (int y = 0, i = 0; y <= ySize; y++) {
            for (int x = 0; x <= xSize; x++, i++) {
                vertices[i] = new Vector3(x, 0, y); // Align mesh along the x-z plane
                uv[i] = new Vector2((float)x / xSize, (float)y / ySize);
                tangents[i] = tangent;
            }
        }

        mesh.vertices = vertices;
        mesh.uv = uv;
        mesh.tangents = tangents;

        triangles = new int[xSize * ySize * 6];
        for (int ti = 0, vi = 0, y = 0; y < ySize; y++, vi++) {
            for (int x = 0; x < xSize; x++, ti += 6, vi++) {
                triangles[ti] = vi;
                triangles[ti + 3] = triangles[ti + 2] = vi + 1;
                triangles[ti + 4] = triangles[ti + 1] = vi + xSize + 1;
                triangles[ti + 5] = vi + xSize + 2;
            }
        }

        mesh.triangles = triangles;
        mesh.RecalculateNormals();
    }

    /* Set vertices based on path vector array */
    private void setLineVertices() {

        Vector2[] lineEdges = new Vector2[path.Length * 2]; 
        for (int i = 0; i < path.Length - 1; i++) {
            Vector2 vec = path[i + 1] - path[i];
            Vector2 pVec = new Vector2(-vec.y, vec.x);
            pVec.Normalize();

            lineEdges[i] = path[i] - pVec * lineWidth / 2;
            lineEdges[i + path.Length] = path[i] + pVec * lineWidth / 2;
        } 

        Vector2 v = path[path.Length - 1] - path[path.Length - 2];
        Vector2 pt = new Vector2(-v.y, v.x);
        pt.Normalize();

        lineEdges[path.Length - 1]  = path[path.Length - 1] - pt * lineWidth / 2;
        lineEdges[lineEdges.Length - 1] = path[path.Length - 1] + pt * lineWidth / 2;

        for (int i = 0; i < lineEdges.Length; i++)
            vertices[i] = new Vector3(lineEdges[i].x, 0, lineEdges[i].y);
        
        mesh.vertices = vertices;
        mesh.RecalculateNormals();
    } 

    /* Sets default path for debugging/testings */
    private void setDefaultPath()
    { 
        int size = 20;
        float xMin = -5.0f;
        float xMax = 5.0f;
        float dx = (xMax - xMin) / (float)size;
        float yMin = -5.0f;
        float yMax = 5.0f;
        string debugStr = "";

        path = new Vector2[size];
        for (int i = 0; i < size; i++) {
            float x = xMin + dx * (float)i;
            float y = 4.0f * Mathf.Sin(x); //(yMax - yMin) * Random.value + yMin;
            path[i] = new Vector2(x, y);
            debugStr += "(" + x + "," + y + ") ";
        }

        if(debug)
            Debug.Log(debugStr);
    }

    /* Debug Gizmos */
    private void OnDrawGizmos()
    {
        if (debug && path != null && vertices != null) {
         
            Gizmos.color = Color.white;
            for (int i = 0; i < vertices.Length / 2; i++) {
                Gizmos.DrawSphere(vertices[i], 0.1f);
            }

            Gizmos.color = Color.black;
            for (int i = vertices.Length / 2; i < vertices.Length; i++) {
                Gizmos.DrawSphere(vertices[i], 0.1f);
            }

            Gizmos.color = Color.red;
            for (int i = 0; i < path.Length; i++) {
                Gizmos.DrawWireSphere(new Vector3(path[i].x, 0, path[i].y), 0.2f);
            }
        }
    }
}
