using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TorchLightBehaviour : MonoBehaviour
{
    // Start is called before the first frame update
    public float fMinLight = 0.7f;
    public float fMaxLight = 1.6f;

    Light lLight;
    void Start()
    {
        lLight = GetComponent<Light>();
    }

    // Update is called once per frame
    void Update()
    {
        float chn = Random.Range(-0.25f, 0.25f);
        lLight.intensity += chn;
        if (lLight.intensity < fMinLight) lLight.intensity = fMinLight;
        if (lLight.intensity > fMaxLight) lLight.intensity = fMaxLight;
    }
}
