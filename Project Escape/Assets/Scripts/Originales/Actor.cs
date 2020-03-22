using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ActorType
{
    acPlayer,
    acEnemy
}

public class Actor
{
    GameObject go;
    int iLife;
    float fSpeed;
    string sName;
    int iPoints = 0;
    ActorType type;

    public void SetActor(GameObject obj)
    {
        go = obj;
    }
    public GameObject GetActor()
    {
        return go;
    }
    public void SetName(string name)
    {
        sName = name;
    }
    public string GetName()
    {
        return sName;
    }

    public int GetPoints()
    {
        return iPoints;
    }

    public int AddPoints(int p)
    {
        iPoints += p;
        return iPoints;
    }

    public void SetType(ActorType t)
    {
        type = t;
    }

    public ActorType GetType()
    {
        return type;
    }



    ////Gestión de vida (GetDMG, Die,...)
    ///
    public void SetLife(int i)
    {
        iLife = i;
    }
    public int GetLife()
    {
        return iLife;
    }
    public int Damage(int iDmg)
    {
        iLife -= iDmg;
        GameManager.manager.HudController.ModifyHealthByAmount(iDmg * -1);
        CheckDeath();
        return iLife;
    }
    public int Heal(int iHP)
    {
        iLife += iHP;
        return iLife;
    }

    //Comprueba si la vida es menor a 0 y actua en consecuencia teniendo en cuenta qué tipo de actor es
    public void CheckDeath()
    {
        if(iLife<=0)
        {
            //go.GetComponent<Animator>().Play("Morirse");
            switch (type)
            {
                case ActorType.acPlayer:
                    GameManager.manager.bGameOver = true;
                    GameManager.manager.ResetGame(3);
                    break;
                case ActorType.acEnemy:
                    //==============
                    //TEMPORAL, CAMBIAR DESPUÉS A LA ANIMACIÓN DE MUERTE
                    //==============
                    GameManager.manager.RemoveEnemyActor(this);
                    break;
            }

        }
    }

    
}
