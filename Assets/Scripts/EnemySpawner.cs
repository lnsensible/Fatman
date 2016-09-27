using UnityEngine;
using System.Collections;

public class EnemySpawner : MonoBehaviour
{
  [SerializeField]
  private GameObject enemyPrefab;
  private float spawnDelay = 0.0f;
  private float spawnInterval = 1.0f;

  // Use this for initialization
  void Start()
  {
    InvokeRepeating("Spawn", spawnDelay, spawnInterval);
  }

  // Update is called once per frame
  void Update()
  {

  }

  private void Spawn()
  {
    Instantiate(enemyPrefab, transform.position, transform.rotation);
  }

}
