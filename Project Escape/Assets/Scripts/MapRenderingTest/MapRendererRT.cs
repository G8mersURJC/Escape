using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class MapRendererRT : MonoBehaviour
{
    public GameObject[] goModelList;
    public GameObject[] goEnemiesList;

    public MapDataRT LoadAndRenderMapFromFile(int mapId)
    {
        int[,] mapCoordinates = LoadMapFromFileId(mapId);

        return RenderAndSetupMap(mapCoordinates, mapId);
    }

    private int[,] LoadMapFromFileId(int number)
    {
        int[,] map = new int[50, 50];

        StreamReader reader = new StreamReader("Assets/data/map" + number + ".txt");
        for (int i = 0; i < 50; i++)
        {
            string sLine = reader.ReadLine();
            for (int j = 0; j < 50; j++)
            {
                map[i, j] = int.Parse(sLine[j].ToString());
            }
        }

        reader.Close();

        return map;
    }

    private MapDataRT RenderAndSetupMap(int[,] mapArray, int mapId)
    {
        Vector2Int vPlayerSpawn = new Vector2Int();
        Vector2Int vLevelExit = new Vector2Int();
        List<Actor> enemies = new List<Actor>();

        for (int i = 0; i < mapArray.GetLength(0); i++)
        {
            for (int j = 0; j < mapArray.GetLength(1); j++)
            {
                //Spawn del jugador
                if (mapArray[i, j] == 3)
                {
                    vPlayerSpawn.x = j;
                    vPlayerSpawn.y = i;
                }

                //Salida del nivel
                if (mapArray[i, j] == 4)
                {
                    vLevelExit.x = j;
                    vLevelExit.y = i;
                }

                //Spawn de un enemigo
                if (mapArray[i, j] == 6)
                {
                    Actor a = new Actor();
                    a.SetActor(InstanceEnemyInCell(i, j, mapId));
                    a.SetLife(1);

                    enemies.Add(a);
                }

                GameObject goCellObject = Instantiate(goModelList[mapArray[i, j]], new Vector3(), Quaternion.identity);
                goCellObject.transform.SetParent(GameObject.Find("Map" + mapId + "/MapCells").transform);
                goCellObject.transform.localPosition = new Vector3(j, 0, -i);
            }
        }

        return new MapDataRT(mapArray, vPlayerSpawn, vLevelExit, enemies);
    }

    private GameObject InstanceEnemyInCell(int i, int j, int mapId)
    {
        GameObject enemyObject = Instantiate(goEnemiesList[0], new Vector3(), Quaternion.identity);

        enemyObject.GetComponent<EnemyControllerRT>().SetCellPosition(new Vector2Int(j, i));
        enemyObject.transform.SetParent(GameObject.Find("Map" + mapId + "/MapEnemies").transform);
        enemyObject.transform.localPosition = new Vector3(j, 0.5f, -i);

        return enemyObject;
    }
}