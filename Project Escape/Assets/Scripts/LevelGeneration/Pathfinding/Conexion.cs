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
}
