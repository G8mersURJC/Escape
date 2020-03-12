using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator
{
    private Vector2Int v2iMapSize;
    private int iRoomCount;
    private int iMinRoomSize;
    private int iMaxRoomSize;

    private List<Room> llRooms;

    public LevelGenerator()
    {
        v2iMapSize = new Vector2Int(30, 30);
        iRoomCount = 3;
        iMinRoomSize = 4;
        iMaxRoomSize = 10;

        llRooms = new List<Room>();
    }

    public LevelGenerator(int size, int roomNumber, int minRoomSize, int maxRoomSize)
    {
        v2iMapSize = new Vector2Int(size, size);
        iRoomCount = roomNumber;
        iMinRoomSize = minRoomSize;
        iMaxRoomSize = maxRoomSize;

        llRooms = new List<Room>();
    }

    public int[,] GenerateMap()
    {
        int[,] map = new int[v2iMapSize.x, v2iMapSize.y];

        RoomPlacer rp = new RoomPlacer();
        RoomArranger ra = new RoomArranger();

        llRooms = rp.GenerateRoomsInMap(v2iMapSize, iRoomCount, iMinRoomSize, iMaxRoomSize);
        llRooms = ra.ArrangeRooms(v2iMapSize, llRooms);


        //Inicializamos las paredes intransitables a 1
        for (int i = 0; i < v2iMapSize.y; i++)
        {
            for (int j = 0; j < v2iMapSize.x; j++)
            {
                map[i, j] = -1;
            }
        }

        //Todas las casillas de las habitaciones se ponen a 1.
        int c = 0;
        foreach (Room r in llRooms)
        {
            Debug.Log("Room "+c+" en " + r.GetIndexCenter().x + ", " + r.GetIndexCenter().y);
            c++;
            for (int i = r.GetIndexPosition().y; i < r.GetIndexPosition().y + r.GetHeight(); i++)
            {
                for (int j = r.GetIndexPosition().x; j < r.GetIndexPosition().x + r.GetWidth(); j++)
                {
                    if (IsValidIndex(i, j))
                        map[i, j] = 0;
                }
            }
        }

        CorridorGenerator cg = new CorridorGenerator();
        map = cg.GenerateCorridors(map, llRooms);

        

        return map;
    }

    private bool IsValidIndex(int i, int j)
    {
        return !(i > v2iMapSize.y - 1 || j > v2iMapSize.x - 1 || i < 0 || j < 0);
    }
}
