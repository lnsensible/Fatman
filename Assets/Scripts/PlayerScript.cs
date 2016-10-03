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
    public int fatLevel = 0;

    public float GrowSpeed = 0.5f;
    public float GrowTimer = 1.0f;

    public ParticleSystem eatEffect;
    public ParticleSystem groweffect;

    public int foodworth;

    public float BumpFOVEffect;
    public float bumpShakeTime;
    public float bumpShakeStrength;

    public int secondLevel;
    public int thirdLevel;

    int level = 0;

	// Use this for initialization
	void Start () {
        reference = this;
	}

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.V))
        {
            StartCoroutine("ShakeCamera"); 
        }
    }

    public void Eat(int foodpoint)
    {
        eatEffect.Play();
        ScoreManager.Instance.AddScore(foodworth);
        ate += 1;
        GetComponent<Rigidbody>().mass += 1;
        SatisfyStomach();
        CharacterManager.Instance.EatFood(foodpoint);
    }

    IEnumerator Grow()
    {
        groweffect.Play();
        float growthleft = GrowTimer;
        float growthspeed = GrowSpeed / growthleft;
        while (growthleft > 0.0f)
        {
            yield return null;
            groweffect.startSize += (growthspeed * Time.deltaTime);
            transform.localScale += new Vector3(growthspeed * Time.deltaTime, growthspeed * Time.deltaTime, growthspeed * Time.deltaTime);
            growthleft -= Time.deltaTime;
        }
        eatEffect.startSize = transform.localScale.x * 3.0f;
    }

    void OnCollisionEnter(Collision col)
    {
        if (col.transform.tag == "Enemy")
        {
            if (CharacterManager.Instance.isFever())
            {
                MusicManager.Instance.PlaySound(MusicManager.soundlist_enemyfly);
                MusicManager.Instance.PlaySound(MusicManager.soundlist_hitenemy);
                if (ScoreManager.Instance)
                {
                    ScoreManager.Instance.hitNurse();
                    ScoreManager.Instance.AddCombo(col.transform);
                }
                StartCoroutine("ShakeCamera"); 
                col.gameObject.GetComponent<AStarEnemy>().Killed();
            }
            else
            {
                MusicManager.Instance.PlaySound(MusicManager.soundlist_hitbynurse);
                col.transform.GetComponent<AStarEnemy>().Attack();
                CharacterManager.Instance.GameOverAnimation();
                GameOverManager.Instance.GameOver();
                //Vector3 dir = col.contacts[0].point - transform.position;
                //dir = -dir.normalized;
                //GetComponent<CharacterMovement>().Knocked();
                //GetComponent<Rigidbody>().velocity = (dir * 5.0f);
                //transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(dir), 0.5f);
            }
        }
        else if (col.transform.tag == "Obstacle")
        {
            MusicManager.Instance.PlaySound(MusicManager.soundlist_wallhit);
            StartCoroutine("ShakeCamera"); 
        }
    }

    IEnumerator ShakeCamera()
    {
        SmoothFollow.shaking = true;
        float myrandomseed = Random.Range(0.0f, 100.0f);
        float amount = bumpShakeTime; //how much it shakes
        while (amount > 0.0f)
        {
            yield return null;
            amount -= Time.deltaTime;
            Camera.main.transform.localPosition = new Vector3(SmoothFollow.wantedPos.x + (Mathf.Sin(myrandomseed) * bumpShakeStrength), SmoothFollow.wantedPos.y, SmoothFollow.wantedPos.z);
            myrandomseed += Random.Range(0, 50.0f);
        }
        SmoothFollow.shaking = false;
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
            hungerUpperLimit += 0;

            if (fatLevel > secondLevel && level == 0)
            {
                level++;
                CharacterManager.Instance.ChangeSize();
            }
            else if (fatLevel > thirdLevel && level == 1)
            {
                level++;
                CharacterManager.Instance.ChangeSize();
            }
        }
    } 

}
