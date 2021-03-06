﻿using UnityEngine;
using System.Collections;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject enemyPrefab;
    private int[] spawnedEnemiesCountInLevel = new int[10];
    private float spawnDelay = 0.0f;
    private float spawnInterval = 1.0f;
    private PlayerScript player;
    private int oldFatLevel = 0;
    private int numEnemiesShouldAppear;
    [SerializeField]
    private float coefficient;
    [SerializeField]
    private float constant;

    public float timeSpawnInterval;

    TitleScreen titlemanager;

    // Use this for initialization
    void Start()
    {
        titlemanager = FindObjectOfType<TitleScreen>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerScript>();
        InvokeRepeating("Spawn", spawnDelay, spawnInterval);
        InvokeRepeating("SpAWNiNTERVAL", 0.0f, timeSpawnInterval);
    }

    // Update is called once per frame
    void Update()
    {

    }

     void SpAWNiNTERVAL()
    {
        MusicManager.Instance.PlaySound(MusicManager.soundlist_enemyspawn);
        Instantiate(enemyPrefab, transform.position, transform.rotation);
    }

    private void Spawn()
    {
        if (titlemanager.GameStarted == false) return;
        int currentFatLevel = player.fatLevel;
        if (currentFatLevel != oldFatLevel)
        {
            numEnemiesShouldAppear += (int)((currentFatLevel * coefficient) + constant);
            oldFatLevel = currentFatLevel;
        }

        if (numEnemiesShouldAppear > 0)
        {
            MusicManager.Instance.PlaySound(MusicManager.soundlist_enemyspawn);
            Instantiate(enemyPrefab, transform.position, transform.rotation);
            --numEnemiesShouldAppear;
        }

        //if (player.fatLevel < levelTable.Length && titlemanager.GameStarted)
        //{
        //    int spawnLimitInThisLevel = levelTable[player.fatLevel - 1];
        //    int spawnedEnemiesCountInThisLevel = spawnedEnemiesCountInLevel[player.fatLevel - 1];
        //    if (spawnedEnemiesCountInThisLevel < spawnLimitInThisLevel)
        //    {
        //        Instantiate(enemyPrefab, transform.position, transform.rotation);
        //        ++spawnedEnemiesCountInLevel[player.fatLevel - 1];
        //    }
        //}
    }

}
