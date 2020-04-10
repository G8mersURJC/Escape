using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RenderingTestManager : MonoBehaviour
{
    public GameObject goContinuePanel;

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
            //En esta prueba, nos cargamos al enemigo directamente
            mapList[iActiveMap].RemoveEnemyActorFromList(enemy);
            Destroy(enemy.GetActor());
        }
    }

    int playerHP = 3;

    public void AttackPlayer(int damage)
    {
        //HudController.ModifyHealthByAmount(-iDmg);
        //player.Damage(damage);

        if(playerHP > 0)
        {
            playerHP -= damage;

            Debug.Log(playerHP);

            if (playerHP <= 0)
            {
                SetupContinueScreen();
            }
        }
    }

    //========================================================================================================
    //CONTROL DE TURNOS
    //========================================================================================================

    int enemyTurnIndex;

    public void StartEnemyTurns()
    {
        enemyTurnIndex = 0;

        if(mapList[0].GetEnemyList().Count > 0)
        {
            mapList[0].GetEnemyList()[enemyTurnIndex].GetActor().GetComponent<EnemyControllerRT>().ProcessTurn();
        }
    }

    public void NextEnemyTurn()
    {
        if(playerHP <= 0)
        {
            enemyTurnIndex = 0;
            return;
        }

        enemyTurnIndex++;

        if(enemyTurnIndex < mapList[0].GetEnemyList().Count)
        {
            mapList[0].GetEnemyList()[enemyTurnIndex].GetActor().GetComponent<EnemyControllerRT>().ProcessTurn();
        }
        else
        {
            player.GetActor().GetComponent<PlayerControlerRT>().SetActionAvailable(true);
        }
    }

    //========================================================================================================
    //CONTROL DE GAME OVER
    //========================================================================================================

    //Llamar cuando el jugador haya muerto
    public void SetupContinueScreen()
    {
        //Sustituir por la puntuación requerida para revivir
        if (true)
        {
            goContinuePanel.SetActive(true);
        }
        else
        {
            ProcessGameOver();
        }
    }

    public void ContinueButtonPressed()
    {
        goContinuePanel.SetActive(false);

        player.GetActor().GetComponent<PlayerControlerRT>().SetActionAvailable(true);
        //player.AddPoints(-250);
        //HudController.FillHealth();
        playerHP = 3;
    }

    public void SurrenderButtonPressed()
    {
        ProcessGameOver();
    }

    private void ProcessGameOver()
    {

    }
}
