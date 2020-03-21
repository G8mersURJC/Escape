using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public float fRad;
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<SphereCollider>().radius = fRad;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
