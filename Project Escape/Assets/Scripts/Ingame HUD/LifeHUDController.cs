using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LifeHUDController : MonoBehaviour
{
    public GameObject goHearth, goHearthPanel;
    public Sprite sFullHearth, sHalfHearth, sEmptyHearth;
    public Vector3 v3HearthOffset;

    private int iCurrentMaxHealth = 6;
    private int iCurrentHealth;  //2 "toques" por corazón
    private int iMaxHealth = 10; //10 "toques" con 5 corazones

    public List<GameObject> lHearths;

    // Start is called before the first frame update
    void Start()
    {
        iCurrentHealth = iCurrentMaxHealth;

        lHearths = new List<GameObject>();

        v3HearthOffset += new Vector3(20, -20, 0);

        //Creamos tantos corazones como valor máximo actual tengamos
        for (int i = 0; i < iCurrentMaxHealth; i += 2)
        {
            GameObject newHearth = Instantiate(goHearth, new Vector3(), Quaternion.identity);
            newHearth.transform.SetParent(goHearthPanel.transform);
            newHearth.transform.localPosition = v3HearthOffset;
            lHearths.Add(newHearth);
            v3HearthOffset += new Vector3(60, 0, 0);
        }

        //Instantiate(myPrefab, new Vector3(0, 0, 0), Quaternion.identity);

        //Creamos tantos corazones como valor máximo actual tengamos
    }

    public void ExpandMaxHealth()
    {
        
        if (iCurrentMaxHealth < iMaxHealth)
        {
            iCurrentMaxHealth += 2;

            GameObject newHearth = Instantiate(goHearth, new Vector3(), Quaternion.identity);
            newHearth.transform.SetParent(goHearthPanel.transform);
            newHearth.transform.localPosition = v3HearthOffset;
            lHearths.Add(newHearth);
            v3HearthOffset += new Vector3(60, 0, 0);
        }

        iCurrentHealth = iCurrentMaxHealth;
        UpdateSprites();
    }

    public void ModifyHealthByAmount(int amount)
    {
        iCurrentHealth += amount;

        if (iCurrentHealth < 0)
        {
            iCurrentHealth = 0;
        }
        else if (iCurrentHealth > iCurrentMaxHealth)
        { 
            iCurrentHealth = iCurrentMaxHealth;
        }

        UpdateSprites();

        Debug.Log("Me quedan "+iCurrentHealth+" de" +iCurrentMaxHealth);
    } 

    private void UpdateSprites()
    {

        int count = 2;
        foreach(GameObject h in lHearths)
        {
            int dif = count - iCurrentHealth;

            if (dif <= 0)
            {
                //Corazón lleno
                h.GetComponent<Image>().sprite = sFullHearth;
            }
            else if(dif == 1)
            {
                //Corazón medio lleno
                h.GetComponent<Image>().sprite = sHalfHearth;
            }
            else if(dif > 1)
            {
                //Corazón vacio
                h.GetComponent<Image>().sprite = sEmptyHearth;
            }

            count += 2;
        }
    }

    
}
