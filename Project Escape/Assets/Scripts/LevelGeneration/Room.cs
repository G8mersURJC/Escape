using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room
{
    private Vector2Int v2IndexPosition;
    private int iWidth;
    private int iHeight;
    private int id;
    private List<Conexion> conexiones;
    
    public Room()
    {
        this.v2IndexPosition = new Vector2Int();
        this.iWidth = 3;
        this.iHeight = 3;
        this.id = 0;
        this.conexiones = new List<Conexion>();
    }

    public Room(int id, Vector2Int indexPosition, int roomWidth, int roomHeight)
    {
        this.id = id;
        this.v2IndexPosition = indexPosition;
        this.iWidth = roomWidth;
        this.iHeight = roomHeight;
        this.conexiones = new List<Conexion>();
    }

    public Vector2Int GetIndexPosition()
    {
        return this.v2IndexPosition;
    }

    public void SetIndexPosition(Vector2Int newIndexPosition)
    {
        this.v2IndexPosition = newIndexPosition;
    }

    public int GetWidth()
    {
        return this.iWidth;
    }

    public void SetWidth(int newWidth)
    {
        this.iWidth = newWidth;
    }

    public int GetHeight()
    {
        return this.iHeight;
    }

    public void SetHeight(int newHeight)
    {
        this.iWidth = newHeight;
    }

    public Vector2Int GetIndexCenter()
    {
        return new Vector2Int(v2IndexPosition.x + Mathf.FloorToInt(iWidth / 2), v2IndexPosition.y + Mathf.FloorToInt(iHeight / 2));
    }

    public int GetId()
    {
        return id;
    }

    public void AddConection(Conexion c)
    {
        this.conexiones.Add(c);
    }

    public List<Conexion> GetConections()
    {
        return this.conexiones;
    }


    public List<Vector2Int> GetUpperSideCells()
    {
        List<Vector2Int> cells = new List<Vector2Int>();

        for (int j = v2IndexPosition.x; j < v2IndexPosition.x + iWidth; j++)
        {
            cells.Add(new Vector2Int(j, v2IndexPosition.y));
        }

        return cells;
    }

    public List<Vector2Int> GetBottomSideCells()
    {
        List<Vector2Int> cells = new List<Vector2Int>();

        for (int j = v2IndexPosition.x; j < v2IndexPosition.x + iWidth; j++)
        {
            cells.Add(new Vector2Int(j, v2IndexPosition.y + iHeight));
        }

        return cells;
    }

    public List<Vector2Int> GetRightSideCells()
    {
        List<Vector2Int> cells = new List<Vector2Int>();

        for (int i = v2IndexPosition.y; i < v2IndexPosition.y + iHeight; i++)
        {
            cells.Add(new Vector2Int(v2IndexPosition.x, i));
        }

        return cells;
    }

    public List<Vector2Int> GetLeftSideCells()
    {
        List<Vector2Int> cells = new List<Vector2Int>();

        for (int i = v2IndexPosition.y; i < v2IndexPosition.y + iHeight; i++)
        {
            cells.Add(new Vector2Int(v2IndexPosition.x + iWidth, i));
        }

        return cells;
    }

    public static bool ContainsPosition(Room r, int x, int y)
    {
        for (int i = r.GetIndexPosition().y; i < r.GetIndexPosition().y + r.GetHeight(); i++)
        {
            for (int j = r.GetIndexPosition().x; j < r.GetIndexPosition().x + r.GetWidth(); j++)
            {
                if (i == y && j == x)
                    return true;
            }
        }

        return false;
    }

    public static bool ContainsPosition(Room r, Vector2Int position)
    {
        for (int i = r.GetIndexPosition().y; i < r.GetIndexPosition().y + r.GetHeight(); i++)
        {
            for (int j = r.GetIndexPosition().x; j < r.GetIndexPosition().x + r.GetWidth(); j++)
            {
                if (i == position.y && j == position.x)
                    return true;
            }
        }

        return false;
    }

    public static bool CheckCollision(Room r1, Room r2)
    {
        return (r1.GetIndexPosition().x < r2.GetIndexPosition().x + r2.GetWidth() &&
               r1.GetIndexPosition().x + r1.GetWidth() > r2.GetIndexPosition().x &&
               r1.GetIndexPosition().y < r2.GetHeight() + r2.GetIndexPosition().y &&
               r1.GetIndexPosition().y + r1.GetHeight() > r2.GetIndexPosition().y);
    }

}
