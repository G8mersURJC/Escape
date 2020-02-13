using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float TIMER = 1.0f;
    private float myTime;

    // Start is called before the first frame update
    void Start()
    {
        myTime = TIMER;
    }

    // Update is called once per frame
    void Update()
    {
        if(myTime >= 1.0f)
        {
            myTime = 0.0f;

            if (Input.GetKey(KeyCode.W))
            {
                transform.Translate(Vector3.forward);
            }
            if (Input.GetKey(KeyCode.A))
            {
                transform.Translate(Vector3.left);
            }
            if (Input.GetKey(KeyCode.S))
            {
                transform.Translate(Vector3.back);
            }
            if (Input.GetKey(KeyCode.D))
            {
                transform.Translate(Vector3.right);
            }
        }

        myTime += Time.deltaTime;
    }
}
