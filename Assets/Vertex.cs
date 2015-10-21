using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Vertex
{
    public Vector3 Position;
    public float Velocity;
    public Vertex[] Neighbours;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="position"></param>
    /// <param name="height"></param>
    /// <param name="neighbours">An array of 8 possible neighbours, starting top left going clockwise</param>
    public Vertex(Vector3 position, Vertex[] neighbours)
    {
        this.Position = position;
        this.Velocity = 0;
        this.Neighbours = neighbours;
    }
}