using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

struct PlayerData
{
    public string sName;
    public int iPoints;
}
public class RankingManager
{
    PlayerData[] playerData = new PlayerData[10];


    string sPath = "Assets/Data/Rankings.txt";
    int iAmntRankings = 0;


    //Carga de los rankings de fichero
    public void LoadRankings()
    {
        try
        {
            if (File.Exists(sPath))
            {
                StreamReader reader = new StreamReader(sPath);
                string sReaded;
                while (iAmntRankings < 10 && reader.Peek() >= 0)
                {
                    sReaded = reader.ReadLine();
                    string[] s1 = sReaded.Split('-');
                    playerData[iAmntRankings].sName = s1[0];
                    playerData[iAmntRankings].iPoints = int.Parse(s1[1]);
                    iAmntRankings++;
                }

                reader.Close();
                //Ordenación de los rankings
                SortRanking();

            }
           

        }
        catch(System.Exception e)
       
        {
            Debug.Log("Error, no se ha encontrado el archivo de rankings");
        }



    }
    //Añade un ranking a la tabla de rankings
    public void AddRanking(string sName, int iPoints)
    {
        //Si cabe
        if(iAmntRankings <10)
        {
            playerData[9].sName = sName;
            playerData[9].iPoints = iPoints;
          
        }
        //Si no cabe, se borra la de menor puntuación
        else
        {
            int iPos = 0;
            int iMaxPoint = playerData[0].iPoints;
            for(int i=1; i< 10; i++)
            {
                if(playerData[i].iPoints < iMaxPoint)
                {
                    iMaxPoint = playerData[i].iPoints;
                    iPos = i;
                }
            }
            playerData[iPos].iPoints = iPoints;
            playerData[iPos].sName = sName;

        }

        //Sea como sea, se ordena
        SortRanking();


    }


    //Función que ordena la tabla de rankings por técnica de la burbuja
    void SortRanking()
    {
        for (int j = 0; j < 9; j++)
            for (int k = 0; k < 9; k++)
            {
                if (playerData[k].iPoints < playerData[k + 1].iPoints)
                {
                    PlayerData swap = playerData[k];
                    playerData[k] = playerData[k + 1];
                    playerData[k + 1] = swap;
                }

            }
    }


    //Salva los rankings en el archivo
    public void SaveRankings()
    {

        try
        {
            if(File.Exists(sPath))
            {
                File.Delete(sPath);
            }
            StreamWriter writer = new StreamWriter(sPath, true);

            for(int i=0; i< 10;i++)
            {
                writer.WriteLine(playerData[i].sName + "-" + playerData[i].iPoints.ToString());
            }
            writer.Close();
        }
        catch (System.Exception e)
        {
            Debug.LogError("Error, no se ha podido escribir el archivo de rankings");
        }
        

       
    }

}
