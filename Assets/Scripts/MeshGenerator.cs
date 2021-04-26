using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class MeshGenerator : MonoBehaviour
{
    public float radius = 5f;
    [Range(0, 10)]
    public int iterations = 0;

    private Mesh mesh;
    private List<Vector3> vertices = new List<Vector3>();
    private List<int> triangles = new List<int>();

    void Start()
    {
        mesh = GetComponent<MeshFilter>().mesh;
        GenerateShape();
        CreateMesh();
    }

    void GenerateShape()
    {
        vertices.Add(new Vector3(-radius/2, -radius, 0).normalized * radius);
        vertices.Add(new Vector3(-radius/2, radius, 0).normalized * radius);
        vertices.Add(new Vector3(radius/2, radius, 0).normalized * radius);
        vertices.Add(new Vector3(radius/2, -radius, 0).normalized * radius);

        vertices.Add(new Vector3(-radius, 0, radius/2).normalized * radius);
        vertices.Add(new Vector3(radius, 0, radius/2).normalized * radius);
        vertices.Add(new Vector3(radius, 0, -radius/2).normalized * radius);
        vertices.Add(new Vector3(-radius, 0, -radius/2).normalized * radius);

        vertices.Add(new Vector3(0, -radius/2, -radius).normalized * radius);
        vertices.Add(new Vector3(0, -radius/2, radius).normalized * radius);
        vertices.Add(new Vector3(0, radius/2, radius).normalized * radius);
        vertices.Add(new Vector3(0, radius/2, -radius).normalized * radius);

        /*triangles.Add(0);
        triangles.Add(1);
        triangles.Add(2);
        triangles.Add(2);
        triangles.Add(3);
        triangles.Add(0);

        triangles.Add(4);
        triangles.Add(5);
        triangles.Add(6);
        triangles.Add(6);
        triangles.Add(7);
        triangles.Add(4);

        triangles.Add(8);
        triangles.Add(9);
        triangles.Add(10);
        triangles.Add(10);
        triangles.Add(11);
        triangles.Add(8);*/

        triangles.AddRange(new List<int>() {0, 7, 8});
        triangles.AddRange(new List<int>() {0, 4, 7});
        triangles.AddRange(new List<int>() {0, 9, 4});
        triangles.AddRange(new List<int>() {8, 6, 3});
        triangles.AddRange(new List<int>() {3, 6, 5});
        triangles.AddRange(new List<int>() {3, 5, 9});
        triangles.AddRange(new List<int>() {8, 7, 11});
        triangles.AddRange(new List<int>() {8, 11, 6});
        triangles.AddRange(new List<int>() {9, 5, 10});
        triangles.AddRange(new List<int>() {9, 10, 4});
        triangles.AddRange(new List<int>() {7, 1, 11});
        triangles.AddRange(new List<int>() {11, 2, 6});
        triangles.AddRange(new List<int>() {11, 1, 2});
        triangles.AddRange(new List<int>() {7, 4, 1});
        triangles.AddRange(new List<int>() {4, 10, 1});
        triangles.AddRange(new List<int>() {6, 2, 5});
        triangles.AddRange(new List<int>() {5, 2, 10});
        triangles.AddRange(new List<int>() {10, 2, 1});
        triangles.AddRange(new List<int>() {0, 8, 3});
        triangles.AddRange(new List<int>() {3, 9, 0});

    }

    void CreateMesh()
    {
        // ok so what I should do is separate each triangle into four triangles by adding 3 new vertices, I should remove the earlier triangle from the list
        // the points should be on the edge of imaginary sphere
        for (int iteration = 0; iteration < iterations; iteration++)
        {
            var ti = 0;
            var cycleCount = triangles.Count / 3;
            Debug.Log(vertices.Count);
            for (int i = 0; i < cycleCount; i++)
            {
                var vx1 = Vector3.Lerp(vertices[triangles[ti + 0]], vertices[triangles[ti + 1]], 0.5f).normalized * radius;
                var vx2 = Vector3.Lerp(vertices[triangles[ti + 1]], vertices[triangles[ti + 2]], 0.5f).normalized * radius;
                var vx3 = Vector3.Lerp(vertices[triangles[ti + 2]], vertices[triangles[ti + 0]], 0.5f).normalized * radius;
                vertices.Add(vx1);
                vertices.Add(vx2);
                vertices.Add(vx3);

                var index = vertices.Count - 1;
                // replace with 4 new
                triangles.AddRange(new List<int>() { triangles[ti + 0], index - 2, index });
                triangles.AddRange(new List<int>() { triangles[ti + 1], index - 1, index - 2 });
                triangles.AddRange(new List<int>() { triangles[ti + 2], index, index - 1 });
                triangles.AddRange(new List<int>() { index, index - 2, index - 1 });
                ti += 3;
            }
            triangles.RemoveRange(0, cycleCount * 3);
        }
        Vector3[] v = new Vector3[vertices.Count];
        for(int i = 0; i < vertices.Count; i++)
        {
            v[i] = vertices[i];
        }
        int[] t = new int[triangles.Count];
        for (int i = 0; i < triangles.Count; i++)
        {
            t[i] = triangles[i];
        }
        mesh.Clear();
        mesh.vertices = v;
        mesh.triangles = t;
        mesh.RecalculateNormals();
    }
}
