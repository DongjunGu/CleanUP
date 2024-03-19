using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomMesh : MonoBehaviour
{
    public MeshFilter meshFilter;
    // Start is called before the first frame update
    void Start()
    {
        Mesh mesh = new Mesh();

        Vector3[] verts = new Vector3[3];
        verts[0] = new Vector3(0, 1, 0);
        verts[1] = new Vector3(-1, 0, 0);
        verts[2] = new Vector3(1, 0, 0);
        mesh.vertices = verts;

        int[] indices = new int[3] {0,1,2};
        mesh.triangles = indices;


        meshFilter.mesh = mesh;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
