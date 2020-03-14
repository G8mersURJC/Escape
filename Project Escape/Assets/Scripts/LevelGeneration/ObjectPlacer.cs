using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class ObjectPlacer
{
    private int[,] map;
    private List<Room> lrRooms;

    private Vector2Int v2iPlayerSpawnPosition;
    private Vector2Int v2iLevelExitPosition;
    private Room rSpawn;
    private Room rExit;

    public ObjectPlacer()
    {

    }

    public int[,] PlaceElementsInMap(int[,] map, List<Room> roomList)
    {
        this.map = map;
        this.lrRooms = roomList;

        PlaceSpawnAndExit();
        PlaceObstacles();
        PlaceEnemySpawns();

        return map;
    }

    private void PlaceSpawnAndExit()
    {
        //Buscamos las salas más alejadas entre si
        //Tomamos como referencia la primera sala de la lista
        rSpawn = lrRooms[0];
        rExit = lrRooms[0];

        if (lrRooms.Count > 1)
        {
            rExit = lrRooms[1];
        }

        float furthestDistance = Vector2Int.Distance(rSpawn.GetIndexCenter(), rExit.GetIndexCenter());

        foreach (Room rA in lrRooms)
        {
            foreach (Room rB in lrRooms)
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

        v2iPlayerSpawnPosition = PlaceValueInRandomCellOfARoom(3, rSpawn);
        v2iLevelExitPosition = PlaceValueInRandomCellOfARoom(4, rExit);
    }

    private Vector2Int PlaceValueInRandomCellOfARoom(int value, Room r)
    {
        int tries = 20;
        while (tries > 0)
        {
            int i = Random.Range(r.GetIndexPosition().y, r.GetIndexPosition().y + r.GetHeight());
            int j = Random.Range(r.GetIndexPosition().x, r.GetIndexPosition().x + r.GetWidth());

            if (!GetForbiddenCells(r).Contains(new Vector2Int(j, i)))
            {
                map[i, j] = value;
                return new Vector2Int(j, i);
            }

            tries--;
        }

        return new Vector2Int(-1, -1);
    }

    private void PlaceObstacles()
    {
        foreach (Room r in lrRooms)
        {
            /*
            //Debug para visualizar los puntos de conexión
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
                    if (!IsValidIndex(i, j) || forbiddenCells.Contains(new Vector2Int(j, i)))
                    {
                        continue;
                    }

                    //Generamos un obstáculo con cierta probabilidad
                    if (Random.Range(0, 100) > 97)
                    {
                        map[i, j] = 1;
                        if (Random.Range(0, 2) > 0)
                            map[i, j] = 2;
                    }else if(Random.Range(0, 100) > 96)
                    {
                        map[i, j] = 6;
                    }
                }
            }
        }
    }

    private bool IsValidIndex(int i, int j)
    {
        return !(i > map.GetLength(0) - 1 || j > map.GetLength(1) - 1 || i < 0 || j < 0);
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

        if (r.GetId() == rSpawn.GetId())
            list.Add(v2iPlayerSpawnPosition);

        if (r.GetId() == rExit.GetId())
            list.Add(v2iLevelExitPosition);

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

    private void PlaceEnemySpawns()
    {

    }
}