using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestManager : MonoBehaviour
{
    public GameObject mapParent;

    public int mapSize;
    public int roomNumber;
    public int minRoomSize;
    public int maxRoomSize;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            //Borramos todos los elementos del mapa anterior
            foreach (Transform child in mapParent.transform)
            {
                Destroy(child.gameObject);
            }

            //Generamos un mapa nuevo
            LevelGenerator lg = new LevelGenerator(mapSize, roomNumber, minRoomSize, maxRoomSize);
            CellMapV2 cm = GetComponentInParent<CellMapV2>();
            cm.SetupMap(lg.GenerateMap());
        }
    }
}
