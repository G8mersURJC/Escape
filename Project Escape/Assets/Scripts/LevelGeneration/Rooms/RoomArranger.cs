using UnityEngine;
using System.Collections.Generic;

public class RoomArranger
{
    public List<Room> ArrangeRooms(Vector2Int mapSize, List<Room> lrRooms)
    {
        int iDisplacement = 1;
        int iTries = 10;
        bool bRoomsColliding = true;

        List<Vector2Int> lv2iSteeringVectors = SetupSteeringVectors(lrRooms.Count);
        while (bRoomsColliding && iTries > 0)
        {
            bRoomsColliding = false;

            for (int i = 0; i < lrRooms.Count; i++)
            {
                for (int j = 0; j < lrRooms.Count; j++)
                {
                    Room rA = lrRooms[i];
                    Room rB = lrRooms[j];
                    Vector2Int v2iA = lv2iSteeringVectors[i];
                    Vector2Int v2iB = lv2iSteeringVectors[j];

                    if (i != j && Room.CheckColision(rA, rB))
                    {
                        bRoomsColliding = true;
                        //Determinamos donde colisionan
                        if (CheckLeftCollision(rA, rB))   //RA por la izquierda de RB
                        {
                            lv2iSteeringVectors[i] = new Vector2Int(lv2iSteeringVectors[i].x - iDisplacement, lv2iSteeringVectors[i].y);
                            lv2iSteeringVectors[j] = new Vector2Int(lv2iSteeringVectors[j].x + iDisplacement, lv2iSteeringVectors[j].y);
                        }

                        if (CheckRightCollision(rA, rB))   //RA por la derecha de RB
                        {
                            lv2iSteeringVectors[i] = new Vector2Int(lv2iSteeringVectors[i].x + iDisplacement, lv2iSteeringVectors[i].y);
                            lv2iSteeringVectors[j] = new Vector2Int(lv2iSteeringVectors[j].x - iDisplacement, lv2iSteeringVectors[j].y);
                        }

                        if (CheckUpCollision(rA, rB))   //RA por arriba de RB
                        {
                            lv2iSteeringVectors[i] = new Vector2Int(lv2iSteeringVectors[i].x, lv2iSteeringVectors[i].y - iDisplacement);
                            lv2iSteeringVectors[j] = new Vector2Int(lv2iSteeringVectors[j].x, lv2iSteeringVectors[j].y + iDisplacement);
                        }

                        if (CheckDownCollision(rA, rB))   //RA por debajo de RB
                        {
                            lv2iSteeringVectors[i] = new Vector2Int(lv2iSteeringVectors[i].x, lv2iSteeringVectors[i].y + iDisplacement);
                            lv2iSteeringVectors[j] = new Vector2Int(lv2iSteeringVectors[j].x, lv2iSteeringVectors[j].y - iDisplacement);
                        }
                    }
                }
            }

            for (int i = 0; i < lrRooms.Count; i++)
            {
                //Debug.Log("Steering: (" + steeringVectors[i].x + ", " + steeringVectors[i].y + ")");

                lrRooms[i].SetIndexPosition(lrRooms[i].GetIndexPosition() + lv2iSteeringVectors[i]);
                lv2iSteeringVectors[i] = new Vector2Int(0, 0);

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

            iTries--;

            if (iTries == 0)
            {
                //Debug.Log("Se acabaron los intentos de mover salas");
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

    private bool CheckLeftCollision(Room rA, Room rB)
    {
        for (int i = rA.GetIndexPosition().y; i < rA.GetIndexPosition().y + rA.GetHeight(); i++)
        {
            if (Room.ContainsPosition(rB, rA.GetIndexPosition().x + rA.GetWidth(), i))
            {
                return true;
            }
        }

        return false;
    }

    private bool CheckRightCollision(Room rA, Room rB)
    {
        for (int i = rA.GetIndexPosition().y; i < rA.GetIndexPosition().y + rA.GetHeight(); i++)
        {
            if (Room.ContainsPosition(rB, rA.GetIndexPosition().x, i))
            {
                return true;
            }
        }

        return false;
    }

    private bool CheckUpCollision(Room rA, Room rB)
    {
        for (int j = rA.GetIndexPosition().x; j < rA.GetIndexPosition().x + rA.GetWidth(); j++)
        {
            if (Room.ContainsPosition(rB, j, rA.GetIndexPosition().y + rA.GetHeight()))
            {
                return true;
            }
        }

        return false;
    }

    private bool CheckDownCollision(Room rA, Room rB)
    {
        for (int j = rA.GetIndexPosition().x; j < rA.GetIndexPosition().x + rA.GetWidth(); j++)
        {
            if (Room.ContainsPosition(rB, j, rA.GetIndexPosition().y))
            {
                return true;
            }
        }

        return false;
    }
}
