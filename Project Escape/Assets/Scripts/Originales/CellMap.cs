using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellMap : MonoBehaviour
{
    Vector2 vSize;
    private int[,] coordinates;
    private Cell [,] cells;
    public float posx;
    public float posz;
    public int iSizeX, iSizeY;
    public GameObject[] goModelList;
    private Vector2 vPlayerSpawn, vExit;



    // Start is called before the first frame update
    void Start()
    {
        cells = new Cell[iSizeX, iSizeY];
       
        
        /*
            0 -> Empty cell
            1 -> Cannot pass cell
            2 -> Can destroy cell
            3 -> Start
            4 -> End
        */
        
        //Suelo lógico
        posx = 4.5f;
        posz = 4.5f;
        coordinates = new int[10, 10] {
            {0, 0, 0, 2, 0, 0, 0, 1, 0, 4},
            {0, 1, 1, 1, 0, 1, 0, 1, 0, 1},
            {0, 0, 0, 1, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 1, 0, 1, 1, 0, 0, 0},
            {1, 0, 1, 1, 0, 1, 1, 0, 0, 0},
            {1, 0, 1, 1, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 1, 1, 1, 1, 1, 0, 2},
            {0, 0, 0, 0, 1, 0, 1, 1, 0, 0},
            {0, 3, 0, 0, 1, 0, 1, 1, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
        };
       

        //Establece el suelo para todos
    
        for (int i = 0; i < coordinates.GetLength(0); i++)
        {
            for (int j = 0; j < coordinates.GetLength(1); j++)
            {
                cells[i, j] = new Cell();
                cells[i, j].SetCellCode(coordinates[i, j]);
                cells[i, j].SetPos(new Vector2(j - posz, posx - i));
               
                if(coordinates[i,j] == 3)
                {
                   
                    vPlayerSpawn.x = j;
                    vPlayerSpawn.y = i;
                }
               
                cells[i, j].SetModel(goModelList[coordinates[i, j]]);
                cells[i, j].SetBehaviour();

            }
        }
        GameManager.manager.SpawnPlayer(vPlayerSpawn);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public int[,] getMap()
    {
        return coordinates;
    }
}
