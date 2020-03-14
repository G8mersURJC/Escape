using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager manager;
    // Start is called before the first frame update

    public GameObject goPlayer;
    public GameObject goExit;
    
    public GameObject goMap;
    private void Awake()
    {
        if (manager == null)
            manager = this;
        else
            Destroy(this.gameObject);


    }
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        //Teclas genéricas del juego
        if (Input.GetKeyDown(KeyCode.Escape))
            ExitGame();
    }


    //Instancia y posiciona al jugador
    public void SpawnPlayer(Vector2 vPos)
    {
        GameObject go =Instantiate(goPlayer, new Vector3(vPos.x, 1.0f, vPos.y), Quaternion.identity);
        go.GetComponent<PlayerController>().SetPos(vPos);
    }

    public void ExitGame()
    {
        Application.Quit();
    }

   

    public void LaunchGame()
    {
        SceneManager.LoadScene("Level1");
    }

}
