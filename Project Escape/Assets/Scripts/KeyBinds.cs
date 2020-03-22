using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyBinds : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //Teclas genéricas del juego
        if (Input.GetKeyDown(KeyCode.Escape))
            GameManager.manager.ExitGame();

        //Pruebas
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            int p = GameManager.manager.player.AddPoints(1);
            Debug.Log("Player points: " + p);
        }
        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            GameManager.manager.SwitchMap();
            Debug.Log("Warping");
        }

    }
}
