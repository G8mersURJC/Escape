using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapDataRT
{
    private int[,] iiMapCells;
    private List<Actor> laEnemies;
    private Vector2Int v2iMapStart;
    private Vector2Int v2iMapEnd;

    public MapDataRT(int[,] mapCells, Vector2Int start_coordinates, Vector2Int end_coordinates, List<Actor> enemies)
    {
        this.iiMapCells = mapCells;
        this.v2iMapStart = start_coordinates;
        this.v2iMapEnd = end_coordinates;
        this.laEnemies = enemies;
    }

    public bool IsCellWalkable(Vector2Int cellPosition)
    {
        if (!IsValidIndex(cellPosition))
        {
            //Debug.Log("No puedes moverte fuera del mapa");
            return false;
        }

        if (IsPositionOcupiedByAnEnemy(cellPosition))
        {
            //Debug.Log("Hay un enemigo en esa casilla");
            return false;
        }

        if (IsPositionOcupiedByPlayer(cellPosition))
        {
            //Debug.Log("El jugador se encuentra en esa casilla");
            return false;
        }

        //Ahora miramos el tipo de casilla que es. Recordemos que las casillas no transitables son 1, 2 y 9.
        return (iiMapCells[cellPosition.y, cellPosition.x] != 1 && iiMapCells[cellPosition.y, cellPosition.x] != 2
            && iiMapCells[cellPosition.y, cellPosition.x] != 9);
    }

    private bool IsValidIndex(Vector2Int pos)
    {
        return !(pos.x < 0 || pos.x > iiMapCells.GetLength(1) - 1 || pos.y < 0 || pos.y > iiMapCells.GetLength(0) - 1);
    }

    private bool IsPositionOcupiedByAnEnemy(Vector2Int pos)
    {
        foreach (Actor a in laEnemies)
        {
            if (pos.x == a.GetActor().GetComponent<EnemyControllerRT>().GetCellPosition().x &&
                pos.y == a.GetActor().GetComponent<EnemyControllerRT>().GetCellPosition().y)
            {
                return true;
            }
        }

        return false;
    }

    private bool IsPositionOcupiedByPlayer(Vector2Int pos)
    {
        return (pos.x == GameObject.Find("Dios impostor").GetComponent<RenderingTestManager>().player.GetActor().GetComponent<PlayerControlerRT>().GetCellPosition().x &&
           pos.y == GameObject.Find("Dios impostor").GetComponent<RenderingTestManager>().player.GetActor().GetComponent<PlayerControlerRT>().GetCellPosition().y);
    }

    //Comprobar si hay un enemigo en cierta casilla, en caso de que haya uno, devolverlo
    public Actor GetEnemyInCellIfThereIs(Vector2Int cellPosition)
    {
        foreach (Actor a in laEnemies)
        {
            if (cellPosition.x == a.GetActor().GetComponent<EnemyControllerRT>().GetCellPosition().x &&
                cellPosition.y == a.GetActor().GetComponent<EnemyControllerRT>().GetCellPosition().y)
            {
                return a;
            }
        }

        return null;
    }

    public bool RemoveEnemyActorFromList(Actor a)
    {
        return laEnemies.Remove(a);
    }

    public int[,] GetMapArray()
    {
        return iiMapCells;
    }

    public void AddEnemy(Actor enemyActor)
    {
        this.laEnemies.Add(enemyActor);
    }

    public List<Actor> GetEnemyList()
    {
        return laEnemies;
    }

    public Vector2Int GetMapStart()
    {
        return v2iMapStart;
    }

    public Vector2Int GetMapExit()
    {
        return v2iMapEnd;
    }
}
