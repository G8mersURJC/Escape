using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Conection
{
    private Room rRoom;
    private List<Vector2Int> lv2iCells;
    private List<Vector2Int> lv2iEndPoints;

    public Conection(Room r)
    {
        this.rRoom = r;
        this.lv2iCells = new List<Vector2Int>();
        this.lv2iEndPoints = new List<Vector2Int>();
    }

    public Conection(Room r, List<Vector2Int> cells)
    {
        this.rRoom = r;
        this.lv2iCells = cells;
        this.lv2iEndPoints = new List<Vector2Int>();
    }

    public Conection(Room r, List<Vector2Int> cells, List<Vector2Int> endPoints)
    {
        this.rRoom = r;
        this.lv2iCells = cells;
        this.lv2iEndPoints = endPoints;
    }

    public void AddCells(List<Vector2Int> pathCells)
    {
        foreach(Vector2Int n in pathCells)
        {
            lv2iCells.Add(n);
        }
    }

    public List<Vector2Int> GetCells()
    {
        return lv2iCells;
    }

    public Room GetRoom()
    {
        return rRoom;
    }

    public void SetEndPoints(Vector2Int a, Vector2Int b)
    {
        this.lv2iEndPoints.Clear();
        this.lv2iEndPoints.Add(a);
        this.lv2iEndPoints.Add(b);
    }

    public List<Vector2Int> GetEndPoints()
    {
        return lv2iEndPoints;
    }

    public static bool ContainsPosition(Conection c, Vector2Int position)
    {
        foreach (Vector2Int p in c.GetCells())
        {
            if (p.x == position.x && p.y == position.y)
            {
                return true;
            }
        }

        return false;
    }

    public static bool ContainsPosition(Conection c, int x, int y)
    {
        foreach (Vector2Int p in c.GetCells())
        {
            if (p.x == x && p.y == y)
            {
                return true;
            }
        }

        return false;
    }
}
