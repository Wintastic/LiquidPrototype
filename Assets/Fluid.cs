using System;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using Random = UnityEngine.Random;

public class Fluid : MonoBehaviour
{
    private Vertex[] vertices;
    private const int framesPerUpdate = 6;
    private int timer = 0;

    private Mesh mesh;

    // Use this for initialization
    private void Start()
    {
        CreateMesh(250, 250);
        var material = Resources.Load("Materials/Water") as Material;
        gameObject.GetComponent<MeshRenderer>().material = material;
        gameObject.transform.rotation = Quaternion.Euler(new Vector3(-90, 0, 0));
        gameObject.transform.position = new Vector3(-4, 0, 4);

        mesh = GetComponent<MeshFilter>().mesh;
    }

    private void CreateMesh(int width, int length)
    {
        var meshFilter = gameObject.AddComponent<MeshFilter>();
        var mesh = meshFilter.mesh;

        vertices = new Vertex[width * length];
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < length; j++)
            {
                vertices[i*width + j] = new Vertex(new Vector3(i, j), null);
            }
        }
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < length; j++)
            {
                var neighbours = new Vertex[8];
                if (i > 0 && j > 0) neighbours[0] = vertices[(i - 1) * width + (j - 1)];
                if (j > 0) neighbours[1] = vertices[(i) * width + (j - 1)];
                if (i < width - 1 && j > 0) neighbours[2] = vertices[(i + 1) * width + (j - 1)];
                if (i < width - 1) neighbours[3] = vertices[(i + 1) * width + j];
                if (i < width - 1 && j < length - 1) neighbours[4] = vertices[(i + 1) * width + (j + 1)];
                if (j < length - 1) neighbours[5] = vertices[(i) * width + (j + 1)];
                if (i > 0 && j < length - 1) neighbours[6] = vertices[(i - 1) * width + (j + 1)];
                if (i > 0) neighbours[7] = vertices[(i - 1) * width + j];
                vertices[i*width + j].Neighbours = neighbours;
            }
        }

        var meshVertices = new List<Vector3>();
        var triangles = new List<int>();

        meshVertices = vertices.Select(v => new Vector3(v.Position.x, v.Position.y, v.Position.z)).ToList();

        foreach (var vertex in vertices)
        {
            if (vertex.Position.x < width - 1 && vertex.Position.y < length - 1)
            {
                triangles.Add((int) (vertex.Position.x * width + vertex.Position.y));
                triangles.Add((int)(vertex.Neighbours[3].Position.x * width + vertex.Neighbours[3].Position.y));
                triangles.Add((int)(vertex.Neighbours[4].Position.x * width + vertex.Neighbours[4].Position.y));
                triangles.Add((int)(vertex.Position.x * width + vertex.Position.y));
                triangles.Add((int)(vertex.Neighbours[4].Position.x * width + vertex.Neighbours[4].Position.y));
                triangles.Add((int)(vertex.Neighbours[5].Position.x * width + vertex.Neighbours[5].Position.y));
            }
        }

        mesh.Clear();
        mesh.vertices = meshVertices.ToArray();
        mesh.triangles = triangles.ToArray();

        mesh.RecalculateNormals();
        mesh.Optimize();
        gameObject.AddComponent<MeshRenderer>();
    }

    // Update is called once per frame
    private void Update()
    {
        timer++;
        if (timer > framesPerUpdate)
        {
            timer = 0;
            UpdateFluid();
        }
    }

    private void UpdateFluid()
    {
        foreach (var vertex in vertices)
        {
            if (Random.value > 0.8f)
            {
                vertex.Position.z += Random.value - 0.5f;
                vertex.Position.z = vertex.Position.z > 1 ? 1 : vertex.Position.z < 0 ? 0 : vertex.Position.z;
            }
        }
        mesh.vertices = vertices.Select(v => v.Position).ToArray();
    }
}