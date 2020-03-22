using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class MapData
{
    //Clase que va a contener datos relevantes de un nivel concreto
    private int[,] iiMapCells;
    private int iEnemyCount;    //Necesitamos saber cuantos enemigos hay en el mapa
    private List<Actor> laEnemies;
    private List<Vector2Int> lv2iEnemyPositions;
    private Vector2Int v2iMapStart;  //Posición de spawn
    private Vector2Int v2iMapEnd;   //Posición de salida


    public MapData(int[,] mapCells, Vector2Int start_coordinates, Vector2Int end_coordinates, int enemies_count, List<Vector2Int> enemy_spawn_positions)
    {
        this.iiMapCells = mapCells;
        this.v2iMapStart = start_coordinates;
        this.v2iMapEnd = end_coordinates;
        this.iEnemyCount = enemies_count;

        laEnemies = new List<Actor>();
        lv2iEnemyPositions = enemy_spawn_positions;
    }

    //Método llamado por los actores que quieren desplazarse hacia una posición
    public bool IsCellWalkable(Vector2Int cellPosition)
    {
        //Comprobamos que se trate de una posición válida
        if (!IsValidIndex(cellPosition))
        {
            //Debug.Log("No puedes moverte fuera del mapa");
            return false;
        }

        //Comprobamos si hay enemigos en esa casilla
        if (IsPositionOcupiedByAnEnemy(cellPosition))
        {
            //Debug.Log("Hay un enemigo en esa casilla");
            return false;
        }

        //Comprobamos si el jugador se encuentra en esa casilla
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
            if (pos.x == a.GetActor().GetComponent<EnemyController>().GetIndexPosition().x &&
                pos.y == a.GetActor().GetComponent<EnemyController>().GetIndexPosition().y)
            {
                return true;
            }
        }

        return false;
    }

    private bool IsPositionOcupiedByPlayer(Vector2Int pos)
    {
        return (pos.x == GameManager.manager.player.GetActor().GetComponent<PlayerController>().GetIndexPosition().x &&
            pos.y == GameManager.manager.player.GetActor().GetComponent<PlayerController>().GetIndexPosition().y);
    }

    //Comprobar si hay un enemigo en cierta casilla, en caso de que haya uno, devolverlo
    public Actor GetEnemyInCellIfThereIs(Vector2Int cellPosition)
    {
        foreach(Actor a in laEnemies)
        {
            if (cellPosition.x == a.GetActor().GetComponent<EnemyController>().GetIndexPosition().x &&
                cellPosition.y == a.GetActor().GetComponent<EnemyController>().GetIndexPosition().y)
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

    public List<Vector2Int> GetEnemyPositions()
    {
        return lv2iEnemyPositions;
    }

    public int GetEnemyCount()
    {
        return iEnemyCount;
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