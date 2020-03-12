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

        CheckPropertiesValues();
    }

    public LevelGenerator(int size, int roomNumber, int minRoomSize, int maxRoomSize)
    {
        v2iMapSize = new Vector2Int(size, size);
        iRoomCount = roomNumber;
        iMinRoomSize = minRoomSize;
        iMaxRoomSize = maxRoomSize;

        llRooms = new List<Room>();

        CheckPropertiesValues();
    }

    private void CheckPropertiesValues()
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

    public int[,] GenerateMap()
    {
        int[,] map = new int[v2iMapSize.x, v2iMapSize.y];

        RoomPlacer rp = new RoomPlacer();
        RoomArranger ra = new RoomArranger();

        llRooms = rp.GenerateRoomsInMap(v2iMapSize, iRoomCount, iMinRoomSize, iMaxRoomSize);
        llRooms = ra.ArrangeRooms(v2iMapSize, llRooms);

        //Inicializamos las casillas intransitables a -1 (No tiene gráfico)
        for (int i = 0; i < v2iMapSize.y; i++)
        {
            for (int j = 0; j < v2iMapSize.x; j++)
            {
                map[i, j] = -1;
            }
        }

        //Todas las casillas de las habitaciones se ponen a 1 (Suelo).
        foreach (Room r in llRooms)
        {
            for (int i = r.GetIndexPosition().y; i < r.GetIndexPosition().y + r.GetHeight(); i++)
            {
                for (int j = r.GetIndexPosition().x; j < r.GetIndexPosition().x + r.GetWidth(); j++)
                {
                    if (IsValidIndex(i, j))
                        map[i, j] = 0;
                }
            }
        }

        //Conectamos las salas con pasillos
        CorridorGenerator cg = new CorridorGenerator();
        map = cg.GenerateCorridors(map, llRooms);
        Debug.Log("PASILLOS HECHOS");

        //Generamos obstáculos aleatorios en las salas (poquitos)
        foreach (Room r in llRooms)
        {
            for (int i = r.GetIndexPosition().y; i < r.GetIndexPosition().y + r.GetHeight(); i++)
            {
                for (int j = r.GetIndexPosition().x; j < r.GetIndexPosition().x + r.GetWidth(); j++)
                {
                    //Generamos un obstáculo con cierta probabilidad
                    if (Random.Range(0, 100) > 95)
                    {
                        //Comprobamos primero si es una casilla dentro de las dimensiones del mapa
                        if (!IsValidIndex(i, j))
                        {
                            continue;
                        }

                        bool valid = true;
                        foreach (Conexion con in r.GetConections())
                        {
                            if (Conexion.ContainsPosition(con, j, i))
                            {
                                valid = false;
                                break;
                            }
                        }

                        if (valid)
                        {
                            map[i, j] = 1;

                            if (Random.Range(0, 2) > 0)
                                map[i, j] = 2;
                        }
                    }
                }
            }
        }

        return map;
    }

    private bool IsValidIndex(int i, int j)
    {
        return !(i > v2iMapSize.y - 1 || j > v2iMapSize.x - 1 || i < 0 || j < 0);
    }

    
}
