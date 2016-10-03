using UnityEngine;
using System.Collections;

public class AStarEnemy : MonoBehaviour
{
    private GameObject target;
    private NavMeshAgent navMeshAgent;
    private float multiplyBy = 1.0f;
    public float chaseSpeed;
    public float fleeSpeed;

    public float flySpeed = 100;

    Rigidbody myRigidbody;

    void OnEnable()
    {
        HordeManager.Instance.AddEnemy(transform);
    }

    // Use this for initialization
    void Start()
    {
        myRigidbody = GetComponent<Rigidbody>();
        target = GameObject.FindGameObjectWithTag("Player");
        navMeshAgent = GetComponent<NavMeshAgent>();

        navMeshAgent.updateRotation = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (navMeshAgent != null)
        {
            if (CharacterManager.Instance.isFever() == true)
            {
                transform.rotation = Quaternion.LookRotation(transform.position - target.transform.position);
                Vector3 runTo = transform.position + transform.forward * multiplyBy;
                NavMeshHit hit;
                NavMesh.SamplePosition(runTo, out hit, 5, 1 << NavMesh.GetAreaFromName("Walkable"));
                navMeshAgent.destination = hit.position;
                navMeshAgent.speed = fleeSpeed;
            }
            else
            {
                navMeshAgent.destination = target.transform.position;
                navMeshAgent.speed = chaseSpeed;
            }
        }
    }

    public void Killed()
    {
        HordeManager.Instance.RemoveEnemy(transform);
        //Destroy(GetComponent<Rigidbody>());
        Destroy(GetComponent<BoxCollider>());
        Destroy(GetComponent<NavMeshAgent>());
        Destroy(GetComponent<ChracterRestrict>());

        if (!HordeManager.Instance.Fly())
        {
            Vector3 velocity = (target.GetComponent<Rigidbody>().velocity.normalized + Vector3.up) * flySpeed;
            Vector3 torque = new Vector3(Random.Range(-100, 100), Random.Range(-100, 100), Random.Range(-100, 100)).normalized * flySpeed;
            myRigidbody.velocity = velocity;
            myRigidbody.AddRelativeTorque(torque, ForceMode.Impulse);    
        }
        else
        {
            Vector3 direction = (Camera.main.transform.position - transform.position).normalized;
            Vector3 velocity = direction * flySpeed;
            myRigidbody.velocity = velocity;
        }

        StartCoroutine("Die");
    }

    public void Attack()
    {
        GetComponentInChildren<Animator>().updateMode = AnimatorUpdateMode.UnscaledTime;
        GetComponentInChildren<Animator>().Play("Attack");
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
