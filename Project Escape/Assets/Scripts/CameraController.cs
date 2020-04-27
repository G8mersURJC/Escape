using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float dRadious = 5f;
    public float dAngle = 60f;

    RenderingTestManager rtm;

    // Start is called before the first frame update
    void Start()
    {
        rtm = GameObject.Find("Dios impostor").GetComponent<RenderingTestManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (rtm.player.GetActor())
        {
            //Situa la cámara en el personaje.
            transform.position = new Vector3(rtm.player.GetActor().GetComponent<Transform>().position.x, rtm.player.GetActor().GetComponent<Transform>().position.y, rtm.player.GetActor().GetComponent<Transform>().position.z);
            //Realiza la rotación.
            transform.rotation = Quaternion.Euler(dAngle, 0, 0);
            //Se desplaza con el radio indicado acorde a la rotación aplicada.
            transform.Translate(-Vector3.forward * dRadious);
        }

       
    }
}