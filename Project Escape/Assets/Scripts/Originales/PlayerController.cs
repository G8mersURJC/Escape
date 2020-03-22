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
        fSpeed = 3.0f;
        fCurrentDistance = 0;
        bIsAtacking = false;
        cmControlador = GameObject.Find("CellContainer").GetComponent(typeof(CellMapV2)) as CellMapV2;
        if (!cmControlador) Debug.Log("Mapa no encontrado");
        animator = GetComponent<Animator>();
        if (!animator) Debug.Log("Animator no encontrado");
        ResetPos();
    }

    private void ProcessAtack()
    {
        //Hay que mirar si en la casilla que estamos mirando hay un enemigo
        switch (iCurrentDir)
        {
            case 0:
                GameManager.manager.AttackInCell(new Vector2Int(iPosx, iPosz - 1), 1);
                break;
            case 1:
                GameManager.manager.AttackInCell(new Vector2Int(iPosx - 1, iPosz), 1);
                break;
            case 2:
                GameManager.manager.AttackInCell(new Vector2Int(iPosx, iPosz + 1), 1);
                break;
            case 3:
                GameManager.manager.AttackInCell(new Vector2Int(iPosx + 1, iPosz), 1);
                break;
        }

        //Hemos atacado, le toca a los enemigos
        GameManager.manager.ProcessEnemyTurn();

        //Debug.Log("TURN ENDO!");
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

                //Procesar ataque
                ProcessAtack();
            }
        }

        if (fTimer > 0)
        {
            if (animator && animator.GetInteger("New Int") == 1) animator.SetInteger("New Int", 0);
            fTimer -= 1f * Time.deltaTime;
            //Debug.Log(fTimer);
        }
        else if (fTimer < 0)
        {
            fTimer = 0;

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
                if (iNewDir >= 0 && (iNewDir == iCurrentDir) && !CanMoveTo(iNewDir))
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
                    //Hemos terminado de movernos, le toca a los enemigos
                    GameManager.manager.ProcessEnemyTurn();

                    //Debug.Log("TURN ENDO!");
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
    private bool CanMoveTo(int c)
    {
        //int[,] mapa = cmControlador.getMap();

        //Pedimos a Dios el mapa activo
        int[,] mapa = GameManager.manager.GetCurrentMap();

        switch (c)
        {
            case 0://Arriba
                if (GameManager.manager.CanWalkToCell(new Vector2Int(iPosx, iPosz - 1)))
                {
                    iPosz--;
                    return true;
                }
                break;
            case 1://Izquierda
                if (GameManager.manager.CanWalkToCell(new Vector2Int(iPosx - 1, iPosz)))
                {
                    iPosx--;
                    return true;
                }
                break;
            case 2://Abajo
                if (GameManager.manager.CanWalkToCell(new Vector2Int(iPosx, iPosz + 1)))
                {
                    iPosz++;
                    return true;
                }
                break;
            case 3://Derecha
                if (GameManager.manager.CanWalkToCell(new Vector2Int(iPosx + 1, iPosz)))
                {
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

    public void SetIndexPos(Vector2 vPos)
    {
        iPosx = (int)vPos.x;
        iPosz = (int)vPos.y;
    }

    void ResetPos()
    {
        /*
         * CUIDAO AHI!
         * ESTO VA A DAR PROBLEMAS CUANDO CAMBIEMOS DE MAPA
         */

        //Centra el movimiendo a la casilla.

        transform.localPosition = new Vector3(iPosx, transform.position.y, -iPosz);

        //transform.position = new Vector3(iPosx - cmControlador.posz, iPosy -0.5f, cmControlador.posx - iPosz);
        iNewDir = -1;
        fCurrentDistance = 0;
        //Debug.Log("Estoy en: " + iPosx + " " + iPosz);
    }

    public Vector2 GetIndexPosition()
    {
        return new Vector2(iPosx, iPosz);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Goal")
            GameManager.manager.ExitGame();
    }
}
