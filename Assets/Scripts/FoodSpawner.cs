using UnityEngine;
using System.Collections;

public class FoodSpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject[] foodPrefabs;
    [SerializeField]
    private int[] foodPoints;
    [SerializeField]
    private GameObject[] spawnPoints;

    // Use this for initialization
    void Start()
    {
        int count = 0;
        while (count < 3)
        {
            var spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
            if (spawnPoint.transform.childCount == 0)
            {
                int random = Random.Range(0, foodPrefabs.Length);
                GameObject food = Instantiate(foodPrefabs[random], spawnPoint.transform.position, Quaternion.identity) as GameObject;
                food.transform.SetParent(spawnPoint.transform);
                food.GetComponent<Food>().foodPoint = foodPoints[random];
                ++count;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        while (GameObject.FindGameObjectsWithTag("Food").Length < 3)
        {
            var spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
            if (spawnPoint.transform.childCount == 0)
            {
                GameObject food = Instantiate(foodPrefabs[Random.Range(0, foodPrefabs.Length)], spawnPoint.transform.position, Quaternion.identity) as GameObject;
                food.transform.SetParent(spawnPoint.transform);
            }
        }
    }
}
