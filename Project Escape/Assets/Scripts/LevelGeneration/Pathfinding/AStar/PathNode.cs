using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PathNode
{

    private Room original;
    private Vector2Int indexPosition;   //Posición del nodo en el tablero
    private PathNode parent;            
    private float f, g, h;

    public PathNode(Room original, Vector2Int pos)
    {
        this.original = original;
        indexPosition = pos;

        f = 0;
        g = 0;
        h = 0;
    }

    public PathNode(Room original, Vector2Int pos, PathNode p)
    {
        this.original = original;
        indexPosition = pos;

        f = 0;
        g = 0;
        h = 0;

        parent = p;
    }

    public Vector2Int getIndexPosition()
    {
        return indexPosition;
    }

    public List<PathNode> getNeighborNodes()
    {
        List<PathNode> lista = new List<PathNode>();

        for (int i = 0; i < original.GetConections().Count; i++)
        {
            if (original.GetConections()[i] == null)
            {
                continue;
            }

            for(int j = 0; j < original.GetConections()[i].GetRooms().Count; j++)
            {
                if(original.GetConections()[i].GetRooms()[j] != original)
                {
                    Room r = original.GetConections()[i].GetRooms()[j];
                    lista.Add(new PathNode(r, r.GetIndexPosition(), this));
                }
            }
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
