using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Video;

public class GameManager : MonoBehaviour
{
    //Singleton
    public static GameManager manager;
 
    //GameObjects
    public GameObject goPlayer;
    public GameObject goExit;
    public GameObject goMap;
    public GameObject goHTP, goLore, goGameOver;

    //Actores
    public Actor player = new Actor();


    //Variables de ranking
    public InputField ifPlayerName;
    RankingManager rManager = new RankingManager();
    public LifeHUDController HudController;

    
    //Variables de flujo de juego
    public bool bGameOver = false;
    int enemyTurnIndex;
    public bool bPlay = false;

    //Variables de mapas
    private List<MapDataV2> mapList;       //Lista de todos los mapas en escena
    private int iActiveMap;
    private int iMaxMaps = 0;

    //Awake<--------------------
    private void Awake()
    {
        if (manager == null)
            manager = this;
        else
            Destroy(this.gameObject);

        bPlay = false;
        HudController = GetComponent<LifeHUDController>();
    }

    // Start is called before the first frame update
    void Start()
    {
        MapRenderer mr = GetComponent<MapRenderer>();

        mapList = new List<MapDataV2>();
        mapList.Add(mr.LoadAndRenderMapFromFile(0));
        mapList.Add(mr.LoadAndRenderMapFromFile(1));

        rManager.sPath = Application.dataPath + "/Rankings.txt";
        rManager.LoadRankings();
        if(HudController)
            HudController.InitLHC();

       
        iMaxMaps++;
        iActiveMap = 0;

        SpawnPlayer(mapList[iActiveMap].GetMapStart());
     
    }

    // Update is called once per frame
    void Update()
    {
       
    }



    //---------------------------------------------------------------------------------------------------


    //CONTROL DE FLUJO DE JUEGO
    public void ExitGame()
    {
        rManager.AddRanking(player.GetName(), player.GetPoints());
        rManager.SaveRankings();
        Application.Quit();
    }

    public void ResetGame(float fSecs)
    {
        if(bGameOver)
        {
            bGameOver = false;
            goGameOver.SetActive(true);
        }
        rManager.AddRanking(player.GetName(), player.GetPoints());
        rManager.SaveRankings();

        
        StartCoroutine(DelayedLoading(fSecs, "MainMenuV2"));
        
    }

    public void LaunchGame()
    {
        SceneManager.LoadScene("Level1");
    }
    public void LaunchGame(string s1)
    {
        SceneManager.LoadScene(s1);
    }

    public void StartGameLvl1()
    {
        if(ifPlayerName.text =="")
            player.SetName("Anonymous");
        else
            player.SetName(ifPlayerName.text);
        bPlay = true;

        goHTP.SetActive(false);

        Debug.Log("Player name =" + player.GetName());
    }

    //Corrutina de carga retrasada en el tiempo
    IEnumerator DelayedLoading(float fSecs, string sScene)
    {
        yield return new WaitForSeconds(fSecs);
        goGameOver.SetActive(false);
        SceneManager.LoadScene(sScene);
    }


    //---------------------------------------------------------------------------------------------------

    //CONTROL DE MAPAS
    //Devuelve el mapa actual
    public int[,] GetCurrentMap()
    {
        return mapList[iActiveMap].GetMapArray();
    }
    
    //Devuelve la posibilidad de caminar a una casilla
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
            enemy.Damage(damage);
        }
    }

    //Elimina un enemigo de la lista
    public void RemoveEnemyActor(Actor a)
    {
        //Un enemigo ha muerto
        mapList[iActiveMap].RemoveEnemyActorFromList(a);
        Destroy(a.GetActor());

        //Sumamos puntos al jugador
        player.AddPoints(50);

        Debug.Log("USTED HA SIDO DESTRUIDO");
    }

   //Cambia de mapa
    public void SwitchMap()
    {
        if (iActiveMap >= iMaxMaps - 1)
        {
            iActiveMap = 0;
            ExitGame();
        }
        else
            iActiveMap++;

        SpawnPlayer(mapList[iActiveMap].GetMapStart());
    }

    //Ataca a un enemigo
    public void AttackPlayer(int damage)
    {
        //HudController.ModifyHealthByAmount(-damage);
        player.Damage(damage);

        
    }

    //Instancia y posiciona al jugador
    public void SpawnPlayer(Vector2Int vPos)
    {
        if (!player.GetActor())
        {
            player.SetActor(goPlayer);
            player.GetActor().tag = "Player";
            player.SetType(0);
            player.SetLife(6);
            HudController.SetMaxHearts(3);
            player.SetActor(Instantiate(goPlayer, new Vector3(vPos.x, 0.5f, vPos.y), Quaternion.identity));
        }
        player.GetActor().GetComponent<PlayerControllerV2>().SetCellPosition(vPos);

        player.GetActor().transform.SetParent(GameObject.Find("Map" + iActiveMap + "/MapEnemies").transform);
        player.GetActor().transform.localPosition = new Vector3(vPos.x, player.GetActor().transform.localPosition.y, -vPos.y);
    }

    //---------------------------------------------------------------------------------------------------

    //GESTIÓN DE TURNOS
    public void StartEnemyTurns()
    {
        enemyTurnIndex = 0;

        if (mapList[iActiveMap].GetEnemyList().Count > 0)
        {
            mapList[iActiveMap].GetEnemyList()[enemyTurnIndex].GetActor().GetComponent<EnemyControllerV2>().ProcessTurn();
        }
    }

    public void NextEnemyTurn()
    {
        if (player.GetLife() <= 0)
        {
            enemyTurnIndex = 0;
            return;
        }

        enemyTurnIndex++;

        if (enemyTurnIndex < mapList[0].GetEnemyList().Count)
        {
            mapList[iActiveMap].GetEnemyList()[enemyTurnIndex].GetActor().GetComponent<EnemyControllerV2>().ProcessTurn();
        }
        else
        {
            player.GetActor().GetComponent<PlayerControllerV2>().SetActionAvailable(true);
        }
    }


    //GESTIÓN DEL CANVAS
    public void CloseLore()
    {
        goLore.SetActive(false);
    }
}
