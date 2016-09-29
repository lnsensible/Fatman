using UnityEngine;
using System.Collections;

public class AStarEnemy : MonoBehaviour
{
    private GameObject target;
    private NavMeshAgent navMeshAgent;
    [SerializeField]
    private float multiplyBy = 1.0f;


    void OnEnable()
    {
        HordeManager.Instance.AddEnemy(transform);
    }

    // Use this for initialization
    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player");
        navMeshAgent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        if (CharacterManager.Instance.isFever() == true)
        {
            transform.rotation = Quaternion.LookRotation(transform.position - target.transform.position);
            Vector3 runTo = transform.position + transform.forward * multiplyBy;
            NavMeshHit hit;
            NavMesh.SamplePosition(runTo, out hit, 5, 1 << NavMesh.GetAreaFromName("Walkable"));
            navMeshAgent.destination = hit.position;
        }
        else
        {
            if (navMeshAgent != null)
                navMeshAgent.destination = target.transform.position;
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
