using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CorridorGenerator
{
    AStarPathGenerator pathGenerator;
    int[,] map;

    public int[,] GenerateCorridors(int[,] map, List<Room> rooms)
    {
        this.map = map;
        List<Room> connectedRooms = new List<Room>();
        List<Room> disconectedRooms = new List<Room>();

        foreach(Room r in rooms)
        {
            disconectedRooms.Add(r);
        }

        //disconectedRooms.AddRange(disconectedRooms);

        connectedRooms.Add(disconectedRooms[0]);
        disconectedRooms.RemoveAt(0);

        pathGenerator = new AStarPathGenerator();

        Debug.Log("Conectamos cosas");

        int tries = disconectedRooms.Count;
        while (disconectedRooms.Count > 0 && tries > 0)
        {
            Debug.Log("Quedan por conectar "+disconectedRooms.Count);

            //Buscar la sala sin conectar más cercana a una de las ya conectadas.
            List<Room> roomsToConnect = FindClosestRoomsIndexToConnect(connectedRooms, disconectedRooms);


            //Comprobar si ya existe un camino entre esas 2 salas.
            if (AreRoomsConnected(roomsToConnect[0], roomsToConnect[1]))
            {
                Debug.Log("Estaban ya conectadas");
                connectedRooms.Add(roomsToConnect[1]);
                disconectedRooms.Remove(roomsToConnect[1]);
            }
            else
            {
                Debug.Log("Generamos un camino para ellos");
                //Generar camino entre las 2 salas
                Conexion c = GenerateCorridorBetweenRooms(roomsToConnect[0], roomsToConnect[1]);
                roomsToConnect[0].AddConection(c);
                roomsToConnect[0].AddConection(c);

                List<Vector2Int> path = c.GetCells();

                Debug.Log("Camino generado");

                if (path == null)
                    Debug.Log("Nos comemos una mierda");

                //Aplicar camino calculado en el array
                foreach (Vector2Int n in path)
                {
                    if (IsValidIndex(n.y, n.x))
                        map[n.y, n.x] = 0;
                }

                //Sacar la sala de la lista
                connectedRooms.Add(roomsToConnect[1]);
                disconectedRooms.Remove(roomsToConnect[1]);
            }

            tries--;
        }
        return map;
    }

    private bool IsValidIndex(int i, int j)
    {
        return !(i > map.GetLength(0) - 1 || j > map.GetLength(1) - 1 || j < 0 || i < 0);
    }

    /*
    public int[,] GenerateCorridorsAStar(int[,] map, List<Room> rooms)
    {

        AStarPathGenerator pg = new AStarPathGenerator();

        List<Room> connectedRooms = new List<Room>();

        connectedRooms.Add(rooms[0]);
        rooms.RemoveAt(0);

        int tries = rooms.Count;
        while(rooms.Count > 0 && tries > 0)
        {
            //Debug.Log("Conectadas " + connectedRooms.Count);
            Debug.Log("Quedan "+rooms.Count);

            //Buscar la sala sin conectar más cercana a una de las ya conectadas.
            //Debug.Log("Buscamos las 2 siguientes rooms");
            List<Room> roomsToConnect = FindRoomsIndexToConnectV1(connectedRooms, rooms);
            //Debug.Log("Tratamos " + connectedRooms[roomIndexes.x].GetId() + " con " + rooms[roomIndexes.y].GetId());

            //Comprobar si ya existe un camino entre esas 2 salas.
            if (pg.GeneratePath(roomsToConnect[0].GetIndexCenter(),
                    roomsToConnect[1].GetIndexCenter(), map, false) != null)
            {

                //Debug.Log("Ya estaban conectadas " + connectedRooms[roomIndexes.x].GetId() + " - " + rooms[roomIndexes.y].GetId());
                connectedRooms.Add(roomsToConnect[1]);
                rooms.Remove(roomsToConnect[1]);
            }
            else
            {
                Debug.Log("Generamos un camino para ellos");
                //Generar camino entre las 2 salas
                List<Vector2Int> path = pg.GeneratePath(roomsToConnect[0].GetIndexCenter(),
                    roomsToConnect[1].GetIndexCenter(), map, true);

                Debug.Log("Camino generado");

                if (path == null)
                    Debug.Log("Nos comemos una mierda");

                //Aplicar camino calculado en el array
                foreach (Vector2Int n in path)
                {
                    map[n.y, n.x] = 0;
                }

                //Debug.Log("Conectadas " + connectedRooms[roomIndexes.x].GetId() + " - " + rooms[roomIndexes.y].GetId());

                //Sacar la sala de la lista
                connectedRooms.Add(roomsToConnect[1]);
                rooms.Remove(roomsToConnect[1]);
            }

            tries--;
        }
        return map;
    }
    */
    private List<Room> FindClosestRoomsIndexToConnect(List<Room> connectedRooms, List<Room> disconectedRooms)
    {
        //Tomamos como referencia la primera sala de la lista
        Room conected = connectedRooms[0];
        Room disconected = disconectedRooms[0];

        float closestDistance = Vector2Int.Distance(conected.GetIndexCenter(), disconected.GetIndexCenter());

        for (int i = 0; i < disconectedRooms.Count; i++)
        {
            for(int j = 0; j < connectedRooms.Count; j++)
            {
                if(Vector2Int.Distance(disconectedRooms[i].GetIndexCenter(), connectedRooms[j].GetIndexCenter()) < closestDistance
                    && disconectedRooms[i].GetId() != connectedRooms[j].GetId())
                {
                    disconected = disconectedRooms[i];
                    conected = connectedRooms[j];
                    closestDistance = Vector2Int.Distance(disconectedRooms[i].GetIndexCenter(), connectedRooms[j].GetIndexCenter());
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

    private bool AreRoomsConnected(Room a, Room b)
    {
        //Comprobamos primero si ya están conectadas de por sí (sin conexión)
        //En caso de que no, comprobamos las conexíones existentes
        return (Room.CheckCollision(a, b) || pathGenerator.GeneratePath(a, b) != null);
    }

    private Conexion GenerateCorridorBetweenRooms(Room a, Room b)
    {
        //Determinamos la posición relativa de las Salas
        bool AisUp = (a.GetIndexPosition().y >= b.GetIndexPosition().y);
        bool AisLeft = (a.GetIndexPosition().x <= b.GetIndexPosition().x);

        //Decidimos que lados conectar
        /*
         * Si A está por encima: 
         *  (A) Preferimos el lado de abajo (A)
         *  (B) Preferimos el lado de arriba (B)
         *  
         * Si a está por debajo:
         *  (A) Preferimos el lado de arriba (A)
         *  (B) Preferimos el lado de abajo (B)
         *  
         *  Si A está a la izquierda:
         *  (A) Preferimos el lado derecho (A)
         *  (B) Preferimos el lado de izquierdo (B)
         *  
         *  Si A está por la derecha:
         *  (A) Preferimos el lado izquierdo (A)
         *  (B) Preferimos el lado de derecho (B)
         */

        int[] Asides = new int[2];
        int[] Bsides = new int[2];

        //0 = Up, 1 = Down, 2 = Left, 3 = Right

        if (AisUp)
        {
            Asides[0] = 1;
            Bsides[0] = 0;
        }
        else
        {
            Asides[0] = 0;
            Bsides[0] = 1;
        }

        if (AisLeft)
        {
            Asides[1] = 3;
            Bsides[1] = 2;
        }
        else
        {
            Asides[1] = 2;
            Bsides[1] = 3;
        }

        //De los seleccionados, optamos por 1 lado
        int chosenASide = Asides[Random.Range(0, 2)];
        int chosenBSide = Bsides[Random.Range(0, 2)];

        List<Vector2Int> Anodes = GetCellsFromConection(a, chosenASide);
        List<Vector2Int> Bnodes = GetCellsFromConection(b, chosenBSide);

        //Seleccionamos los nodos n1 y n2 de esos lados
        Vector2Int StartCell = Anodes[Random.Range(0, Anodes.Count)];
        Vector2Int TargetCell = Bnodes[Random.Range(0, Bnodes.Count)];

        //Generamos el camino


        Vector2Int currentNode = StartCell;
        List<Vector2Int> path = new List<Vector2Int>();
        //int tries = 30;

        //1ª Fase: Avanzamos 2 casillas en la dirección de salida tanto de A como de B
        currentNode = MoveVectorInDirection(currentNode, chosenASide);
        path.Add(currentNode);
        currentNode = MoveVectorInDirection(currentNode, chosenASide);
        path.Add(currentNode);

        TargetCell = MoveVectorInDirection(TargetCell, chosenBSide);
        path.Add(TargetCell);
        TargetCell = MoveVectorInDirection(TargetCell, chosenBSide);
        path.Add(TargetCell);

        int tries = 50;
        while (!Vector2Int.Equals(currentNode, TargetCell) && tries > 0)
        {
            //Modo simplón: Primero horizontal, después vertical
            if(AisLeft && TargetCell.x > currentNode.x)
            {
                currentNode = new Vector2Int(currentNode.x + 1, currentNode.y);
                path.Add(currentNode);
                continue;
            }
            else if (!AisLeft && TargetCell.x < currentNode.x)
            {
                currentNode = new Vector2Int(currentNode.x - 1, currentNode.y);
                path.Add(currentNode);
                continue;
            }

            if (AisUp && TargetCell.y < currentNode.y)
            {
                currentNode = new Vector2Int(currentNode.x, currentNode.y - 1);
                path.Add(currentNode);
                continue;
            }
            else if (!AisUp && TargetCell.y > currentNode.y)
            {
                currentNode = new Vector2Int(currentNode.x, currentNode.y + 1);
                path.Add(currentNode);
            }
            tries--;

            if (tries == 0)
                Debug.Log("No hay mas TRIES que valgan");
        }
        

        return new Conexion(a, b, path);
    }

    private Vector2Int MoveVectorInDirection(Vector2Int c, int dir)
    {
        Vector2Int newC = new Vector2Int();

        switch (dir)
        {
            case 0:
                newC = new Vector2Int(c.x, c.y + 1);
                break;
            case 1:
                newC = new Vector2Int(c.x, c.y - 1);
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
