using UnityEngine;
using System.Collections;

public class NormalEnemy : MonoBehaviour {

    GameObject characterReference;
    public float speed;

    Rigidbody myrigidbody;

    public float idleMin;
    public float idleMax;

    public float moveMin;
    public float moveMax;

    public float idleSpeed;

    Vector3 idleDirection;

    bool isMoving;
    float wanderTimer;

    ParticleSystem particleSystemIdle;
    ParticleSystem particleSystemChase;

    enum State
    {
        Wander,
        Chase
    };

    State state;

	// Use this for initialization
	void Start () {
        characterReference = GameObject.FindGameObjectWithTag("Player");
        myrigidbody = GetComponent<Rigidbody>();
        ParticleSystem[] ps = GetComponentsInChildren<ParticleSystem>();
        particleSystemIdle = ps[0];
        particleSystemChase = ps[1];

        particleSystemIdle.Play();
        particleSystemChase.Stop();
        particleSystemChase.Clear();
	}
	
	// Update is called once per frame
	void Update () {

        Vector3 movement = Vector3.zero;
        if (state == State.Chase)
        {
            movement = (characterReference.transform.position - transform.position).normalized;

            myrigidbody.velocity = (movement * speed);
        }
        else if (state == State.Wander)
        {
            RaycastHit[] hit;
            hit = Physics.RaycastAll(transform.position, (characterReference.transform.position - transform.position), 1000.0f);

            bool obstacle = false;
            for (int i = 0; i < hit.Length; ++i)
            {
                if (hit[i].transform.tag == "Obstacle")
                {
                    obstacle = true;
                    break;
                }
            }

            if (obstacle)
            {
                if (isMoving)
                {
                    wanderTimer -= Time.deltaTime;
                    if (wanderTimer > 0)
                    {
                        myrigidbody.velocity = idleDirection * idleSpeed;
                        movement = idleDirection;
                    }
                    else
                    {
                        wanderTimer = Random.Range(idleMin, idleMax);
                        myrigidbody.velocity = Vector3.zero;
                        isMoving = false;
                        
                    }
                }
                else
                {
                    wanderTimer -= Time.deltaTime;
                    if (wanderTimer < 0)
                    {
                        idleDirection = new Vector3(Random.Range(-1.0f, 1.0f), 0, Random.Range(-1.0f, 1.0f));
                        wanderTimer = Random.Range(moveMin, moveMax);
                        isMoving = true;
                    }
                }
            }
            else
            {
                particleSystemChase.Play();
                particleSystemIdle.Stop();
                particleSystemIdle.Clear();
                state = State.Chase;
            }
        }

        if (movement != Vector3.zero)
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(movement), 0.5f);
	}

    void OnCollisionEnter(Collision col)
    {
        if (col.transform.tag == "Obstacle")
        {
            isMoving = false;
            particleSystemIdle.Play();
            particleSystemChase.Stop();
            particleSystemChase.Clear();
            state = State.Wander;

            myrigidbody.velocity = (transform.position - col.contacts[0].point).normalized * 0.3f;
            transform.rotation = Quaternion.LookRotation(myrigidbody.velocity);
        }
    }
}
