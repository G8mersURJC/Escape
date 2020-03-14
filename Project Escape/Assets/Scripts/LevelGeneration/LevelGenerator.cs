using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator
{
    private Vector2Int v2iMapSize;
    private int iRoomCount;
    private int iMinRoomSize;
    private int iMaxRoomSize;
    private int[,] map;

    private List<Room> llRooms;

    private Vector2Int v2iSpawnCell;
    private Vector2Int v2iExitCell;

    public LevelGenerator()
    {
        v2iMapSize = new Vector2Int(30, 30);
        iRoomCount = 3;
        iMinRoomSize = 4;
        iMaxRoomSize = 10;

        v2iSpawnCell = new Vector2Int();
        v2iExitCell = new Vector2Int();

        llRooms = new List<Room>();
    }

    public LevelGenerator(int size, int roomNumber, int minRoomSize, int maxRoomSize)
    {
        v2iMapSize = new Vector2Int(size, size);
        iRoomCount = roomNumber;
        iMinRoomSize = minRoomSize;
        iMaxRoomSize = maxRoomSize;

        v2iSpawnCell = new Vector2Int();
        v2iExitCell = new Vector2Int();

        llRooms = new List<Room>();
    }

    public int[,] GenerateMap()
    {
        CheckForInvalidParameters();

        map = new int[v2iMapSize.x, v2iMapSize.y];

        GenerateRooms();
        GenerateCorridors();
        GenerateObstacles();

        PlaceSpawnAndExit();

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
        RoomPlacer rp = new RoomPlacer();
        RoomArranger ra = new RoomArranger();

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
        //Conectamos las salas con pasillos
        CorridorGenerator cg = new CorridorGenerator();
        map = cg.GenerateCorridors(map, llRooms);
    }

    private void GenerateObstacles()
    {
        foreach (Room r in llRooms)
        {
            
            /*
            foreach (Conection con in r.GetConections())
            {
                for (int i = 0; i < con.GetEndPoints().Count; i++)
                {
                    if (con.GetRooms()[i].GetId() == r.GetId())
                    {
                        map[con.GetEndPoints()[i].y, con.GetEndPoints()[i].x] = 3;
                    }
                }
            }
            */

            //Generamos una lista con las casillas prohibidas. En estas no se van a generar obstaculos.
            List<Vector2Int> forbiddenCells = GetForbiddenCells(r);

            for (int i = r.GetIndexPosition().y; i < r.GetIndexPosition().y + r.GetHeight(); i++)
            {
                for (int j = r.GetIndexPosition().x; j < r.GetIndexPosition().x + r.GetWidth(); j++)
                {
                    if (!IsValidIndex(i, j))
                    {
                        continue;
                    }

                    //Generamos un obstáculo con cierta probabilidad
                    if (Random.Range(0, 100) > 97 && !forbiddenCells.Contains(new Vector2Int(j, i)))
                    {
                        map[i, j] = 1;
                        if (Random.Range(0, 2) > 0)
                            map[i, j] = 2;
                    }
                }
            }
        }
    }

    private List<Vector2Int> GetForbiddenCells(Room r)
    {
        List<Vector2Int> list = new List<Vector2Int>();

        foreach (Conection con in r.GetConections())
        {
            foreach (Vector2Int v in con.GetEndPoints())
            {
                List<Vector2Int> adyacents = GetAdyacentNodes(v);
                foreach (Vector2Int n in adyacents)
                {
                    list.Add(n);
                }
            }
        }

        if (r.ContainsPlayerSpawn())
            list.Add(r.GetPlayerSpawnPosition());

        if (r.ContainsMapExit())
            list.Add(r.GetMapExitPosition());

        //Debug.Log("Prohibidas: "+list.Count);

        return list;
    }

    private List<Vector2Int> GetAdyacentNodes(Vector2Int n)
    {
        List<Vector2Int> adyacentList = new List<Vector2Int>();

        for (int i = n.y - 1; i <= n.y + 1; i++)
        {
            for (int j = n.x - 1; j <= n.x + 1; j++)
            {
                adyacentList.Add(new Vector2Int(j, i));
            }
        }

        return adyacentList;
    }

    private void PlaceSpawnAndExit()
    {
        //Buscamos las salas más alejadas entre si
        //Tomamos como referencia la primera sala de la lista
        Room rSpawn = llRooms[0];
        Room rExit = llRooms[0];

        if (llRooms.Count > 1)
        {
            rExit = llRooms[1];
        }

        float furthestDistance = Vector2Int.Distance(rSpawn.GetIndexCenter(), rExit.GetIndexCenter());

        foreach (Room rA in llRooms)
        {
            foreach (Room rB in llRooms)
            {
                if (Vector2Int.Distance(rA.GetIndexCenter(), rB.GetIndexCenter()) > furthestDistance
                    && rA.GetId() != rB.GetId())
                {
                    rSpawn = rA;
                    rExit = rB;
                    furthestDistance = Vector2Int.Distance(rA.GetIndexCenter(), rB.GetIndexCenter());
                }
            }
        }

        PlaceValueInRandomCellOfARoom(3, rSpawn);
        PlaceValueInRandomCellOfARoom(4, rExit);
    }

    private void PlaceValueInRandomCellOfARoom(int value, Room r)
    {
        bool placed = false;
        while (!placed)
        {
            int i = Random.Range(r.GetIndexPosition().y, r.GetIndexPosition().y + r.GetHeight());
            int j = Random.Range(r.GetIndexPosition().x, r.GetIndexPosition().x + r.GetWidth());

            if(!GetForbiddenCells(r).Contains(new Vector2Int(j, i)))
            {
                map[i, j] = value;
                placed = true;

                if(value == 3)
                {
                    r.SetPlayerSpawnPosition(new Vector2Int(j, i));
                }else if(value == 4)
                {
                    r.SetMapExitPosition(new Vector2Int(j, i));
                }


            }
        }
    }
}