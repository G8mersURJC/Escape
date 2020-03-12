using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Conexion
{
    private List<Room> rooms;
    private List<Vector2Int> casillas;

    public Conexion(Room a, Room b)
    {
        this.rooms = new List<Room>();
        rooms.Add(a);
        rooms.Add(b);
        this.casillas = new List<Vector2Int>();
    }

    public Conexion(Room a, Room b, List<Vector2Int> casillas)
    {
        this.rooms = new List<Room>();
        rooms.Add(a);
        rooms.Add(b);
        this.casillas = casillas;
    }

    public void AddCells(List<Vector2Int> pathCells)
    {
        casillas.AddRange(pathCells);
    }

    public List<Vector2Int> GetCells()
    {
        return casillas;
    }

    public List<Room> GetRooms()
    {
        return rooms;
    }

    public static bool ContainsPosition(Conexion c, Vector2Int position)
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

    public static bool ContainsPosition(Conexion c, int x, int y)
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
