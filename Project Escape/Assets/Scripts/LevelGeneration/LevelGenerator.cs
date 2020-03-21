using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class LevelGenerator
{
    private Vector2Int v2iMapSize;
    private int iRoomCount;
    private int iMinRoomSize;
    private int iMaxRoomSize;
    private int[,] map;

    private List<Room> llRooms;
    private RoomPlacer rp;
    private RoomArranger ra;
    private CorridorGenerator cg;
    private ObjectPlacer op;

    public LevelGenerator()
    {
        rp = new RoomPlacer();
        ra = new RoomArranger();
        cg = new CorridorGenerator();
        op = new ObjectPlacer();
        llRooms = new List<Room>();
    }

    public int[,] GenerateMap(int size, int roomNumber, int minRoomSize, int maxRoomSize)
    {
        llRooms.Clear();
        v2iMapSize = new Vector2Int(size, size);
        iRoomCount = roomNumber;
        iMinRoomSize = minRoomSize;
        iMaxRoomSize = maxRoomSize;

        CheckForInvalidParameters();

        map = new int[v2iMapSize.x, v2iMapSize.y];

        GenerateRooms();
        GenerateCorridors();

        map = op.PlaceElementsInMap(map, llRooms);

        StreamWriter writer = new StreamWriter("Assets/Data/Map1.txt");
        for(int i=0; i< size; i++)
        {
            for(int j=0; j< size; j++)
            {
                writer.Write(map[i, j]);
            }
            writer.Write("\n");

        }

        writer.Close();
        return map;
    }

    private void CheckForInvalidParameters()
    {
        if (v2iMapSize.x < 10)
            v2iMapSize = new Vector2Int(10, 10);

        if (iRoomCount < 1)
            iRoomCount = 1;

        if (iMinRoomSize < 3)
            iMinRoomSize = 3;

        if (iMaxRoomSize < 0 || iMaxRoomSize < iMinRoomSize)
            iMaxRoomSize = iMinRoomSize;
    }

    private void GenerateRooms()
    {
        llRooms = rp.GenerateRoomsInMap(v2iMapSize, iRoomCount, iMinRoomSize, iMaxRoomSize);
        llRooms = ra.ArrangeRooms(v2iMapSize, llRooms);

        FillMapCellsWithValue(-1);
        FillRoomCellsWithValue(0);
    }

    private void FillMapCellsWithValue(int value)
    {
        for (int i = 0; i < v2iMapSize.y; i++)
        {
            for (int j = 0; j < v2iMapSize.x; j++)
            {
                map[i, j] = value;
            }
        }
    }

    private void FillRoomCellsWithValue(int value)
    {
        foreach (Room r in llRooms)
        {
            //Debug.Log("Room "+r.GetId()+" en "+r.GetIndexPosition().ToString());

            for (int i = r.GetIndexPosition().y; i < r.GetIndexPosition().y + r.GetHeight(); i++)
            {
                for (int j = r.GetIndexPosition().x; j < r.GetIndexPosition().x + r.GetWidth(); j++)
                {
                    if (IsValidIndex(i, j))
                    {
                        map[i, j] = value;
                    }
                    else
                    {
                        Debug.Log("CASILLA DE SALA INVALIDA: "+i+", "+j);
                    }
                        
                }
            }
        }
    }

    private bool IsValidIndex(int i, int j)
    {
        return !(i > v2iMapSize.y - 1 || j > v2iMapSize.x - 1 || i < 0 || j < 0);
    }

    private void GenerateCorridors()
    {
        map = cg.GenerateCorridors(map, llRooms);
    }
}