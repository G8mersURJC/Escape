﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawn : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GameManager.manager.SpawnEnemy(new Vector2(transform.position.x, transform.position.z));
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}