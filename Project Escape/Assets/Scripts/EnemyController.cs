using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public int iPosx = 0;
    public int iPosz = 0;
    public float fSpeed;
    public int iCurrentDir;
    public int iNewDir;
    public float fTimer = 0;
    private CellMapV2 cmControlador;
    private float fCurrentDistance;
    private bool bIsAtacking = false;

    Animator animator;

    bool aiActivated = false;

    public float fRad;
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<SphereCollider>().radius = fRad;
        animator = GetComponent<Animator>();

        iNewDir = -1;
        iCurrentDir = 0;
        fSpeed = 3.0f;
        fCurrentDistance = 0;
        bIsAtacking = false;
    }

    // Update is called once per frame
    void Update()
    {
        
        if (iNewDir == iCurrentDir)
        {
            transform.Translate(Vector3.forward * fSpeed * Time.deltaTime);
            fCurrentDistance += fSpeed * Time.deltaTime;

            if (fCurrentDistance >= 1)
            {
                Debug.Log("Ya vale");
                ResetPos();
            }
        }
        
    }

    public void SetIndexPos(Vector2 vPos)
    {
        iPosx = (int)vPos.x;
        iPosz = (int)vPos.y;
    }


    public void ProcessTurn()
    {
        if (!aiActivated)
            return;

        //Se supone que ya nos podemos mover
        Vector2 playerPos = GameManager.manager.player.GetActor().GetComponent<PlayerController>().GetIndexPosition();

        if (IsCloseEnoughToPosition(playerPos))
        {
            //Si el jugador se encuentra lo bastante cerca, le podemos atacar

            //1. Tenemos que mirar hacia el jugador
            if (playerPos.y < iPosz)
            {
                iNewDir = 0;
            }
            else if (playerPos.y > iPosz)
            {
                iNewDir = 2;
            }
            else if (playerPos.x < iPosx)
            {
                iNewDir = 1;
            }
            else if (playerPos.x > iPosx)
            {
                iNewDir = 3;
            }

            RotateFacingDirection(iNewDir);

            //2. Hay que activar la animación de golpear
            animator.Play("Punching");
            Debug.Log("PUM, EN LA BOQUITA!");
            //3. Hay que hacer damages al jugador

            iNewDir = -1;
        }
        else
        {
            //Tenemos que movernos hacia el jugador

            //1. Determinamos hacia donde nos movemos para acercarnos
            int chosenDirectionToMove = CalculateDirectionToMoveTowardsPosition(playerPos);

            iNewDir = chosenDirectionToMove;

            //2. Procedemos a movernos hacia la casilla elegida (si podemos)
            if (!CanMoveTo(iNewDir))
            {
                iNewDir = -1;
            }
            else
            {
                RotateFacingDirection(iNewDir);
                Debug.Log("ME MUEVO");
            }
        }
        
    }

    private bool IsCloseEnoughToPosition(Vector2 pos)
    {
        int distance = (int) (Mathf.Abs(iPosx - pos.x) + Mathf.Abs(iPosz - pos.y));

        return distance == 1;   //Si la distancia es 1, significa que el jugador se encuentra en una casilla de al lado
    }

    private int CalculateDirectionToMoveTowardsPosition(Vector2 pos)
    {
        List<int> directions = new List<int>();

        //Determinamos nuestra posición relativa con respecto a la Posición dada
        if (pos.y < iPosz)
        {
            //Estamos por debajo
            directions.Add(0);
        }
        else if (pos.y > iPosz)
        {
            //Estamos por encima
            directions.Add(2);
        }

        if (pos.x < iPosx)
        {
            //Estamos a la derecha
            directions.Add(1);
        }
        else if (pos.x > iPosx)
        {
            //Estamos a la izquierda
            directions.Add(3);
        }

        //De las posibles opciones, seleccionamos aleatoriamente
        int chosenDirection = directions[Random.Range(0, directions.Count)];
       
        return chosenDirection;
    }

    //Literalmente el mismo procedimiento que en el jugador
    private bool CanMoveTo(int c)
    {
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

    private bool RotateFacingDirection(int newDir)
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

    public Vector2 GetIndexPosition()
    {
        return new Vector2(iPosx, iPosz);
    }

    void ResetPos()
    {
        /*
         * CUIDAO AHI!
         * ESTO VA A DAR PROBLEMAS CUANDO CAMBIEMOS DE MAPA
         */

        //Centra el movimiendo a la casilla.
        transform.localPosition = new Vector3(iPosx, transform.position.y, -iPosz);

        //transform.position = new Vector3(iPosx, transform.position.y, iPosz);
        iNewDir = -1;
        fCurrentDistance = 0;
        //Debug.Log("Estoy en: " + iPosx + " " + iPosz);
    }

    //EVENTOS DE COLISIÓN
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            //En el momento que el jugador se ha acercado lo suficiente, se activa la IA
            aiActivated = true;
        }
            
    }

    
}
