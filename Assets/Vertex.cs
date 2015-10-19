using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Vertex
{
    public Vector2 Position { get; set; }
    public float Height { get; set; }
    public Vertex[] Neighbours { get; set; }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="position"></param>
    /// <param name="height"></param>
    /// <param name="neighbours">An array of 8 possible neighbours, starting top left going clockwise</param>
    public Vertex(Vector2 position, float height, Vertex[] neighbours)
    {
        this.Position = position;
        this.Height = height;
        this.Neighbours = neighbours;
    }
}