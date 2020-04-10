using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyBinds : MonoBehaviour
{
    PlayerControllerV2 pc;
    // Start is called before the first frame update
    void Start()
    {
        if (!pc)
            pc = GameManager.manager.player.GetActor().GetComponent<PlayerControllerV2>();
    }

    // Update is called once per frame
    void Update()
    {
        //====================================================
        //TEMPORAL: Colocar este bloque en Keybinds y llamar a dicho método a través de GameManager

        //Ejes de movimiento (W, A, S, D y Joystick de un mando)
        if (Input.GetAxis("Horizontal") != 0)
        {
            pc.ProcessMovementTowardsDirection(new Vector2(Input.GetAxis("Horizontal"), 0));
        }
        else if (Input.GetAxis("Vertical") != 0)
        {
            pc.ProcessMovementTowardsDirection(new Vector2(0, Input.GetAxis("Vertical") * -1));
        }
        //Ataque normal
        else if (Input.GetKeyDown(KeyCode.E) /* || Input.GetButtonDown("Attack") */)
        {
            pc.ProcessNormalAtack();
        }


        //====================================================
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
