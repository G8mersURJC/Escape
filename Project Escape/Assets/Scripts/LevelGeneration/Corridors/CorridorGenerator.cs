﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CorridorGenerator
{
    AStarPathGenerator aPathGenerator;
    int[,] map;
    private List<Room> lrConectedRooms;
    private List<Room> lrDisconectedRooms;

    public int[,] GenerateCorridors(int[,] mapArray, List<Room> rooms)
    {
        this.map = mapArray;
        lrConectedRooms = new List<Room>();
        lrDisconectedRooms = new List<Room>();
        aPathGenerator = new AStarPathGenerator();

        foreach (Room r in rooms)
        {
            lrDisconectedRooms.Add(r);
        }

        ConectColidingAndAdyacentRooms();

        GenerateGraphConections();

        GenerateAditionalConections();

        return mapArray;
    }

    private void ConectColidingAndAdyacentRooms()
    {
        foreach (Room a in lrDisconectedRooms)
        {
            foreach (Room b in lrDisconectedRooms)
            {
                if (a.GetId() != b.GetId() && !ConectionAlreadyExist(a, b))
                {
                    if (Room.CheckColision(a, b))
                    {
                        //Buscamos una casilla colindante entre ambas salas
                        foreach (Vector2Int v in Room.GetAllCells(a))
                        {
                            if (Room.HasCellInPosition(b, v))
                            {
                                AddConectionBetweenColidingRooms(a, b, v);
                                break;
                            }
                        }
                    }
                    else if (Room.CheckAdyacency(a, b))
                    {
                        //Buscamos una casilla vecina entre ambas salas
                        foreach(Vector2Int v in Room.GetAllBorderNodes(a))
                        {
                            if (Room.IsCellAdyacentToRoom(v, b))
                            {
                                AddConectionBetweenColidingRooms(a, b, v);
                                break;
                            }
                        }
                    }
                }
            }
        }
    }

    private void AddConectionBetweenColidingRooms(Room a, Room b, Vector2Int n)
    {
        List<Vector2Int> conectionNodes = new List<Vector2Int>();
        conectionNodes.Add(n);
        a.AddConection(new Conection(b, conectionNodes, conectionNodes));
        b.AddConection(new Conection(a, conectionNodes, conectionNodes));
    }

    private bool ConectionAlreadyExist(Room a, Room b)
    {
        foreach (Conection c in a.GetConections())
        {
            if (c.GetRoom().GetId() == b.GetId())
            {
                return true;
            }
        }

        return false;
    }

    private void GenerateGraphConections()
    {
        //Consideramos a la primera sala de la lista como ya conectada
        MarkRoomAsConected(lrDisconectedRooms[0]);

        int tries = lrDisconectedRooms.Count;
        while (lrDisconectedRooms.Count > 0 && tries > 0)
        {
            //Buscar las salas sin conectar más cercana a una de las ya conectadas.
            List<Room> roomsToConnect = FindClosestRoomsIndexToConnect(lrConectedRooms, lrDisconectedRooms);

            
            if (AreRoomsConnected(roomsToConnect[0], roomsToConnect[1], true))
            {
                MarkRoomAsConected(roomsToConnect[1]);
            }
            else
            {
                if(GenerateCorridorBetweenRooms(roomsToConnect[0], roomsToConnect[1]))
                {
                    ApplyPathInMap(roomsToConnect[0].GetConections()[roomsToConnect[0].GetConections().Count - 1].GetCells());
                    MarkRoomAsConected(roomsToConnect[1]);
                }
            }

            tries--;
        }
    }

    private void MarkRoomAsConected(Room r)
    {
        lrConectedRooms.Add(r);
        lrDisconectedRooms.Remove(r);
    }

    private void ApplyPathInMap(List<Vector2Int> path)
    {
        //Aplicar camino calculado en el array
        foreach (Vector2Int n in path)
        {
            if (IsValidIndex(n.y, n.x))
            {
                map[n.y, n.x] = 0;
            }
            else
            {
                Debug.Log("NODO DE CONEXIÓN INVALIDO: " + n.y + ", " + n.x);
            }
        }
    }

    private void GenerateAditionalConections()
    {
        int newConections = lrConectedRooms.Count / 2;

        int tries = newConections;
        while (tries > 0)
        {
            List<Room> roomsToConnect = FindRandomRoomsToConnect(lrConectedRooms, lrConectedRooms);

            if (roomsToConnect[0].GetId() != roomsToConnect[1].GetId() && !AreRoomsConnected(roomsToConnect[0], roomsToConnect[1], false))
            {
                //Generar camino entre las 2 salas
                if (GenerateCorridorBetweenRooms(roomsToConnect[0], roomsToConnect[1]))
                {
                    ApplyPathInMap(roomsToConnect[0].GetConections()[roomsToConnect[0].GetConections().Count - 1].GetCells());
                    MarkRoomAsConected(roomsToConnect[1]);
                }
            }

            tries--;
        }
    }

    private bool IsValidIndex(int i, int j)
    {
        return !(i > map.GetLength(0) - 1 || j > map.GetLength(1) - 1 || j < 0 || i < 0);
    }
    
    private List<Room> FindClosestRoomsIndexToConnect(List<Room> connectedRooms, List<Room> disconectedRooms)
    {
        //Tomamos como referencia la primera sala de la lista
        Room conected = connectedRooms[0];
        Room disconected = disconectedRooms[0];
        float closestDistance = Vector2Int.Distance(conected.GetIndexCenter(), disconected.GetIndexCenter());

        foreach(Room rA in disconectedRooms)
        {
            foreach (Room rB in connectedRooms)
            {
                if (Vector2Int.Distance(rA.GetIndexCenter(), rB.GetIndexCenter()) < closestDistance
                    && rA.GetId() != rB.GetId())
                {
                    disconected = rA;
                    conected = rB;
                    closestDistance = Vector2Int.Distance(rA.GetIndexCenter(), rB.GetIndexCenter());
                }
            }
        }

        List<Room> selectedRooms = new List<Room>();
        selectedRooms.Add(conected);
        selectedRooms.Add(disconected);

        return selectedRooms;
    }

    private List<Room> FindRandomRoomsToConnect(List<Room> connectedRooms, List<Room> disconectedRooms)
    {
        List<Room> rooms = new List<Room>();
        rooms.Add(connectedRooms[Random.Range(0, connectedRooms.Count)]);
        rooms.Add(disconectedRooms[Random.Range(0, disconectedRooms.Count)]);

        return rooms;
    }

    private bool AreRoomsConnected(Room a, Room b, bool usePathFinding)
    {

        if(ConectionAlreadyExist(a, b))
        {
            return true;
        }

        if (usePathFinding)
        {
            return (aPathGenerator.GeneratePath(a, b) != null);
        }

        return false;
    }

    private bool IsRoomCloseToMapBorder(Room r, int side)
    {
        switch (side)
        {
            case 0:
                return (r.GetIndexPosition().y < 2);
            case 1:
                return (r.GetIndexPosition().y + r.GetHeight() >= map.GetLength(0) - 2);
            case 2:
                return (r.GetIndexPosition().x  < 2);
            case 3:
                return (r.GetIndexPosition().x + r.GetWidth() >= map.GetLength(1) - 2);
        }

        return false;
    }

    private int[] FindSidesToConnect(Room a, Room b)
    {
        List<int> SidesForA = new List<int>();
        List<int> SidesForB = new List<int>();

        //Determinamos la posición relativa de las Salas
        //0 = Up, 1 = Down, 2 = Left, 3 = Right
        if ((a.GetIndexPosition().y < b.GetIndexPosition().y))
        {
            //A por encima de B
            if(!IsRoomCloseToMapBorder(a, 1))
                SidesForA.Add(1);

            if (!IsRoomCloseToMapBorder(b, 0))
                SidesForB.Add(0);

        }
        else if((a.GetIndexPosition().y > b.GetIndexPosition().y))
        {
            //A por debajo de B
            if (!IsRoomCloseToMapBorder(a, 0))
                SidesForA.Add(0);
            if (!IsRoomCloseToMapBorder(b, 1))
                SidesForB.Add(1);
        }
        else
        {
            //A a la altura de B
        }

        if (a.GetIndexPosition().x < b.GetIndexPosition().x)
        {
            //A a la izquierda de B
            if (!IsRoomCloseToMapBorder(a, 3))
                SidesForA.Add(3);
            if (!IsRoomCloseToMapBorder(b, 2))
                SidesForB.Add(2);
        }
        else if(a.GetIndexPosition().x > b.GetIndexPosition().x)
        {
            //A a la derecha de B
            if (!IsRoomCloseToMapBorder(a, 2))
                SidesForA.Add(2);
            if (!IsRoomCloseToMapBorder(b, 3))
                SidesForB.Add(3);
        }
        else
        {
            //A a la altura de B
        }

        //De las posibles opciones, seleccionamos aleatoriamente
        int chosenASide = SidesForA[Random.Range(0, SidesForA.Count)];
        int chosenBSide = SidesForB[Random.Range(0, SidesForB.Count)];

        return new int[] {chosenASide, chosenBSide};
    }

    private Vector2Int GetRandomNodeFromSide(Room r, int side)
    {
        List<Vector2Int> nodes = GetCellsFromConection(r, side);
        return nodes[Random.Range(0, nodes.Count)];
    }

    private bool CanMoveToDirection(Vector2Int c, int dir)
    {
        if(dir == 0 && c.y > 0)
        {
            return true;
        }
        else if (dir == 1 && c.y < map.GetLength(0))
        {
            return true;
        }
        else if (dir == 2 && c.x > 0)
        {
            return true;
        }
        else if(dir == 3 && c.x < map.GetLength(1))
        {
            return true;
        }

        return false;
    }

    private bool GenerateCorridorBetweenRooms(Room a, Room b)
    {
        //Debug.Log("CONECTAMOS LAS SALAS "+a.GetId()+" con "+b.GetId());
        int[] ChosenSides = FindSidesToConnect(a, b);

        Vector2Int StartCell = GetRandomNodeFromSide(a, ChosenSides[0]);
        Vector2Int TargetCell = GetRandomNodeFromSide(b, ChosenSides[1]);

        List<Vector2Int> endPoints = new List<Vector2Int>();
        endPoints.Add(StartCell);
        endPoints.Add(TargetCell);

        Vector2Int currentNode = StartCell;
        List<Vector2Int> path = new List<Vector2Int>();

        //1ª Fase: Avanzamos 2 casillas en la dirección de salida tanto de A como de B
        for(int i = 0; i < 2; i++)
        {
            if(CanMoveToDirection(currentNode, ChosenSides[0]))
            {
                currentNode = MoveVectorInDirection(currentNode, ChosenSides[0]);
                path.Add(currentNode);
            }

            if (CanMoveToDirection(TargetCell, ChosenSides[1]))
            {
                TargetCell = MoveVectorInDirection(TargetCell, ChosenSides[1]);
                path.Add(TargetCell);
            }
        }

        bool AMovingHorizontal = (ChosenSides[0] == 0 || ChosenSides[0] == 1);
        bool BMovingHorizontal = (ChosenSides[1] == 0 || ChosenSides[1] == 1);

        //2ª Fase: Generamos el camino para conectar ambos puntos
        int tries = 100;
        while (!Vector2Int.Equals(currentNode, TargetCell) && tries > 0)
        {
            //CurrentNode trata de desplazarse hacia Target
            if (AMovingHorizontal)
            {
                if (currentNode.x < TargetCell.x && CanMoveToDirection(currentNode, 3))
                {
                    currentNode = new Vector2Int(currentNode.x + 1, currentNode.y);
                    path.Add(currentNode);
                }
                else if(currentNode.x > TargetCell.x && CanMoveToDirection(currentNode, 2))
                {
                    currentNode = new Vector2Int(currentNode.x - 1, currentNode.y);
                    path.Add(currentNode);
                }
                else
                {
                    AMovingHorizontal = !AMovingHorizontal;
                }
            }
            else
            {
                if (currentNode.y < TargetCell.y && CanMoveToDirection(currentNode, 1))
                {
                    currentNode = new Vector2Int(currentNode.x, currentNode.y + 1);
                    path.Add(currentNode);
                }
                else if (currentNode.y > TargetCell.y && CanMoveToDirection(currentNode, 0))
                {
                    currentNode = new Vector2Int(currentNode.x, currentNode.y - 1);
                    path.Add(currentNode);
                }
                else
                {
                    AMovingHorizontal = !AMovingHorizontal;
                }
            }
            
            //Target intenta desplazarse hacia CurrentNode
            if (BMovingHorizontal)
            {
                if (TargetCell.x < currentNode.x && CanMoveToDirection(TargetCell, 3))
                {
                    TargetCell = new Vector2Int(TargetCell.x + 1, TargetCell.y);
                    path.Add(TargetCell);
                }
                else if (TargetCell.x > currentNode.x && CanMoveToDirection(TargetCell, 2))
                {
                    TargetCell = new Vector2Int(TargetCell.x - 1, TargetCell.y);
                    path.Add(TargetCell);
                }
                else
                {
                    BMovingHorizontal = !BMovingHorizontal;
                }
            }
            else
            {
                if (TargetCell.y < currentNode.y && CanMoveToDirection(TargetCell, 1))
                {
                    TargetCell = new Vector2Int(TargetCell.x, TargetCell.y + 1);
                    path.Add(TargetCell);
                }
                else if (TargetCell.y > currentNode.y && CanMoveToDirection(TargetCell, 0))
                {
                    TargetCell = new Vector2Int(TargetCell.x, TargetCell.y - 1);
                    path.Add(TargetCell);
                }
                else
                {
                    BMovingHorizontal = !BMovingHorizontal;
                }
            }
            
           
            tries--;

            if (tries == 0)
            {
                Debug.Log("No hay mas TRIES que valgan");
                return false;
            }

        }

        a.AddConection(new Conection(b, path, endPoints));
        b.AddConection(new Conection(a, path, endPoints));

        return true;
    }

    private Vector2Int MoveVectorInDirection(Vector2Int c, int dir)
    {
        Vector2Int newC = new Vector2Int();

        switch (dir)
        {
            case 0:
                newC = new Vector2Int(c.x, c.y - 1);
                break;
            case 1:
                newC = new Vector2Int(c.x, c.y + 1);
                break;
            case 2:
                newC = new Vector2Int(c.x - 1, c.y);
                break;
            case 3:
                newC = new Vector2Int(c.x + 1, c.y);
                break;
        }

        if (IsValidIndex(newC.y, newC.x))
        {
            return newC;
        }
        else
        {
            return new Vector2Int();    //No avanzamos
        }
    }

    private List<Vector2Int> GetCellsFromConection(Room r, int side)
    {
        switch (side)
        {
            case 0:
                return r.GetUpperSideCells();
            case 1:
                return r.GetBottomSideCells();
            case 2:
                return r.GetLeftSideCells();
            case 3:
                return r.GetRightSideCells();
        }

        return r.GetUpperSideCells();
    }
}
