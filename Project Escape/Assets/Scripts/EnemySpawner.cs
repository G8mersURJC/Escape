using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject[] goEnemies;
    private CellMap controlador;
    float timer;
    public float fTimeBetweenSpawns = 1;

    int posx, posz;

    private List<GameObject> goEnemyList;
    // Start is called before the first frame update
    void Start()
    {
        timer = 0;
        goEnemyList = new List<GameObject>();

        controlador = GameObject.Find("CellContainer").GetComponent(typeof(CellMap)) as CellMap;
        if (!controlador) Debug.Log("No encontrado");    
    }

    // Update is called once per frame
    void Update()
    {
        UpdateEnemies();
        SpawnNewEnemy();
    }

    private void SpawnNewEnemy()
    {
        timer += Time.deltaTime;
        if(timer > fTimeBetweenSpawns)
        {
            bool spawned = false;
            do
            {
                posx = Random.Range(0, 10);
                posz = Random.Range(0, 10);
                if (controlador.getMap()[posz, posx] == 0) {
                    Debug.Log("Spawned successfully " + "[" + posx + "," + posz + "]");
                    spawned = true;
                }
                

            } while (!spawned);

            GameObject go = Instantiate(goEnemies[0], new Vector3(posx, 0, posz), Quaternion.identity);
            go.transform.position = new Vector3(posx - controlador.posx, 2, controlador.posz - posz);
            goEnemyList.Add(go);

            timer = 0.0f;
        }
    }

    private void UpdateEnemies()
    {
        /**
        foreach (GameObject go in goEnemyList)
        {
            
        }
        //*/
    }
}
