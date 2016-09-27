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
        if (navMeshAgent != null)
        navMeshAgent.destination = target.transform.position;
    }

    public void Killed()
    {
        Destroy(GetComponent<Rigidbody>());
        Destroy(GetComponent<BoxCollider>());
        Destroy(GetComponent<NavMeshAgent>());
        StartCoroutine("Die");
    }

    IEnumerator Die()
    {
        Vector3 max = transform.localScale;
        float t = 0;
        while (transform.localScale.x > 0)
        {
            yield return null;
            t += Time.deltaTime / 1.0f;
            transform.localScale = Vector3.MoveTowards(max, Vector3.zero, t);
        }

        Destroy(gameObject);
    }


}
