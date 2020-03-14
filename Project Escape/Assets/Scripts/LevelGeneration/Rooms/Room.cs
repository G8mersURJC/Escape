using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room
{
    private Vector2Int v2IndexPosition;
    private int iWidth;
    private int iHeight;
    private int iId;
    private List<Conection> lcConections;

    public Room()
    {
        this.v2IndexPosition = new Vector2Int();
        this.iWidth = 3;
        this.iHeight = 3;
        this.iId = 0;
        this.lcConections = new List<Conection>();
    }

    public Room(int id, Vector2Int indexPosition, int roomWidth, int roomHeight)
    {
        this.iId = id;
        this.v2IndexPosition = indexPosition;
        this.iWidth = roomWidth;
        this.iHeight = roomHeight;
        this.lcConections = new List<Conection>();
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
        return iId;
    }

    public void AddConection(Conection c)
    {
        this.lcConections.Add(c);
    }

    public List<Conection> GetConections()
    {
        return this.lcConections;
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
            cells.Add(new Vector2Int(j, v2IndexPosition.y + iHeight - 1));
        }

        return cells;
    }

    public List<Vector2Int> GetLeftSideCells()
    {
        List<Vector2Int> cells = new List<Vector2Int>();

        for (int i = v2IndexPosition.y; i < v2IndexPosition.y + iHeight; i++)
        {
            cells.Add(new Vector2Int(v2IndexPosition.x, i));
        }

        return cells;
    }

    public List<Vector2Int> GetRightSideCells()
    {
        List<Vector2Int> cells = new List<Vector2Int>();

        for (int i = v2IndexPosition.y; i < v2IndexPosition.y + iHeight; i++)
        {
            cells.Add(new Vector2Int(v2IndexPosition.x + iWidth - 1, i));
        }

        return cells;
    }

    public static bool ContainsPosition(Room r, int column, int row)
    {
        for (int i = r.GetIndexPosition().y; i < r.GetIndexPosition().y + r.GetHeight(); i++)
        {
            for (int j = r.GetIndexPosition().x; j < r.GetIndexPosition().x + r.GetWidth(); j++)
            {
                if (i == row && j == column)
                    return true;
            }
        }

        return false;
    }

    public static bool HasCellInPosition(Room r, Vector2Int position)
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

    public static bool CheckColision(Room a, Room b)
    {
        return (a.GetIndexPosition().x < b.GetIndexPosition().x + b.GetWidth() &&
               a.GetIndexPosition().x + a.GetWidth() > b.GetIndexPosition().x &&
               a.GetIndexPosition().y < b.GetHeight() + b.GetIndexPosition().y &&
               a.GetIndexPosition().y + a.GetHeight() > b.GetIndexPosition().y);
    }

    public static bool CheckAdyacency(Room a, Room b)
    {
        //Vamos a comprobar solamente las celdas adyacentes
        List<Vector2Int> AAdyacents = Room.GetAllBorderNodes(a);

        foreach(Vector2Int v in AAdyacents) { 
            if (IsCellAdyacentToRoom(new Vector2Int(v.x, v.y), b)){
                    return true;
            }
        }

        return false;
    }

    public static List<Vector2Int> GetAllCells(Room r)
    {
        List<Vector2Int> list = new List<Vector2Int>();

        for (int i = r.GetIndexPosition().y; i < r.GetIndexPosition().y + r.GetHeight(); i++)
        {
            for (int j = r.GetIndexPosition().x; j < r.GetIndexPosition().x + r.GetWidth(); j++)
            {
                list.Add(new Vector2Int(j, i));
            }
        }

        return list;
    }

    public static List<Vector2Int> GetAllBorderNodes(Room r)
    {
        List<Vector2Int> list = new List<Vector2Int>();
        list = AddAll(list, r.GetUpperSideCells());
        list = AddAll(list, r.GetBottomSideCells());
        list = AddAll(list, r.GetLeftSideCells());
        list = AddAll(list, r.GetRightSideCells());

        return list;
    }

    public static bool IsCellAdyacentToRoom(Vector2Int c, Room r)
    {
        List<Vector2Int> cellsToCheck = GetAllBorderNodes(r);

        foreach(Vector2Int n in cellsToCheck)
        {
            if(Mathf.Abs(c.x - n.x) + Mathf.Abs(c.y - n.y) <= 1)
            {
                return true;
            }
        }

        return false;
    }

    private static List<Vector2Int> AddAll(List<Vector2Int> a, List<Vector2Int> b)
    {
        foreach (Vector2Int n in b)
        {
            a.Add(n);
        }

        return a;
    }

}
