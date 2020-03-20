using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HUDTest : MonoBehaviour
{
    private LifeHUDController lifeController;

    // Start is called before the first frame update
    void Start()
    {
        lifeController = GameObject.Find("Main Camera").GetComponent<LifeHUDController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {

            Debug.Log("PAF!");

            //Recibir daño
            lifeController.ModifyHealthByAmount(-1);
        }

        if (Input.GetKeyDown(KeyCode.W))
        {
            Debug.Log("SANACIÓN!");

            //Curación
            lifeController.ModifyHealthByAmount(2);
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("CORAZÓN EXTRA!");
            lifeController.ExpandMaxHealth();
        }
    }
}
