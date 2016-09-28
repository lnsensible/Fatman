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

    float feverAmount = 0;
    bool inFeverMode = false;
    float feverTimer = 0.0f;

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
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
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
        while (feverFill.fillAmount > 0)
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
                Debug.Log("unfever");
                UNFEVER();
            }
        }
	}

    public void FEVER()
    {
        inFeverMode = true;
        feverTimer = feverTime;
        feverAmount = 0;
        StopAllCoroutines();
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
    }

    public void UNFEVER()
    {
        inFeverMode = false;
        StopAllCoroutines();
        StartCoroutine("ChangeFOV", false);
        for (int i = 0; i < FeverFX.Length; ++i)
        {
            FeverFX[i].Stop();
        }
        GaugeFX.Stop();
        feverFill.color = GaugeOriginalColor;
        feverText.SetActive(false);
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
            while (Camera.main.fieldOfView < FeverFOV)
            {
                yield return null;
                Camera.main.fieldOfView += Time.deltaTime * FOVSpeed;
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
}
