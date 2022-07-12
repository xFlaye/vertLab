using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridGenerator : MonoBehaviour
{
    // Based on 3D Procedural Terrain Generation from Zenva course

    [Header("Dimensions")]
    [Tooltip("Size of the plane along the X-axis")]
    public float xSize;
    [Tooltip("Size of the plane along the Z-axis")]
    public float zSize;
    [Tooltip("Number of of X-subidvisions")]
    public int xSubdivs;
    [Tooltip("Number of Z-subdivisions")]
    public int zSubdivs;

    [Header("Material")]
    public Material material;

    private MeshRenderer meshRenderer;
    private MeshFilter meshFilter;
    private MeshCollider meshCollider;

    void Awake()
    {
        GenenerateNewGrid();
    }
    
    public void GenenerateNewGrid()
    {
        // add the required components to create the grid
        meshRenderer = gameObject.AddComponent<MeshRenderer>();
        meshRenderer.material = material;

        meshFilter = gameObject.AddComponent<MeshFilter>();
        meshCollider = gameObject.AddComponent<MeshCollider>();

        // create a new mesh

        Mesh mesh = new Mesh();
        meshFilter.mesh = mesh;

        // create vertices and uv array
        Vector3[] vertices = new Vector3[(xSubdivs + 1) * (zSubdivs + 1)];
        Vector2[] uv = new Vector2[vertices.Length];

        // calculate the length of each edge
        float xSubLength = xSize / (float)xSubdivs;
        float zSubLength = zSize / (float)zSubdivs;

        // create vertices and uvs on grid
        for (int i = 0, z =0; z <= zSubdivs; z++)
        {
            for (int x = 0; x <= zSubdivs; x++, i++)
            {
                vertices[i] = new Vector3(x * xSubLength, 0 , z * zSubLength);
                uv[i] = new Vector2((float)x / xSubdivs, (float)z / zSubdivs);
            }
        }

        // set vertices and uvs
        mesh.vertices = vertices;
        mesh.uv = uv;

        // create the triangles
        int[] tris = new int[xSubdivs * zSubdivs * 6];

        for (int ti = 0, vi = 0, y = 0; y < zSubdivs; y++, vi++)
        {
            for (int x = 0; x < xSubdivs; x++, ti += 6, vi++)
            {
                tris[ti] = vi;
                tris[ti + 3] = tris [ti + 2] = vi + 1;
                tris[ti + 4] = tris[ti + 1] = vi + xSubdivs + 1;
                tris[ti + 5] = vi + xSubdivs + 2;
            }
        }

        // set the triangles and recalculate the normals
        mesh.triangles = tris;
        mesh.RecalculateNormals();
    }


}


