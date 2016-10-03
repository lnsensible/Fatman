using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CharacterManager : MonoBehaviour {

    public float feverTime = 5;
    public float feverRequired = 10;

    public float FOVSpeed = 100;
    public float FOVBackSpeed = 20;
    public float FeverFOV = 80;
    public float NormalFOV = 60;

    public float MaxPulseScale = 0.7f;
    public float PulseSpeed = 1.0f;
    public float originalPulse = 0.5f;

    public float fillSpeed;

    public Image feverFill;
    public GameObject feverGauge;

    Color GaugeOriginalColor;
    public Gradient GaugeFeverGradient;
    public float GaugeFeverColorSpeed;

    [SerializeField]
    ParticleSystem[] FeverFX;

    [SerializeField]
    ParticleSystem GaugeFX;

    [SerializeField]
    GameObject feverText;
    public Shake[] shakes;

    [SerializeField]
    ParticleSystem feverRingFX;

    float feverAmount = 0;
    bool inFeverMode = false;
    float feverTimer = 0.0f;

    Transform playerTransform;

    public Animator fever2D;
    public string fever2dname;
    public Animator unfever2D;
    public string unfever2dname;

    public float rollSpeed;

    int currentIndex = 0;
    public Transform modelTransform;

    public GameObject[] characterModels;

    public Animator[] playeranimator;
    public string speedParameterName;

    public string gameoveranimname;
    public string rollanimname;
    public string runanimname;

    private static CharacterManager instance = null;

    public static CharacterManager Instance
    {
        get { return instance; }
    }

    public bool isFever()
    {
        return inFeverMode;
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

    void Start()
    {
        playeranimator[currentIndex].SetFloat(speedParameterName, 999);

        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        GaugeOriginalColor = feverFill.color;

        for (int i = 0; i < FeverFX.Length; ++i)
        {
            FeverFX[i].Stop();
            FeverFX[i].Clear();
        }

        feverFill.fillAmount = 0;
    }

    IEnumerator GaugeFill()
    {
        float fillpercentage = feverAmount/feverRequired;
        if (fillpercentage > 1.0f)
            fillpercentage = 1.0f;

        while (feverFill.fillAmount < fillpercentage)
        {
            yield return null;
            feverFill.fillAmount = Mathf.MoveTowards(feverFill.fillAmount, fillpercentage, Time.deltaTime * fillSpeed);
        }

        if (fillpercentage == 1.0f)
        {
            FEVER();
        }
    }

    IEnumerator Deplete()
    {
        float max = 1;
        float min = 0;
        float t = 0;
        while (feverFill.fillAmount > min)
        {
            yield return null;
            t += Time.deltaTime/feverTime;
            feverFill.fillAmount = Mathf.MoveTowards(max, min, t);
        }
    }

    public void EatFood(float amt)
    {
        if (!inFeverMode)
        {
            feverAmount += amt;
            StopCoroutine("GaugeFill");
            StartCoroutine("GaugeFill");
        }
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            EatFood(1.0f);
        }

        if (inFeverMode)
        {
            feverTimer -= Time.deltaTime;
            if (feverTimer < 0)
            {
                UNFEVER();
            }
        }
	}

    public void FEVER()
    {
        Time.timeScale = 0;
        fever2D.gameObject.SetActive(true);
        fever2D.Play(fever2dname);
    }

    public void StartFever(bool roll = false)
    {
        Time.timeScale = 1;
        inFeverMode = true;
        feverTimer = feverTime;
        feverAmount = 0;
        StopAllCoroutines();

        feverText.SetActive(true);

        feverRingFX.Play();
        feverRingFX.transform.localScale = playerTransform.localScale * 2;

        StartCoroutine("ChangeFOV", true);
        for (int i = 0; i < FeverFX.Length; ++i)
        {
            FeverFX[i].Play();
        }
        StartCoroutine("Deplete");
        StartCoroutine("Pulse");
        GaugeFX.Play();
        StartCoroutine("RainbowGauge");
        feverText.SetActive(true);
        for (int i = 0; i < shakes.Length; ++i)
        {
            shakes[i].enabled = true;
        }

        if (currentIndex == 2)
        {
            StartCoroutine("Roll");
        }
        HordeManager.Instance.Fever();
        ScoreManager.Instance.Fever();
    }

    IEnumerator Roll()
    {
        PrepareRoll();
        while (true)
        {
            yield return null;
            modelTransform.Rotate(Vector3.left, rollSpeed * Time.deltaTime, Space.Self);
        }
    }

    public void PrepareRoll(){
        playeranimator[currentIndex].updateMode = AnimatorUpdateMode.UnscaledTime;
        playeranimator[currentIndex].Play(rollanimname);
    }

    public void ChangeSize()
    {
        characterModels[currentIndex].SetActive(false);
        currentIndex += 1;
        characterModels[currentIndex].SetActive(true);
    }

    public void UNFEVER()
    {
        feverRingFX.Stop();
        feverRingFX.Clear();

        GaugeFX.Stop();
        GaugeFX.Clear();

        for (int i = 0; i < FeverFX.Length; ++i)
        {
            FeverFX[i].Stop();
            FeverFX[i].Clear();
        }

        feverText.SetActive(false);

        StopCoroutine("Roll");
        modelTransform.localEulerAngles = Vector3.zero;

        playeranimator[currentIndex].updateMode = AnimatorUpdateMode.Normal;
        playeranimator[currentIndex].Play(runanimname);
        Time.timeScale = 0;

        unfever2D.gameObject.SetActive(true);
        unfever2D.Play(unfever2dname);
    }

    public void endFever()
    {
        Time.timeScale = 1;
        inFeverMode = false;
        StopAllCoroutines();

        StartCoroutine("ChangeFOV", false);

        feverFill.color = GaugeOriginalColor;
        
    }

    IEnumerator RainbowGauge()
    {
        while (true)
        {
            yield return null;
            float t = Mathf.Repeat(Time.time, GaugeFeverColorSpeed) / GaugeFeverColorSpeed;
            feverFill.color = GaugeFeverGradient.Evaluate(t);
        }
    }

    IEnumerator ChangeFOV(bool t)
    {
        if (t)
        {
            while (true)
            {
                yield return null;
                while (Camera.main.fieldOfView < FeverFOV)
                {
                    yield return null;
                    Camera.main.fieldOfView += Time.deltaTime * FOVSpeed;
                }
            }
        }
        else
        {
            while (Camera.main.fieldOfView > NormalFOV)
            {
                yield return null;
                Camera.main.fieldOfView -= Time.deltaTime * FOVBackSpeed;
            }
        }
    }

    IEnumerator Pulse()
    {
        Vector3 scaleSpeed = new Vector3(PulseSpeed, PulseSpeed, 0);
        Vector3 normalScale = new Vector3(originalPulse, originalPulse, 1);
        while (true)
        {
            feverGauge.transform.localScale += scaleSpeed * Time.deltaTime;
            if (feverGauge.transform.localScale.x > MaxPulseScale)
            {
                feverGauge.transform.localScale = normalScale;
            }
            yield return null;
        }
    }

    public void GameOverAnimation()
    {
        playeranimator[currentIndex].updateMode = AnimatorUpdateMode.UnscaledTime;
        playeranimator[currentIndex].Play(gameoveranimname);
    }

    public void SetAnimatorSpeed(float s)
    {
        playeranimator[currentIndex].SetFloat(speedParameterName, s);
    }
}
