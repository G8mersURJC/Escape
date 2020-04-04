using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControllerV2 : MonoBehaviour
{
    private Animator animator;

    private Vector2Int v2iCellPosition;
    private bool bNewActionAvailable = true;
    private float fWalkSpeed = 2.0f;

    //Tiempo pulsado necesario para moverte
    private bool bMovementPressedLongEnough = false;
    private float fPressTimeToMove = 0.3f;
    private float fTimePressedToMove = 0.0f;

    //Tiempo transcurrido desde el último input de movimiento (Sirve para reiniciar el timer de tiempo pulsado)
    private float fMaxWaitingTimeForMovement = 0.005f;
    private float fTimeSinceLastMovementInput = 0.005f;

    private bool bWalkingForward = false;
    private float fTraveledDistance = 0;

    void Start()
    {
        animator = this.GetComponent<Animator>();
    }

    void Update()
    {
        //====================================================
        //TEMPORAL: Colocar este bloque en Keybinds y llamar a dicho método a través de GameManager

        //Ejes de movimiento (W, A, S, D y Joystick de un mando)
        if (Input.GetAxis("Horizontal") != 0)
        {
            ProcessMovementTowardsDirection(new Vector2(Input.GetAxis("Horizontal"), 0));
        }
        else if (Input.GetAxis("Vertical") != 0)
        {
            ProcessMovementTowardsDirection(new Vector2(0, Input.GetAxis("Vertical") * -1));
        }
        //Ataque normal
        else if (Input.GetKeyDown(KeyCode.E) /* || Input.GetButtonDown("Attack") */)
        {
            ProcessNormalAtack();
        }
        //====================================================

        UpdateMovement();
    }

    //========================================================================================================
    //UPDATE MOVIMIENTO
    //========================================================================================================

    private void UpdateMovement()
    {
        if (bWalkingForward)
        {
            WalkForward();
        }
        else
        {
            UpdateMovementPressedTimer();   //Actualizamos el tiempo que llevamos pulsando

            UpdateMovementInputTimeOut();   //Actualizamos el tiempo 
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
        animator.SetInteger("New Int", 0);          //Activar animación Idle

        EndTurn();
    }

    private void UpdateMovementPressedTimer()
    {
        if (fTimeSinceLastMovementInput < fMaxWaitingTimeForMovement && !bMovementPressedLongEnough)
        {
            fTimePressedToMove += Time.deltaTime;
            if (fTimePressedToMove >= fPressTimeToMove)
            {
                //Hemos presionado durante el tiempo suficiente para poder movernos
                bMovementPressedLongEnough = true;
            }
        }
    }

    private void UpdateMovementInputTimeOut()
    {
        if (fTimeSinceLastMovementInput < fMaxWaitingTimeForMovement)
        {
            fTimeSinceLastMovementInput += Time.deltaTime;
        }
        else if (fTimeSinceLastMovementInput >= fMaxWaitingTimeForMovement)
        {
            //Hace tiempo que no tenemos entradas de movimiento, reseteamos el contador de tiempo pulsando
            //para movernos.

            fTimePressedToMove = 0.0f;
            bMovementPressedLongEnough = false;
        }
    }

    //========================================================================================================
    //ENTRADA MOVIMIENTO
    //========================================================================================================

    //LLAMAR DESDE KEYBINDS (A través de GameManager)
    public void ProcessMovementTowardsDirection(Vector2 direction)
    {
        if (!bNewActionAvailable)
            return;

        fTimeSinceLastMovementInput = 0.0f;

        Vector2Int parsedDirection = ParseVector(direction);    //Adaptamos las componentes del vector para que valgan -1, 0 o 1.

        RotateToFaceDirection(parsedDirection);

        if (bMovementPressedLongEnough && CanMoveToPosition(v2iCellPosition + parsedDirection))
        {
            StartMovementToCell(parsedDirection);
        }
    }

    private Vector2Int ParseVector(Vector2 v)
    {
        return new Vector2Int(NormalizeValue(v.x), NormalizeValue(v.y));
    }

    private int NormalizeValue(float value)
    {
        if (value > 0)
            return 1;
        else if (value < 0)
            return -1;

        return 0;
    }

    private void RotateToFaceDirection(Vector2Int direction)
    {
        if (direction.y == 1)
            this.transform.rotation = Quaternion.Euler(0, 180, 0);  //Arriba
        else if (direction.x == -1)
            this.transform.rotation = Quaternion.Euler(0, -90, 0);  //Izquierda
        else if (direction.y == -1)
            this.transform.rotation = Quaternion.Euler(0, 0, 0);    //Abajo
        else if (direction.x == 1)
            this.transform.rotation = Quaternion.Euler(0, 90, 0);   //Derecha
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
        bNewActionAvailable = false;

        //Activar animación Walking
        animator.SetInteger("New Int", 1);
    }

    //========================================================================================================
    //ATAQUE
    //========================================================================================================

    //LLAMAR DESDE KEYBINDS (A través de GameManager)
    public void ProcessNormalAtack()
    {
        if (!bNewActionAvailable)
            return;

        Vector3 TargetCell = transform.position + Vector3.forward;
        GameManager.manager.AttackInCell(new Vector2Int((int) TargetCell.x, (int) -TargetCell.z), 1);
        animator.SetInteger("New Int", 2);  //Activar animación de ataque

        bNewActionAvailable = false;

        EndTurn();
    }
    
    //LLAMAR DESDE GAMEMANAGER CUANDO VUELVA A SER NUESTRO TURNO
    public void SetActionAvailable(bool available)
    {
        this.bNewActionAvailable = available;
    }

    private void EndTurn()
    {
        //Notificamos a GameManager que ya he terminado mi turno
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
