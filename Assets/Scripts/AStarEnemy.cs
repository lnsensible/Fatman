using UnityEngine;
using System.Collections;

public class AStarEnemy : MonoBehaviour
{
  private GameObject target;
  private NavMeshAgent navMeshAgent;

  // Use this for initialization
  void Start()
  {
    target = GameObject.FindGameObjectWithTag("Player");
    navMeshAgent = GetComponent<NavMeshAgent>();
  }

  // Update is called once per frame
  void Update()
  {
    navMeshAgent.destination = target.transform.position;
  }

}
