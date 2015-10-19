using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class Fluid : MonoBehaviour
{
    private List<Vertex> vertices;

    // Use this for initialization
    private void Start()
    {
        CreateMesh(100, 100);
        var material = Resources.Load("Materials/Water") as Material;
        gameObject.GetComponent<MeshRenderer>().material = material;
        gameObject.transform.rotation = Quaternion.Euler(new Vector3(-90, 0, 0));
        gameObject.transform.position = new Vector3(-4, 0, 4);
    }

    private void CreateMesh(int width, int length)
    {
        var meshFilter = gameObject.AddComponent<MeshFilter>();
        var mesh = meshFilter.mesh;

        vertices = new List<Vertex>();
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < length; j++)
            {
                vertices.Add(new Vertex(new Vector2(i, j), Random.value, null));
            }
        }
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < length; j++)
            {
                var neighbours = new Vertex[8];
                if (i > 0 && j > 0) neighbours[0] = vertices.Find(x => x.Position == new Vector2(i - 1, j - 1));
                if (j > 0) neighbours[1] = vertices.Find(x => x.Position == new Vector2(i, j - 1));
                if (i < width - 1 && j > 0) neighbours[2] = vertices.Find(x => x.Position == new Vector2(i + 1, j - 1));
                if (i < width - 1) neighbours[3] = vertices.Find(x => x.Position == new Vector2(i + 1, j));
                if (i < width - 1 && j < length - 1) neighbours[4] = vertices.Find(x => x.Position == new Vector2(i + 1, j + 1));
                if (j < length - 1) neighbours[5] = vertices.Find(x => x.Position == new Vector2(i, j + 1));
                if (i > 0 && j < length - 1) neighbours[6] = vertices.Find(x => x.Position == new Vector2(i - 1, j + 1));
                if (i > 0) neighbours[7] = vertices.Find(x => x.Position == new Vector2(i - 1, j));
                vertices.ElementAt(i*width + j).Neighbours = neighbours;
            }
        }

        var meshVertices = new List<Vector3>();
        var triangles = new List<int>();

        meshVertices = vertices.Select(v => new Vector3(v.Position.x, v.Position.y, v.Height)).ToList();

        foreach (var vertex in vertices)
        {
            if (vertex.Position.x < width - 1 && vertex.Position.y < length - 1)
            {
                triangles.Add(vertices.IndexOf(vertex));
                triangles.Add(vertices.IndexOf(vertex.Neighbours[3]));
                triangles.Add(vertices.IndexOf(vertex.Neighbours[4]));
                triangles.Add(vertices.IndexOf(vertex));
                triangles.Add(vertices.IndexOf(vertex.Neighbours[4]));
                triangles.Add(vertices.IndexOf(vertex.Neighbours[5]));
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
    }
}