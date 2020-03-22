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
    private Vector2Int vPlayerSpawn, vExit;

    private MapData mdCurrentMap;
    int mapId;

    public void SetupMap()
    {
        //PrintArray();

        cells = new CellV2[coordinates.GetLength(0), coordinates.GetLength(1)];

        //Necesario para regular la IA
        int enemyCounter = 0;
        List<Vector2Int> enemySpawnPositions = new List<Vector2Int>();

        for (int i = 0; i < coordinates.GetLength(0); i++)
        {
            for (int j = 0; j < coordinates.GetLength(1); j++)
            {
                cells[i, j] = new CellV2();
                cells[i, j].SetCellCode(coordinates[i, j]);
                cells[i, j].SetPos(new Vector2(j - posz, posx - i));
                
                //Spawn
                if (coordinates[i, j] == 3)
                {
                    vPlayerSpawn.x = j;
                    vPlayerSpawn.y = i;
                }

                //Salida del nivel
                if (coordinates[i, j] == 4)
                {
                    vExit.x = j;
                    vExit.y = i;
                }

                //Enemigo
                if (coordinates[i, j] == 6)
                {
                    enemyCounter++;
                    enemySpawnPositions.Add(new Vector2Int(j, i));
                }

                //if (coordinates[i, j] != 9)
                //{
                    cells[i, j].SetModel(goModelList[coordinates[i, j]+(mapId*10)], mapId);
                    //cells[i, j].SetBehaviour();
               // }
            }
            
        }

        Debug.Log("Contamos "+enemyCounter+" enemigos");

        //Montamos el objeto MapData con los datos leidos
        mdCurrentMap = new MapData(coordinates, vPlayerSpawn, vExit, enemyCounter, enemySpawnPositions);
    }

    public void LoadMap(int number)
    {
        //Nos cargamos lo que haya guardado previamente
        int[,] map = new int[50, 50];

        mapId = number;
        StreamReader reader = new StreamReader("Assets/data/map"+number+".txt");
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

    public MapData GetCurrentMapData()
    {
        return mdCurrentMap;
    }
}
