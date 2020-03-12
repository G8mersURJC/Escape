using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RoomPlacer
{
    public List<Room> GenerateRoomsInMap(Vector2Int v2iMapSize, int iRoomCount, int iMinRoomSize, int iMaxRoomSize)
    {
        List<Room> lRoomList = new List<Room>();

        for (int i = 0; i < iRoomCount; i++)
        {
            int width = Random.Range(iMinRoomSize, iMaxRoomSize);
            int height = Random.Range(iMinRoomSize, iMaxRoomSize);

            Room newRoom = new Room(i, new Vector2Int(Random.Range(0, v2iMapSize.x - width), Random.Range(0, v2iMapSize.y - height)), width, height);

            //Debug.Log("Room " + i + " en " + newRoom.GetIndexPosition().x + ", " + newRoom.GetIndexPosition().y);
            lRoomList.Add(newRoom);
        }

        return lRoomList;
    }
}
