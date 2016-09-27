using UnityEngine;
using System.Collections;

public class PlayerScript : MonoBehaviour {

    //!食べている時間
    private float eatingTimer;

    public static PlayerScript reference;

    //!成長するまで何個食べたか
    private int ate = 0;
    //!太るまでの上限
    private int hungerUpperLimit = 1;
    //!デブレベル
    public int fatLevel = 1;

    public float GrowSpeed = 0.5f;
    public float GrowTimer = 1.0f;

	// Use this for initialization
	void Start () {
        reference = this;
	}

    public void Eat(int foodpoint)
    {
        ate += 1;
        GetComponent<Rigidbody>().mass += 1;
        SatisfyStomach();
        CharacterManager.Instance.EatFood(foodpoint);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            StartCoroutine("Grow");
        }
    }

    IEnumerator Grow()
    {
        float growthleft = GrowTimer;
        float growthspeed = GrowSpeed / growthleft;
        while (growthleft > 0.0f)
        {
            yield return null;
            transform.localScale += new Vector3(growthspeed * Time.deltaTime, growthspeed * Time.deltaTime, growthspeed * Time.deltaTime);
            growthleft -= Time.deltaTime;
        }
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
