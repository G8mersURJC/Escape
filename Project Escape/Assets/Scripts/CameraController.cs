using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject goDios;
    public GameObject goFollowTo = null;
    public float dRadious = 5f;
    public float dAngle = 60f;


    // Start is called before the first frame update
    void Start()
    {

    }
    // Update is called once per frame
    void Update()
    {
        //Situa la cámara en el personaje.
        transform.position = new Vector3(goFollowTo.GetComponent<Transform>().position.x, goFollowTo.GetComponent<Transform>().position.y, goFollowTo.GetComponent<Transform>().position.z);
        //Realiza la rotación.
        transform.rotation = Quaternion.Euler(dAngle, 0, 0);
        //Se desplaza con el radio indicado acorde a la rotación aplicada.
        transform.Translate(-Vector3.forward * dRadious);
    }
}