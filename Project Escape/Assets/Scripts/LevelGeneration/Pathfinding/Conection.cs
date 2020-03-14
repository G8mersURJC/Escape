using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Conection
{
    private List<Room> lrRooms;
    private List<Vector2Int> lv2iCells;
    private List<Vector2Int> lv2iEndPoints;

    public Conection(Room a, Room b)
    {
        this.lrRooms = new List<Room>();
        lrRooms.Add(a);
        lrRooms.Add(b);
        this.lv2iCells = new List<Vector2Int>();
        this.lv2iEndPoints = new List<Vector2Int>();
    }

    public Conection(Room a, Room b, List<Vector2Int> cells)
    {
        this.lrRooms = new List<Room>();
        lrRooms.Add(a);
        lrRooms.Add(b);
        this.lv2iCells = cells;
        this.lv2iEndPoints = new List<Vector2Int>();
    }

    public void AddCells(List<Vector2Int> pathCells)
    {
        foreach(Vector2Int c in pathCells)
        {
            lv2iCells.Add(c);
        }
    }

    public List<Vector2Int> GetCells()
    {
        return lv2iCells;
    }

    public List<Room> GetRooms()
    {
        return lrRooms;
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
        for (int i = 0; i < c.GetCells().Count; i++)
        {
            if(c.GetCells()[i].x == position.x && c.GetCells()[i].y == position.y)
            {
                return true;
            }
        }

        return false;
    }

    public static bool ContainsPosition(Conection c, int x, int y)
    {
        for (int i = 0; i < c.GetCells().Count; i++)
        {
            if (c.GetCells()[i].x == x && c.GetCells()[i].y == y)
            {
                return true;
            }
        }

        return false;
    }
}
