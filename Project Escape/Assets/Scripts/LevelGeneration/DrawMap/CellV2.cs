using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellV2 : MonoBehaviour
{
    GameObject obj;
    GameObject asset;
    Vector2 vPos;
    Vector2 vCenter;
    float fSize = 1;
    int iCellCode;

    Renderer rend;

    private void Awake()
    {
        //rend = obj.GetComponent<Renderer>();
    }
    // Start is called before the first frame update
    void Start()
    {
        vCenter = new Vector2(vPos.x + (fSize / 2), vPos.y + (fSize / 2));
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetPos(Vector2 vPos)
    {
        this.vPos = vPos;
    }
    public void SetPos(int iX, int iY)
    {
        vPos.x = iX;
        vPos.y = iY;

        vCenter.x = vPos.x + (fSize / 2);
        vCenter.y = vPos.y + (fSize / 2);

    }

    public  Vector2 GetCenter() 
    {
        return vCenter;
    }

    public void SetCellCode(int iCode)
    {
        iCellCode = iCode;
    }

    /*
    public void SetBehaviour()
    {
        switch (iCellCode)
        {
            case 0:
                rend.material.color = Color.white;
                break;
            case 1:
                rend.material.color = Color.red;
                break;
            case 2:
                rend.material.color = Color.yellow;
                break;
            case 3:
                rend.material.color = Color.blue;
                break;
            case 4:
                rend.material.color = Color.magenta;          
                GameManager.manager.goExit.transform.position = obj.transform.position;

                break;
            case 5: //Just in order to debug
                rend.material.color = Color.black;
                break;
        }
    }
    */
    public void SetModel(GameObject prefab)
    {
        obj = Instantiate(prefab, new Vector3(vPos.x, 0, vPos.y), Quaternion.identity);
        obj.transform.SetParent(GameObject.Find("Map1").transform);
       

        //rend = obj.GetComponent<Renderer>();
    }
}
