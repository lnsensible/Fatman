using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class HordeManager : MonoBehaviour {

    public float angleForEffect;

    public float effectDistance;
    public float effectTimeScale;
    public float timeScaleAffectSpeed;

    public float effectFOV;
    public float FOVAffectSpeed;

    public CanvasGroup bloodBorder;
    public float bloodAppearSpeed;

    List<Transform> enemies =new List<Transform>();
    
    [SerializeField]
    GameObject nearestEnemy;
    [SerializeField]
    float nearestAngle;

    float RealEffectDistance;

    public float distance;

    Transform playerTransform;

    bool isSlowmo = false;

    float curentBoostTime;
    public float boostTime = 0.2f;
    bool triggerBoost = false;

    public bool isSlow()
    {
        return isSlowmo;
    }

    private static HordeManager instance = null;

    public static HordeManager Instance
    {
        get { return instance; }
    }

    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
            return;
        }

        instance = this;
    }

    public void AddEnemy(Transform t){
        enemies.Add(t);
    }

    public void RemoveEnemy(Transform t){
        enemies.Remove(t);
    }

	// Use this for initialization
	void Start () {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
	}

    public bool CanSpeedup()
    {
        return (curentBoostTime > 0.0f);
    }
	
	// Update is called once per frame
	void Update () {
        RealEffectDistance = playerTransform.localScale.x + effectDistance;

        curentBoostTime -= Time.deltaTime;

        distance = Mathf.Infinity;
        float tmpd = 0;
        for (int i = 0; i < enemies.Count; ++i)
        {
            tmpd = Vector3.Distance(enemies[i].position, playerTransform.position);
            if (tmpd < distance)
            {
                distance = tmpd;
                nearestEnemy = enemies[i].gameObject;
            }
        }

        if (nearestEnemy != null)
            nearestAngle = Mathf.Abs(Vector3.Angle(playerTransform.forward, nearestEnemy.transform.position - playerTransform.position));

        if (!CharacterManager.Instance.isFever())
        {
            if (distance < RealEffectDistance && nearestAngle < angleForEffect)
            {
                triggerBoost = true;
                isSlowmo = true;
                if (!GameOverManager.Instance.isOver)
                Time.timeScale = Mathf.Lerp(Time.timeScale, effectTimeScale, timeScaleAffectSpeed * Time.unscaledDeltaTime);
                Time.fixedDeltaTime = Time.timeScale * 0.02f;
                Camera.main.fieldOfView = Mathf.Lerp(Camera.main.fieldOfView, effectFOV, FOVAffectSpeed * Time.unscaledDeltaTime);
                bloodBorder.alpha += bloodAppearSpeed * Time.unscaledDeltaTime;
            }
            else
            {
                if (triggerBoost)
                {
                    curentBoostTime = boostTime;
                    triggerBoost = false;
                }
                isSlowmo = false;
                if (!GameOverManager.Instance.isOver)
                Time.timeScale = Mathf.Lerp(Time.timeScale, 1.0f, timeScaleAffectSpeed * Time.deltaTime);
                Time.fixedDeltaTime = Time.timeScale * 0.02f;
                Camera.main.fieldOfView = Mathf.Lerp(Camera.main.fieldOfView, 60, FOVAffectSpeed * Time.unscaledDeltaTime);
                bloodBorder.alpha -= bloodAppearSpeed * Time.unscaledDeltaTime;
            }
        }
	}

    public void Fever()
    {
        isSlowmo = false;
        bloodBorder.alpha = 0;
        Time.timeScale = 1.0f;
        Time.fixedDeltaTime = Time.timeScale * 0.02f;
    }
}
