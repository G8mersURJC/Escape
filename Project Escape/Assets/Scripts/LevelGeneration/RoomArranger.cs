using UnityEngine;
using System.Collections.Generic;

public class RoomArranger
{

    public List<Room> ArrangeRooms(Vector2Int mapSize, List<Room> lrRooms)
    {
        int displacement = 1;
        int tries = 10;
        bool roomsColliding = true;

        List<Vector2Int> steeringVectors = SetupSteeringVectors(lrRooms.Count);
        while (roomsColliding && tries > 0)
        {
            roomsColliding = false;

            for (int i = 0; i < lrRooms.Count; i++)
            {
                for (int j = 0; j < lrRooms.Count; j++)
                {
                    Room r1 = lrRooms[i];
                    Room r2 = lrRooms[j];
                    Vector2Int v1 = steeringVectors[i];
                    Vector2Int v2 = steeringVectors[j];

                    if (i != j && Room.CheckCollision(r1, r2))
                    {
                        roomsColliding = true;
                        //Determinamos donde colisionan
                        if (CheckLeftCollision(r1, r2))   //R1 por la izquierda de R2
                        {
                            steeringVectors[i] = new Vector2Int(steeringVectors[i].x - displacement, steeringVectors[i].y);
                            steeringVectors[j] = new Vector2Int(steeringVectors[j].x + displacement, steeringVectors[j].y);
                        }

                        if (CheckRightCollision(r1, r2))   //R1 por la derecha de R2
                        {
                            steeringVectors[i] = new Vector2Int(steeringVectors[i].x + displacement, steeringVectors[i].y);
                            steeringVectors[j] = new Vector2Int(steeringVectors[j].x - displacement, steeringVectors[j].y);
                        }

                        if (CheckUpCollision(r1, r2))   //R1 por arriba de R2
                        {
                            steeringVectors[i] = new Vector2Int(steeringVectors[i].x, steeringVectors[i].y - displacement);
                            steeringVectors[j] = new Vector2Int(steeringVectors[j].x, steeringVectors[j].y + displacement);
                        }

                        if (CheckDownCollision(r1, r2))   //R1 por debajo de R2
                        {
                            steeringVectors[i] = new Vector2Int(steeringVectors[i].x, steeringVectors[i].y + displacement);
                            steeringVectors[j] = new Vector2Int(steeringVectors[j].x, steeringVectors[j].y - displacement);
                        }

                        //Debug.Log("Calculado: " + v1.x + ", " + v1.y);
                    }
                }
            }

            for (int i = 0; i < lrRooms.Count; i++)
            {
                //Debug.Log("Steering: (" + steeringVectors[i].x + ", " + steeringVectors[i].y + ")");

                lrRooms[i].SetIndexPosition(lrRooms[i].GetIndexPosition() + steeringVectors[i]);
                steeringVectors[i] = new Vector2Int(0, 0);

                //Tenemos que asegurarnos de que la Room no se salga del mapa
                if (lrRooms[i].GetIndexPosition().x < 0)
                    lrRooms[i].SetIndexPosition(new Vector2Int(0, lrRooms[i].GetIndexPosition().y));

                if (lrRooms[i].GetIndexPosition().x + lrRooms[i].GetWidth() > mapSize.x)
                    lrRooms[i].SetIndexPosition(new Vector2Int(mapSize.x - lrRooms[i].GetWidth(), lrRooms[i].GetIndexPosition().y));

                if (lrRooms[i].GetIndexPosition().y < 0)
                    lrRooms[i].SetIndexPosition(new Vector2Int(lrRooms[i].GetIndexPosition().x, 0));

                if (lrRooms[i].GetIndexPosition().y + lrRooms[i].GetHeight() > mapSize.y)
                    lrRooms[i].SetIndexPosition(new Vector2Int(lrRooms[i].GetIndexPosition().x, mapSize.y - lrRooms[i].GetHeight()));
            }

            tries--;

            if (tries == 0)
            {
                Debug.Log("Se acabaron los intentos");
            }
        }

        return lrRooms;
    }

    private List<Vector2Int> SetupSteeringVectors(int iListSize)
    {
        List<Vector2Int> steeringVectors = new List<Vector2Int>();
        for(int i = 0; i < iListSize; i++)
        {
            steeringVectors.Add(new Vector2Int());
        }

        return steeringVectors;
    }

    private bool CheckLeftCollision(Room r1, Room r2)
    {
        for (int i = r1.GetIndexPosition().y; i < r1.GetIndexPosition().y + r1.GetHeight(); i++)
        {
            if (Room.ContainsPosition(r2, r1.GetIndexPosition().x + r1.GetWidth(), i))
            {
                return true;
            }
        }

        return false;
    }

    private bool CheckRightCollision(Room r1, Room r2)
    {
        for (int i = r1.GetIndexPosition().y; i < r1.GetIndexPosition().y + r1.GetHeight(); i++)
        {
            if (Room.ContainsPosition(r2, r1.GetIndexPosition().x, i))
            {
                return true;
            }
        }

        return false;
    }

    private bool CheckUpCollision(Room r1, Room r2)
    {
        for (int j = r1.GetIndexPosition().x; j < r1.GetIndexPosition().x + r1.GetWidth(); j++)
        {
            if (Room.ContainsPosition(r2, j, r1.GetIndexPosition().y + r1.GetHeight()))
            {
                return true;
            }
        }

        return false;
    }

    private bool CheckDownCollision(Room r1, Room r2)
    {
        for (int j = r1.GetIndexPosition().x; j < r1.GetIndexPosition().x + r1.GetWidth(); j++)
        {
            if (Room.ContainsPosition(r2, j, r1.GetIndexPosition().y))
            {
                return true;
            }
        }

        return false;
    }
}
