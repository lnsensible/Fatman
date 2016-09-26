using UnityEngine;
using System.Collections;

public class PlayerScript : MonoBehaviour {

    public bool holdingFood;

    private float eatingTimer;

    public GameObject foodHeld;

    HungerManager hunger;
	// Use this for initialization
	void Start () {
        hunger = FindObjectOfType<HungerManager>();
	}
	
	// Update is called once per frame
	void Update () {
	    if (holdingFood)
        {
            eatingTimer -= Time.deltaTime;
            if (eatingTimer < 0)
            {
                hunger.Eat();
                Destroy(foodHeld);
                GetComponent<Rigidbody>().mass += 1;
                StopCoroutine("Grow");
                StartCoroutine("Grow");
                holdingFood = false;
            }
        }
	}

    IEnumerator Grow()
    {
        float growthleft = 1.0f;
        float growthspeed = 0.2f / growthleft;
        while (growthleft > 0.0f)
        {
            yield return null;
            transform.localScale += new Vector3(growthspeed * Time.deltaTime, growthspeed * Time.deltaTime, growthspeed * Time.deltaTime);
            growthleft -= Time.deltaTime;
        }
    }

    public void HoldFood(GameObject food)
    {
        foodHeld = food;
        eatingTimer = 1.5f;
        holdingFood = true;
    }

    public void DroppedFood()
    {
        Debug.Log("dropped food");
        if (foodHeld)
            Destroy(foodHeld);

        holdingFood = false;
    }

    void OnCollisionEnter(Collision col)
    {
        if (col.transform.tag == "Enemy")
        {
            Debug.Log("enemy");
            Vector3 dir = col.contacts[0].point - transform.position;
            dir = -dir.normalized;
            GetComponent<CharacterMovement>().Knocked();
            GetComponent<Rigidbody>().velocity = (dir * 5.0f);
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(dir), 0.5f);
            hunger.Damaged();
        }
    }
}
