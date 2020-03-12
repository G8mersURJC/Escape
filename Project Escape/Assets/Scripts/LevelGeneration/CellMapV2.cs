using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellMapV2 : MonoBehaviour
{
    Vector2 vSize;
    private int[,] coordinates;
    private CellV2[,] cells;
    public float posx;
    public float posz;
    public int iSizeX, iSizeY;
    public GameObject[] goModelList;
    private Vector2 vPlayerSpawn, vExit;

    public void SetupMap(int [,] map)
    {
        coordinates = map;

        //PrintArray();

        cells = new CellV2[coordinates.GetLength(0), coordinates.GetLength(1)];

        for (int i = 0; i < coordinates.GetLength(0); i++)
        {
            for (int j = 0; j < coordinates.GetLength(1); j++)
            {
                cells[i, j] = new CellV2();
                //cells[i, j] = new Cell();
                cells[i, j].SetCellCode(coordinates[i, j]);
                cells[i, j].SetPos(new Vector2(j - posz, posx - i));

                if (coordinates[i, j] == 3)
                {

                    //vPlayerSpawn.x = j;
                    //vPlayerSpawn.y = i;
                }

                if(coordinates[i, j] != -1)
                {
                    cells[i, j].SetModel(goModelList[coordinates[i, j]]);
                    //cells[i, j].SetBehaviour();
                }


            }
        }
    }
}
