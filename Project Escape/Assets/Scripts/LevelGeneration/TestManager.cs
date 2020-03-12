using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestManager : MonoBehaviour
{
    void Start()
    {
        //Generar mapa
        LevelGenerator lg = new LevelGenerator(50, 10, 6, 15);
        CellMapV2 cm = GetComponentInParent<CellMapV2>();
        cm.SetupMap(lg.GenerateMap());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
