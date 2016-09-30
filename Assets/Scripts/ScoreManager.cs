using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class ScoreManager : MonoBehaviour {

    public float jumpSpeed;
    public float jumpHeight;

    public float sinScale;

    [SerializeField]
    private int theActualScore;

    private float originalHeight;

    public Text[] scoreText;

    public float ScaleDownSpeed;
    public float ScaleUpSpeed;
    public float ScaleBackSpeed;
    public float smallScaleAmt;
    public float bigScaleAmt;
    public float originalScaleAmt;

    public GameObject comboTextPrefab;

    public float comboTextHeightOffset;
    public float comboTextOffsetMultiplier;
    public float maxComboTextScreenHeight;

    public float combobasedSizeIncrease;
    public float maxTextSizeIncrease;

    public Color[] comboColors;

    public Canvas parentCanvas;

    Transform player;

    public int baseEnemyScore;
    public float comboMultiplier;
    public int maxComboScore;

    public float camFOV;

    int comboCount = 0;

    int foodEaten;
    int nurseHit;
    
    private static ScoreManager instance = null;

    public static ScoreManager Instance
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

    public void ateFood()
    {
        foodEaten++;
    }

    public void hitNurse()
    {
        nurseHit++;
    }

	// Use this for initialization
	void Start () {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        originalHeight = scoreText[0].transform.localPosition.y;
        jumpHeight += originalHeight;

        theActualScore = 0;

        //StartCoroutine("ScoreJump");
	}

    public void Fever()
    {
        comboCount = 0;
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.F))
        {
            AddCombo(GameObject.FindGameObjectWithTag("Player").transform);
        }

        if(Input.GetKeyDown(KeyCode.Space)){
            AddScore(99);
        }
	}

    public void AddCombo(Transform t){
        ++comboCount;
        Camera.main.fieldOfView = camFOV;
        GameObject clone = Instantiate(comboTextPrefab);

        Vector3 worldPos = new Vector3(t.position.x, t.position.y, t.position.z + (comboTextHeightOffset + (comboCount * comboTextOffsetMultiplier)));
        Vector3 screenPos = Camera.main.WorldToScreenPoint(worldPos);
        clone.transform.SetParent(parentCanvas.transform, false);
        clone.transform.position = new Vector3(screenPos.x, Mathf.Min(screenPos.y, maxComboTextScreenHeight), screenPos.z);

        int size = (int)(Mathf.Min(maxTextSizeIncrease, comboCount * combobasedSizeIncrease));
        clone.GetComponent<Text>().fontSize += size;

        int correctColor = comboCount % comboColors.Length;
        clone.GetComponent<Text>().color = comboColors[correctColor];

        int worth = (int)(baseEnemyScore * (comboCount * comboMultiplier));
        if (worth > maxComboScore)
            worth = maxComboScore;

        clone.GetComponent<Text>().text = worth.ToString();

        AddScore(worth);
    }

    public void AddScore(int howmuch)
    {
        StopAllCoroutines();
        for (int i = 0; i < scoreText.Length; ++i)
        {
            scoreText[i].transform.localPosition = new Vector3(scoreText[i].transform.localPosition.x, originalHeight, scoreText[i].transform.localPosition.z);
        }

        theActualScore += howmuch;
        if (theActualScore > 999999)
            theActualScore = 999999;

        for (int i = 0; i < theActualScore.ToString().Length; ++i)
        {
            StartCoroutine("ScoreChange", i);
        }
    }

    IEnumerator ScoreChange(int i)
    {
        bool changing = true;
        int process = 0;
        Vector3 scaleDVector = new Vector3(ScaleDownSpeed, ScaleDownSpeed, ScaleDownSpeed);
        Vector3 scaleUVector = new Vector3(ScaleUpSpeed, ScaleUpSpeed, ScaleUpSpeed);
        Vector3 scaleOVector = new Vector3(ScaleBackSpeed, ScaleBackSpeed, ScaleBackSpeed);

        scoreText[i].transform.localScale = new Vector3(originalScaleAmt, originalScaleAmt, originalScaleAmt);

        while (changing)
        {
            yield return null;
            if (process == 0)
            {
                scoreText[i].transform.localScale -= scaleDVector * Time.deltaTime;
                if (scoreText[i].transform.localScale.x < smallScaleAmt)
                {
                    ++process;
                    string s = theActualScore.ToString();
                    char[] reversed = new char[s.Length];
                    for (int j = 0; j < s.Length; ++j)
                    {
                        reversed[j] = s[s.Length-1 - j];
                    }
                        
                    scoreText[i].text = reversed[i].ToString();
                }
            }
            else if (process == 1)
            {
                scoreText[i].transform.localScale += scaleUVector * Time.deltaTime;
                if (scoreText[i].transform.localScale.x > bigScaleAmt)
                    ++process;
            }
            else
            {
                scoreText[i].transform.localScale -= scaleOVector * Time.deltaTime;
                if (scoreText[i].transform.localScale.x < originalScaleAmt)
                {
                    changing = false;
                    scoreText[i].transform.localScale = new Vector3(originalScaleAmt, originalScaleAmt, originalScaleAmt);
                }
            }
        }  
    }

    IEnumerator ScoreJump()
    {
        float sinresult = 0;
        while (true)
        {
            yield return null;
            for (int i = 0; i < scoreText.Length; ++i)
            {
                sinresult = Mathf.Sin(i * sinScale + Time.time * jumpSpeed);
                if (sinresult >= 0)
                {
                    scoreText[i].transform.localPosition = new Vector3(scoreText[i].transform.localPosition.x, originalHeight + sinresult * jumpHeight, scoreText[i].transform.localPosition.z);
                }
                else
                {
                    scoreText[i].transform.localPosition = new Vector3(scoreText[i].transform.localPosition.x, originalHeight, scoreText[i].transform.localPosition.z);
                }
            }
        }
    }

    public void SaveScoreToPlayerPref()
    {
        PlayerPrefs.SetInt("foodeaten", foodEaten);
        PlayerPrefs.SetInt("nursehit", nurseHit);
        PlayerPrefs.SetFloat("timesurvive", 0);
    }

    //public static string[] IntToIntArray(int num)
    //{
    //    List<string> digits = new List<string>();

    //    for (; num != 0; num /= 10)
    //        digits.Add((num % 10).ToString());

    //    string[] array = digits.ToArray();
    //    System.Array.Reverse(array);

    //    return array;
    //}
}
