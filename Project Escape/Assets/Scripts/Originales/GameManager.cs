using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager manager;
    // Start is called before the first frame update

    public GameObject goPlayer;
    public GameObject goEnemy;
    public GameObject goExit;
    public GameObject goMap;
    public InputField ifPlayerName;
    public GameObject goHTP, goLore;

    RankingManager rManager = new RankingManager();
   
    public bool bPlay;

    public Actor player = new Actor();

    public LifeHUDController HudController;


    private List<MapData> mapList;       //Lista de todos los mapas en escena
    private int iActiveMap;

    private void Awake()
    {
        if (manager == null)
            manager = this;
        else
            Destroy(this.gameObject);

        bPlay = false;
        HudController = GetComponent<LifeHUDController>();
    }

    void Start()
    {
        mapList = new List<MapData>();

        rManager.LoadRankings();
        HudController.InitLHC();

        //Carga el primer nivel
        goMap.GetComponent<CellMapV2>().LoadMap(0);
        goMap.GetComponent<CellMapV2>().SetupMap();
        mapList.Add(goMap.GetComponent<CellMapV2>().GetCurrentMapData());

        goMap.GetComponent<CellMapV2>().LoadMap(1);
        goMap.GetComponent<CellMapV2>().SetupMap();
        mapList.Add(goMap.GetComponent<CellMapV2>().GetCurrentMapData());

        iActiveMap = 0;

        SpawnPlayer(mapList[iActiveMap].GetMapStart());
    }

    // Update is called once per frame
    void Update()
    {
       
    }


    //Instancia y posiciona al jugador
    public void SpawnPlayer(Vector2Int vPos)
    {
        player.SetActor(goPlayer);
        player.GetActor().tag = "Player";
        player.SetType(0);
        player.SetLife(6);
        HudController.SetMaxHearts(3);
        player.SetActor(Instantiate(goPlayer, new Vector3(vPos.x, 0.5f, vPos.y), Quaternion.identity));
        player.GetActor().GetComponent<PlayerController>().SetIndexPos(vPos);

        player.GetActor().transform.SetParent(GameObject.Find("Map" + iActiveMap + "/MapEnemies").transform);
        player.GetActor().transform.localPosition = new Vector3(vPos.x, player.GetActor().transform.localPosition.y, -vPos.y);
    }

    public void SpawnEnemy(Vector2 vPos)
    {
        //Los enemigos van a hacer Spawn después de generar los niveles. Hay que asignarlos a sus respectivas listas.

        Actor enem = new Actor();
        enem.SetActor(goEnemy);
        enem.SetType(ActorType.acEnemy);
        enem.SetLife(1);
        enem.SetActor(Instantiate(goEnemy, new Vector3(vPos.x, 0.5f, vPos.y), Quaternion.identity));

        PlaceEnemyInList(enem);

        //Añadimos el Actor del enemigo a la lista de enemigos de su nivel correspondiente
    }

    private void PlaceEnemyInList(Actor a)
    {
        iActiveMap = 0;
        foreach(MapData m in mapList)
        {
            //Añadimos el enemigo a la lista si en el nivel faltan por asignar
            if (m.GetEnemyList().Count < m.GetEnemyCount())
            {
                m.AddEnemy(a);

                Vector2Int pos = m.GetEnemyPositions()[m.GetEnemyList().Count - 1];

                //POSICIÓN EN EL TABLERO
                a.GetActor().GetComponent<EnemyController>().SetIndexPos(pos);
                a.GetActor().transform.SetParent(GameObject.Find("Map" + iActiveMap + "/MapEnemies").transform);

                //POSICIÓN LOCAL BAJO LA CAPA DE ENEMIGOS
                a.GetActor().transform.localPosition = new Vector3(pos.x, a.GetActor().transform.localPosition.y, -pos.y);

                iActiveMap = 0;
                return;
            }

            iActiveMap++;
        }

    }


    public void ExitGame()
    {
        rManager.AddRanking(player.GetName(), player.GetPoints());
        rManager.SaveRankings();
        Application.Quit();
    }

    public void ResetGame(float fSecs)
    {
        
        rManager.AddRanking(player.GetName(), player.GetPoints());
        rManager.SaveRankings();
        StartCoroutine(DelayedLoading(3, "Inicial"));
        
    }

    public void LaunchGame()
    {
        SceneManager.LoadScene("Level1");
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
        SceneManager.LoadScene(sScene);
    }

    //CONTROL DE MAPAS

    //Devuelve el mapa actual
    public int[,] GetCurrentMap()
    {
        return mapList[iActiveMap].GetMapArray();
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
            enemy.Damage(damage);
            enemy.CheckDeath();
        }
    }

    public void RemoveEnemyActor(Actor a)
    {
        //Un enemigo ha muerto
        mapList[iActiveMap].RemoveEnemyActorFromList(a);
        Destroy(a.GetActor());

        //Sumamos puntos al jugador
        player.AddPoints(50);

        Debug.Log("USTED HA SIDO DESTRUIDO");
    }

    public void ProcessEnemyTurn()
    {
        //El jugador ya ha procesado su turno, le toca ahora a los enemigos
        foreach(Actor a in mapList[iActiveMap].GetEnemyList())
        {
            //Actualizar la IA del actor enemigo
            a.GetActor().GetComponent<EnemyController>().ProcessTurn();
        }
    }

    public void CloseLore()
    {
        goLore.SetActive(false);
    }
}
