using UnityEngine;
using System.Collections;

public class AStarEnemy : MonoBehaviour
{
    private GameObject target;
    private NavMeshAgent navMeshAgent;
    private EnemySpawner enemySpawner;

    void OnEnable()
    {
        HordeManager.Instance.AddEnemy(transform);
    }

    // Use this for initialization
    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player");
        navMeshAgent = GetComponent<NavMeshAgent>();
        enemySpawner = FindObjectOfType<EnemySpawner>();
    }

    // Update is called once per frame
    void Update()
    {
        if (navMeshAgent != null)
        {
            if (CharacterManager.Instance.isFever() == false)
            {
                navMeshAgent.destination = target.transform.position;
            }
            else
            {
                navMeshAgent.destination = enemySpawner.transform.position;
            }
        }
    }

    public void Killed()
    {
        HordeManager.Instance.RemoveEnemy(transform);
        Destroy(GetComponent<Rigidbody>());
        Destroy(GetComponent<BoxCollider>());
        Destroy(GetComponent<NavMeshAgent>());
        Destroy(GetComponent<ChracterRestrict>());
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
