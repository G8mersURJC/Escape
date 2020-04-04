using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RenderingTestManager : MonoBehaviour
{
    public Actor player = new Actor();
    public GameObject goPlayer;

    private List<MapDataRT> mapList;       //Lista de todos los mapas en escena
    private int iActiveMap;

    void Start()
    {
        MapRendererRT mr = GetComponent<MapRendererRT>();

        mapList = new List<MapDataRT>();
        mapList.Add(mr.LoadAndRenderMapFromFile(0));
        mapList.Add(mr.LoadAndRenderMapFromFile(1));

        iActiveMap = 0;


        SpawnPlayer(mapList[iActiveMap].GetMapStart());
    }

    public void SpawnPlayer(Vector2Int vPos)
    {
        if (!player.GetActor())
        {
            player.SetActor(goPlayer);
            player.GetActor().tag = "Player";
            player.SetType(0);
            player.SetLife(6);
            player.SetActor(Instantiate(goPlayer, new Vector3(0, 0.5f, 0), Quaternion.identity));
        }
        player.GetActor().GetComponent<PlayerControlerRT>().SetCellPosition(vPos);

        player.GetActor().transform.SetParent(GameObject.Find("Map" + iActiveMap).transform);
        player.GetActor().transform.localPosition = new Vector3(vPos.x, player.GetActor().transform.localPosition.y, -vPos.y);
    }

    public bool CanWalkToCell(Vector2Int cellPos)
    {
        return mapList[iActiveMap].IsCellWalkable(cellPos);
    }


    //Cuando el jugador quiere atacar a la casilla que mira
    public void AttackInCell(Vector2Int cellPosition, int damage)
    {
        Actor enemy = mapList[iActiveMap].GetEnemyInCellIfThereIs(cellPosition);
        //Hay que comprobar si hay un enemigo en dicha casilla
        if (enemy != null)
        {
            //Reducimos la salud del enemigo
            Debug.Log("ME MORÍ NO MAS");

            mapList[iActiveMap].RemoveEnemyActorFromList(enemy);
            Destroy(enemy.GetActor());
        }
    }

    public void ProcessEnemyTurn()
    {
        //El jugador ya ha procesado su turno, le toca ahora a los enemigos
        foreach (Actor a in mapList[iActiveMap].GetEnemyList())
        {
            //Actualizar la IA del actor enemigo
            a.GetActor().GetComponent<EnemyControllerRT>().ProcessTurn();
        }


        player.GetActor().GetComponent<PlayerControlerRT>().SetActionAvailable(true);
    }
}
