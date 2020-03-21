using System.Collections;
using System.Collections.Generic;
using System.IO;
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
    int[,] map = new int[50,50];

    public void SetupMap()
    {




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

                    vPlayerSpawn.x = j;
                    vPlayerSpawn.y = i;
                }

                if(coordinates[i, j] != 9)
                {
                    cells[i, j].SetModel(goModelList[coordinates[i, j]]);
                    //cells[i, j].SetBehaviour();
                }
                
            }
            GameManager.manager.SpawnPlayer(new Vector2(vPlayerSpawn.x, vPlayerSpawn.y));
        }
    }

    public void LoadMap()
    {
    

        StreamReader reader = new StreamReader("Assets/data/map0.txt");
        for(int i=0; i<50;i++)
        {
            string sLine = reader.ReadLine();
            for(int j=0; j<50; j++)
            {
                map[i, j] = int.Parse(sLine[j].ToString());
            }
        }
        coordinates = map;
    }
}
