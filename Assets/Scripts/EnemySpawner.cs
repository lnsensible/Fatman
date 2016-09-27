﻿using UnityEngine;
using System.Collections;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject enemyPrefab;
    [SerializeField]
    private int[] levelTable;
    private int[] spawnedEnemiesCountInLevel = new int[10];
    private float spawnDelay = 0.0f;
    private float spawnInterval = 1.0f;
    private PlayerScript player;

    TitleScreen titlemanager;

    // Use this for initialization
    void Start()
    {
        titlemanager = FindObjectOfType<TitleScreen>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerScript>();
        InvokeRepeating("Spawn", spawnDelay, spawnInterval);
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void Spawn()
    {
        if (player.fatLevel < levelTable.Length && titlemanager.GameStarted)
        {
            int spawnLimitInThisLevel = levelTable[player.fatLevel - 1];
            int spawnedEnemiesCountInThisLevel = spawnedEnemiesCountInLevel[player.fatLevel - 1];
            if (spawnedEnemiesCountInThisLevel < spawnLimitInThisLevel)
            {
                Instantiate(enemyPrefab, transform.position, transform.rotation);
                ++spawnedEnemiesCountInLevel[player.fatLevel - 1];
            }
        }
    }

}
