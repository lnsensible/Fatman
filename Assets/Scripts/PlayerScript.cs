using UnityEngine;
using System.Collections;

public class PlayerScript : MonoBehaviour {
    //!食べているかどうか
    public bool holdingFood;
    //!食べている時間
    private float eatingTimer;

    public GameObject foodHeld;

    HungerManager hunger;

    //!成長するまで何個食べたか
    private int ate = 0;
    //!太るまでの上限
    private int hungerUpperLimit = 1;
    //!デブレベル
    public int fatLevel = 1;


	// Use this for initialization
	void Start () {
        hunger = FindObjectOfType<HungerManager>();
	}
	
	// Update is called once per frame
	void Update () {
        PlayerAttack();
        //!食べているとき
        if (holdingFood)
        {
            eatingTimer -= Time.deltaTime;
            //!食べ終わったら
            if (eatingTimer < 0)
            {
                ate += 1;
                hunger.Eat();
                Destroy(foodHeld);
                GetComponent<Rigidbody>().mass += 1;
                SatisfyStomach();
                //StopCoroutine("Grow");
                //StartCoroutine("Grow");
                holdingFood = false;
            }
        }
	}

    IEnumerator Grow()
    {
        float growthleft = 1.0f;
        float growthspeed = 0.5f / growthleft;
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

    void PlayerAttack()
    {
            if(Input.GetButtonDown("Jump"))
            {
                this.transform.Rotate(20.0f,0.0f,0.0f);
            }
    }

    ///
    ///@    食べた数が成長までの上限を超えたら大きくする
    ///@    デブレベルをワンランク上げて成長までにかかる量を増やす
    public void SatisfyStomach()
    {
        if(ate > hungerUpperLimit)
        {
            StopCoroutine("Grow");
            StartCoroutine("Grow");
            ate = 0;
            fatLevel += 1;
            hungerUpperLimit += 2;
        }
    } 

}
