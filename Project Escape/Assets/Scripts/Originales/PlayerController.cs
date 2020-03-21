using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public int iPosx = 0;
    public int iPosy = 1;
    public int iPosz = 0;
    public float fSpeed;
    public int iCurrentDir;
    public int iNewDir;
    public float fTimer = 0;
    private CellMapV2 cmControlador;
    private float fCurrentDistance;
    private bool bIsAtacking = false;
    
    Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        iNewDir = -1;
        iCurrentDir = 0;
        fSpeed = 1.0f;
        fCurrentDistance = 0;
        bIsAtacking = false;
        cmControlador = GameObject.Find("CellContainer").GetComponent(typeof(CellMapV2)) as CellMapV2;
        if (!cmControlador) Debug.Log("Mapa no encontrado");
        animator = GetComponent<Animator>();
        if (!animator) Debug.Log("Animator no encontrado");
        ResetPos();
    }
    // Update is called once per frame
    void Update()
    {
        if (!bIsAtacking && iNewDir == -1)
        {
            if (Input.GetKey(KeyCode.E))
            {
                bIsAtacking = true;
                if (animator) animator.SetInteger("New Int", 2);
                iNewDir = -1;
                fTimer = 1.0f;
            }
        }

        if (fTimer > 0)
        {
            if (animator && animator.GetInteger("New Int") == 1) animator.SetInteger("New Int", 0);
            fTimer -= 1f * Time.deltaTime;
            //Debug.Log(fTimer);
        }
        else if (fTimer <= 0)
        {
            if (animator && animator.GetInteger("New Int") == 2) animator.SetInteger("New Int", 0);
            bIsAtacking = false;
        }

        if(!bIsAtacking && fTimer <= 0)
        {
            if (iNewDir == -1)
            {
                if (Input.GetKey(KeyCode.W)) iNewDir = 0;
                if (Input.GetKey(KeyCode.A)) iNewDir = 1;
                if (Input.GetKey(KeyCode.S)) iNewDir = 2;
                if (Input.GetKey(KeyCode.D)) iNewDir = 3;
                if (iNewDir >= 0 && (iNewDir == iCurrentDir) && !canMoveTo(iNewDir))
                {
                    iNewDir = -1;
                }
                if ((iNewDir == -1) && (animator != null)) animator.SetInteger("New Int", 0);
            }
            else if (iNewDir == iCurrentDir)
            {

                if (animator != null) animator.SetInteger("New Int", 1);
                transform.Translate(Vector3.forward * fSpeed * Time.deltaTime);
                fCurrentDistance += fSpeed * Time.deltaTime;
                if (fCurrentDistance >= 1)
                {
                    ResetPos();
                }
            }
            else
            {
                rotateFacing(iNewDir);
                iNewDir = -1;
                fTimer = 0.2f;
            }
        }
    }
    private bool canMoveTo(int c)
    {
        int[,] mapa = cmControlador.getMap();
        switch (c)
        {
            case 0://Arriba
                if (iPosz == 0) return false;

                if (mapa[iPosz - 1, iPosx] !=1 && mapa[iPosz - 1, iPosx] != 2 && mapa[iPosz - 1, iPosx] != 9)
                {
                    Debug.Log("Me muevo a " + mapa[iPosz - 1, iPosx]);
                    iPosz--;
                    return true;
                }
                break;
            case 1://Izquierda
                if (iPosx == 0) return false;

                if (mapa[iPosz, iPosx - 1] != 1 && mapa[iPosz, iPosx - 1] != 2 && mapa[iPosz, iPosx - 1] != 9)
                {
                    Debug.Log("Me muevo a " + mapa[iPosz - 1, iPosx]);
                    iPosx--;
                    return true;
                }
                break;
            case 2://Abajo
                if (iPosz == mapa.GetLength(0) - 1) return false;

                if (mapa[iPosz + 1, iPosx] != 1 && mapa[iPosz + 1, iPosx] != 2 && mapa[iPosz + 1, iPosx] != 9)
                {
                    Debug.Log("Me muevo a "+ mapa[iPosz - 1, iPosx]);
                    iPosz++;
                    return true;
                }
                break;

            case 3://Derecha
                if (iPosx == mapa.GetLength(1) - 1) return false;

                if (mapa[iPosz, iPosx + 1] != 1 && mapa[iPosz, iPosx + 1] != 2 && mapa[iPosz, iPosx + 1] != 9)
                {
                    Debug.Log("Me muevo a " + mapa[iPosz, iPosx + 1]);
                    iPosx++;
                    return true;
                }
                break;
        }
        return false;
    }
    private bool rotateFacing(int newDir)
    {
        if (newDir != iCurrentDir)
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
            iCurrentDir = newDir;
        }
        return false;
    }

    public void SetPos(Vector2 vPos)
    {
        iPosx = (int)vPos.x;
        iPosz = (int)vPos.y;
    }

    void ResetPos()
    {
        //Centra el movimiendo a la casilla.
        transform.position = new Vector3(iPosx - cmControlador.posz, iPosy -0.5f, cmControlador.posx - iPosz);
        iNewDir = -1;
        fCurrentDistance = 0;
        Debug.Log("Estoy en: " + iPosx + " " + iPosz);
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Goal")
            GameManager.manager.ExitGame();
    }
}
