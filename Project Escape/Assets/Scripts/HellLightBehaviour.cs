using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HellLightBehaviour : MonoBehaviour
{
    // Start is called before the first frame update
    public float fMinLight = 0.9f;
    public float fMaxLight = 2.1f;
    public float fChange;

    Light lLight;
    void Start()
    {
        lLight = GetComponent<Light>();
    }

    // Update is called once per frame
    void Update()
    {
        fChange += 0.01f;
        if (fChange > Mathf.PI) fChange = 0;
        lLight.intensity = (1 - Mathf.Sin(fChange)) * fMinLight + Mathf.Sin(fChange) * fMaxLight;
    }
}
