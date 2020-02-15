﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public int posx;
    public int posz;
    public float speed;
    public int currentDir;
    public int newDir;
    private CellMap controlador;
    private float currentDistance; 
    // Start is called before the first frame update
    void Start()
    {
        posx = 0;
        posz = 0;
        newDir = -1;
        speed = 1.0f;
        currentDir = -1;
        currentDistance = 0;
        controlador = GameObject.Find("CellContainer").GetComponent(typeof(CellMap)) as CellMap;
        if (!controlador) Debug.Log("No encontrado");
    }
    // Update is called once per frame
    void Update()
    {
        if (newDir==-1) {
            if (Input.GetKey(KeyCode.W) && canMoveTo(0))
            {
                newDir = 0;                
            }
            if (Input.GetKey(KeyCode.A) && canMoveTo(1))
            {
                newDir = 1;

            }
            if (Input.GetKey(KeyCode.S) && canMoveTo(2))
            {
                newDir = 2;

            }
            if (Input.GetKey(KeyCode.D) && canMoveTo(3))
            {
                newDir = 3;
            }
        }
        else
        {
            rotateFacing(newDir);
            transform.Translate(Vector3.forward * speed * Time.deltaTime);
            currentDistance += speed * Time.deltaTime;
            if (currentDistance >= 1)
            {
                transform.position.Set(posx, 0, posz);
                newDir = -1;
                currentDistance = 0;
            }
        }
    }
    private bool canMoveTo(int c)
    {
        int[,] mapa = controlador.getMap();
        switch (c)
        {
            case 0://Arriba
                if (posz == 0)
                    return false;

                if (mapa[posx, --posz] == 0)
                    return true;

                break;
            case 1://Izquierda
                if (posx == 0)
                    return false;

                if (mapa[posx, --posx] == 0)
                    return true;
                break;
            case 2://Abajo
                if (posz == 9)
                    return false;

                if (mapa[posx, ++posz] == 0)
                    return true;
                break;

            case 3://Derecha
                if (posx == 9)
                    return false;

                if (mapa[posx, ++posx] == 0)
                    return true;
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
        
        /*
        switch (newDir - currentDir)
        {    
            case -1:
                currentDir = newDir;
                this.transform.Rotate(0, 90, 0, 0);
                break;
            case -2:
                currentDir = newDir;
                this.transform.Rotate(0, 180, 0, 0);
                break;
            case -3:
                currentDir = newDir;
                this.transform.Rotate(0, 270, 0, 0);
                break;
            case 1:
                currentDir = newDir;
                this.transform.Rotate(0, -90, 0, 0);
                break;
            case 2:
                currentDir = newDir;
                this.transform.Rotate(0, -180, 0, 0);
                break;
            case 3:
                currentDir = newDir;
                this.transform.Rotate(0, -270, 0, 0);
                break;
            default:
                break;
        }
        */

        return false;
    }
}
