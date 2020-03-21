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
    public GameObject goExit;
    public GameObject goMap;
    public InputField ifPlayerName;
    public GameObject goHTP;

    RankingManager rManager = new RankingManager();
   
    public bool bPlay;

    public Actor player = new Actor();
    public Actor[] enemies;

    LifeHUDController HudController;

    int iActiveLvl;
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
       
        rManager.LoadRankings();
        iActiveLvl = 0;
        HudController.InitLHC();
        goMap.GetComponent<CellMapV2>().LoadMap();
        goMap.GetComponent<CellMapV2>().SetupMap();
    }

    // Update is called once per frame
    void Update()
    {
       
    }


    //Instancia y posiciona al jugador
    public void SpawnPlayer(Vector2 vPos)
    {
        player.SetActor(goPlayer);
        player.SetType(0);
        player.SetLife(6);
        HudController.SetMaxHearts(3);
        GameObject go =Instantiate(player.GetActor(), new Vector3(vPos.x, 0.5f, vPos.y), Quaternion.identity);
        go.GetComponent<PlayerController>().SetPos(vPos);
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

}
