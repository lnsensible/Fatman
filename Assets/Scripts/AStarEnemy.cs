using UnityEngine;
using System.Collections;

public class AStarEnemy : MonoBehaviour
{
  private NavMeshAgent navMeshAgent;

  // Use this for initialization
  void Start()
  {
    navMeshAgent = GetComponent<NavMeshAgent>();
  }

  // Update is called once per frame
  void Update()
  {
    GameObject target = PlayerScript.reference.gameObject;
    float distanceToPlayer = (PlayerScript.reference.transform.position - transform.position).magnitude;
    float distanceToNearest = distanceToPlayer;
    GameObject[] foods;
    foods = GameObject.FindGameObjectsWithTag("Food");
    foreach (GameObject food in foods)
    {
      float distanceToFood = (food.transform.position - transform.position).magnitude;
      if (distanceToFood < distanceToNearest)
      {
        distanceToNearest = distanceToFood;
        target = food;
      }
    }

    navMeshAgent.destination = target.transform.position;
  }

}
