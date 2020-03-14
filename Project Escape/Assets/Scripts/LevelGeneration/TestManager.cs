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

    private CellMapV2 cm;
    private LevelGenerator lg;

    void Start()
    {
        cm = GetComponentInParent<CellMapV2>();
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
            lg = new LevelGenerator(mapSize, roomNumber, minRoomSize, maxRoomSize);
            
            cm.SetupMap(lg.GenerateMap());
        }
    }
}
