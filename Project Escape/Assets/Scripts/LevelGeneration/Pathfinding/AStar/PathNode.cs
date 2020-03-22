using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PathNode
{

    private Room original;
    private PathNode parent;            
    private float f, g, h;

    public PathNode(Room original)
    {
        this.original = original;

        f = 0;
        g = 0;
        h = 0;
    }

    public PathNode(Room original, PathNode p)
    {
        this.original = original;

        f = 0;
        g = 0;
        h = 0;

        parent = p;
    }

    public Vector2Int getIndexPosition()
    {
        return original.GetIndexPosition();
    }

    public List<PathNode> getNeighborNodes()
    {
        List<PathNode> lista = new List<PathNode>();

        foreach(Conection c in original.GetConections())
        {
            lista.Add(new PathNode(c.GetRoom(), this));
        }

        return lista;
    }

    public Room getOriginal()
    {
        return original;
    }

    public PathNode getParent()
    {
        return parent;
    }

    public float getF()
    {
        return f;
    }

    public void setF(float f)
    {
        this.f = f;
    }

    public float getG()
    {
        return g;
    }

    public void setG(float g)
    {
        this.g = g;
    }

    public float getH()
    {
        return h;
    }

    public void setH(float h)
    {
        this.h = h;
    }
}
