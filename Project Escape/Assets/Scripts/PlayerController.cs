using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public int posx = 0;
    public int posz = 0;
    public float speed;
    public int currentDir;
    public int newDir;
    private CellMap controlador;
    private float currentDistance; 
    // Start is called before the first frame update
    void Start()
    {
        newDir = -1;
        speed = 1.0f;
        currentDir = -1;
        currentDistance = 0;
        controlador = GameObject.Find("CellContainer").GetComponent(typeof(CellMap)) as CellMap;
        if (!controlador) Debug.Log("No encontrado");
        ResetPos();
    }
    // Update is called once per frame
    void Update()
    {
        if (newDir==-1) {
            if (Input.GetKeyDown(KeyCode.W)) newDir = 0;
            if (Input.GetKeyDown(KeyCode.A)) newDir = 1;
            if (Input.GetKeyDown(KeyCode.S)) newDir = 2;
            if (Input.GetKeyDown(KeyCode.D)) newDir = 3;
            if (newDir>=0&&!canMoveTo(newDir)) newDir = -1;
        }
        else
        {
            rotateFacing(newDir);
            transform.Translate(Vector3.forward * speed * Time.deltaTime);
            currentDistance += speed * Time.deltaTime;
            if (currentDistance >= 1)
            {
                ResetPos();
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
        transform.position = new Vector3(posx - controlador.posz, 0.5f, controlador.posx - posz);
        newDir = -1;
        currentDistance = 0;
        Debug.Log("Estoy en: " + posx + " " + posz);
    }
}
