﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public int posx = 0;
    public int posy = 1;
    public int posz = 0;
    public float speed;
    public int currentDir;
    public int newDir;
    private CellMap controlador;
    private float currentDistance;
    public float pressedTime = 0;
    Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        newDir = -1;
        currentDir = 0;
        speed = 1.0f;
        currentDistance = 0;
        controlador = GameObject.Find("CellContainer").GetComponent(typeof(CellMap)) as CellMap;
        if (!controlador) Debug.Log("Mapa no encontrado");
        animator = GetComponent<Animator>();
        if (!animator) Debug.Log("Animator no encontrado");
        ResetPos();
    }
    // Update is called once per frame
    void Update()
    {
        if (pressedTime > 0)
        {
            if (animator) animator.SetInteger("New Int", 0);
            pressedTime -= 0.001f;
            Debug.Log(pressedTime);
        }
        else
        {
            if (newDir == -1)
            {
                if (Input.GetKey(KeyCode.W)) newDir = 0;
                if (Input.GetKey(KeyCode.A)) newDir = 1;
                if (Input.GetKey(KeyCode.S)) newDir = 2;
                if (Input.GetKey(KeyCode.D)) newDir = 3;
                if (newDir >= 0 && (newDir == currentDir) && !canMoveTo(newDir))
                {
                    newDir = -1;
                }
                if((newDir == -1) && (animator != null)) animator.SetInteger("New Int", 0);
            }
            else if (newDir == currentDir)
            {
                
                if(animator!=null) animator.SetInteger("New Int", 1);
                transform.Translate(Vector3.forward * speed * Time.deltaTime);
                currentDistance += speed * Time.deltaTime;
                if (currentDistance >= 1)
                {
                    ResetPos();
                }
            }
            else
            {
                rotateFacing(newDir);
                newDir = -1;
                pressedTime = 2 * Time.deltaTime;
            }
        }
    }
    private bool canMoveTo(int c)
    {
        int[,] mapa = controlador.getMap();
        switch (c)
        {
            case 0://Arriba
                if (posz == 0) return false;

                if (mapa[posz - 1, posx] !=1 && mapa[posz - 1, posx] != 2)
                {
                    posz--;
                    return true;
                }
                break;
            case 1://Izquierda
                if (posx == 0) return false;

                if (mapa[posz, posx - 1] != 1 && mapa[posz, posx - 1] != 2)
                {
                    posx--;
                    return true;
                }
                break;
            case 2://Abajo
                if (posz == 9) return false;

                if (mapa[posz + 1, posx] != 1 && mapa[posz + 1, posx] != 2)
                {
                    posz++;
                    return true;
                }
                break;

            case 3://Derecha
                if (posx == 9) return false;

                if (mapa[posz, posx + 1] != 1 && mapa[posz, posx + 1] != 2)
                {
                    posx++;
                    return true;
                }
                break;
        }
        return false;
    }
    private bool rotateFacing(int newDir)
    {
        if (newDir != currentDir)
        {
            switch (newDir)
            {
                case 0:
                    this.transform.rotation = Quaternion.Euler(0, 0, 0);
                    break;
                case 1:
                    this.transform.rotation = Quaternion.Euler(0, -90, 0);
                    break;
                case 2:
                    this.transform.rotation = Quaternion.Euler(0, 180, 0);
                    break;
                case 3:
                    this.transform.rotation = Quaternion.Euler(0, 90, 0);
                    break;
            }
            currentDir = newDir;
        }
        return false;
    }

    public void SetPos(Vector2 vPos)
    {
        posx = (int)vPos.x;
        posz = (int)vPos.y;
    }

    void ResetPos()
    {
        //Centra el movimiendo a la casilla.
        transform.position = new Vector3(posx - controlador.posz, posy, controlador.posx - posz);
        newDir = -1;
        currentDistance = 0;
        Debug.Log("Estoy en: " + posx + " " + posz);
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Goal")
            GameManager.manager.ExitGame();
    }
}
