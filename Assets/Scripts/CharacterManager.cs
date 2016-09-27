using UnityEngine;
using System.Collections;

public class CharacterManager : MonoBehaviour {

    public float feverTime;
    public int feverRequired;

    public float FOVSpeed;
    public float FOVBackSpeed;
    public float FeverFOV;
    public float NormalFOV = 60;

    [SerializeField]
    ParticleSystem[] FeverFX;

    int feverAmount = 0;
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
        for (int i = 0; i < FeverFX.Length; ++i)
        {
            FeverFX[i].Stop();
            FeverFX[i].Clear();
        }
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            FEVER();
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
}
