using UnityEngine;
using System.Collections;

public class PlayerScript : MonoBehaviour {

    public bool holdingFood;
    //!食べている時間
    private float eatingTimer;

    public GameObject foodHeld;

    HungerManager hunger;

    //!成長するまで何個食べたか
    private int ate = 0;
    //!太るまでの上限
    private int hungerUpperLimit = 4;
    //!デブレベル
    public int fatLevel = 1;
    //!生存フラグ
    //public bool dead = false;


	// Use this for initialization
	void Start () {
        hunger = FindObjectOfType<HungerManager>();
    //reference = this;
	}
	
	// Update is called once per frame
	void Update () {
        //!死んでいるかどうか
        //if(dead)
        //{
        //    //!移動速度を0に
        //    this.GetComponent<Rigidbody>().velocity = Vector3.zero;
        //}
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
        eatingTimer = 0.1f;
        holdingFood = true;
    }

    public void DroppedFood()
    {
        Debug.Log("dropped food");
        if (foodHeld)
            Destroy(foodHeld);

        holdingFood = false;
    }

    //!ほかのオブジェクトとの衝突
    void OnCollisionEnter(Collision col)
    {
        //!エネミーと衝突したら
        if (col.transform.tag == "Enemy")
        {
            //dead = true;
            //Destroy(this.gameObject);
            //Debug.Log("enemy");
            //Vector3 dir = col.contacts[0].point - transform.position;
            //dir = -dir.normalized;
            //GetComponent<CharacterMovement>().Knocked();
            //GetComponent<Rigidbody>().velocity = (dir * 5.0f);
            //transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(dir), 0.5f);
            //hunger.Damaged();
        }
    }

    ///
    ///@    食べた数が成長までの上限を超えたら大きくする
    ///@    デブレベルをワンランク上げた後成長までにかかる量を増やす
    ///
    public void SatisfyStomach()
    {
        if(ate >= hungerUpperLimit)
        {
            StopCoroutine("Grow");
            StartCoroutine("Grow");
            ate = 0;
            fatLevel += 1;
            hungerUpperLimit *= fatLevel;
        }
    }


}
