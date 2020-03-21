using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUDTest : MonoBehaviour
{ 
    public Button bStart;
    public InputField ifName;
    public GameObject goHowToPlayPanel;

    private LifeHUDController lifeController;

    private bool bControlsWindowActive = true;

    // Start is called before the first frame update
    void Start()
    {
        lifeController = this.GetComponent<LifeHUDController>();
    }

    public void StartButtonPressed()
    {
        //Guardar el nombre introducido...
        string username = ifName.text;
        Debug.Log("Mi nombre es "+username);

        //Desactivamos la ventana
        Destroy(goHowToPlayPanel);
        bControlsWindowActive = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!bControlsWindowActive)
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
}
