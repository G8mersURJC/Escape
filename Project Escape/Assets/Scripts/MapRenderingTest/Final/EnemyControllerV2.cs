using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyControllerV2 : MonoBehaviour
{
    private Vector2Int v2iCellPosition;
    private float fWalkSpeed = 1.0f;

    private bool bWalkingForward = false;
    private float fTraveledDistance = 0;

    Animator animator;
    public float fTriggerColiderRadius;

    private bool bActivated = false;

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<SphereCollider>().radius = fTriggerColiderRadius;
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (bWalkingForward)
        {
            WalkForward();
        }
    }

    private void WalkForward()
    {
        transform.Translate(Vector3.forward * fWalkSpeed * Time.deltaTime);
        fTraveledDistance += fWalkSpeed * Time.deltaTime;

        if (fTraveledDistance > 1)
        {
            StopWalking();
        }
    }

    private void StopWalking()
    {
        transform.localPosition = new Vector3(v2iCellPosition.x, transform.position.y, -v2iCellPosition.y);
        bWalkingForward = false;

        //Activar animación Idle
        //animator.

        //Turno concluido
        EndTurn();
    }

    //LLAMAR DESDE GAMEMANAGER CUANDO SEA SU TURNO
    public void ProcessTurn()
    {
        if (!bActivated)
        {
            EndTurn();
            return;
        }

        Vector2Int playerPos = GameManager.manager.player.GetActor().GetComponent<PlayerControllerV2>().GetCellPosition();

        if (IsCloseEnoughToPosition(playerPos))
        {
            AttackPlayer(playerPos);
        }
        else
        {
            MoveTowardsPlayer(playerPos);
        }
    }

    private bool IsCloseEnoughToPosition(Vector2 pos)
    {
        int distance = (int) (Mathf.Abs(v2iCellPosition.x - pos.x) + Mathf.Abs(v2iCellPosition.y - pos.y));

        return distance == 1;   //Si la distancia es 1, significa que el jugador se encuentra en una casilla de al lado
    }

    private void AttackPlayer(Vector2Int playerPos)
    {
        RotateToFacePosition(playerPos);
        animator.Play("Punching");
        GameManager.manager.player.Damage(1);

        EndTurn();
    }

    private void RotateToFacePosition(Vector2Int position)
    {
        if (position.y < v2iCellPosition.y)
            this.transform.rotation = Quaternion.Euler(0, 0, 0);  //Arriba
        else if (position.x < v2iCellPosition.x)
            this.transform.rotation = Quaternion.Euler(0, -90, 0);  //Izquierda
        else if (position.y > v2iCellPosition.y)
            this.transform.rotation = Quaternion.Euler(0, 180, 0);    //Abajo
        else if (position.x > v2iCellPosition.x)
            this.transform.rotation = Quaternion.Euler(0, 90, 0);   //Derecha
    }

    private void MoveTowardsPlayer(Vector2Int playerPos)
    {
        Vector2Int direction = CalculateDirectionToMoveTowardsPosition(playerPos);

        if (CanMoveToPosition(v2iCellPosition + direction))
        {
            RotateToFacePosition(v2iCellPosition + direction);
            StartMovementToCell(direction);
        }
        else
        {
            EndTurn();
        }
    }

    private Vector2Int CalculateDirectionToMoveTowardsPosition(Vector2Int position)
    {
        List<Vector2Int> PossibleDirectionsToMove = new List<Vector2Int>();

        //Determinamos nuestra posición relativa con respecto a la Posición dada
        if (position.y < v2iCellPosition.y)
            PossibleDirectionsToMove.Add(new Vector2Int(0, -1));
        else if (position.y > v2iCellPosition.y)
            PossibleDirectionsToMove.Add(new Vector2Int(0, 1));

        if (position.x < v2iCellPosition.x)
            PossibleDirectionsToMove.Add(new Vector2Int(-1, 0));
        else if (position.x > v2iCellPosition.x)
            PossibleDirectionsToMove.Add(new Vector2Int(1, 0));

        //De las posibles opciones, seleccionamos aleatoriamente
        return PossibleDirectionsToMove[Random.Range(0, PossibleDirectionsToMove.Count)];
    }

    private bool CanMoveToPosition(Vector2Int cellPosition)
    {
        return GameManager.manager.CanWalkToCell(cellPosition);
    }

    private void StartMovementToCell(Vector2Int dir)
    {
        v2iCellPosition += dir;
        fTraveledDistance = 0;
        bWalkingForward = true;

        //Activar animación Walking
        //animator.
    }

    private void EndTurn()
    {
        //Notificamos a GameManager que ya he terminado mi turno
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            bActivated = true;     //En el momento que el jugador se ha acercado lo suficiente, se activa la IA
        }
    }

    public void SetCellPosition(Vector2Int position)
    {
        this.v2iCellPosition = position;
    }

    public Vector2Int GetCellPosition()
    {
        return this.v2iCellPosition;
    }
}
