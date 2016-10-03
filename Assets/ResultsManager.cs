using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class ResultsManager : MonoBehaviour {

    public float scoreDelay;

    public float roadSpeed;
    public GameObject fakeRoad;

    public float fadeTime;
    public CanvasGroup screenHider;

    int foodEaten;
    int nurseHit;
    int nurseScore;
    int ElapsedTime;

    public Text[] foodeatenText;
    public Transform foodTextHolder;
    public Text[] nursehitText;
    public Transform nurseTextHolder;
    public int nurseScoreIncreaseInterval = 250;

    public Text[] timeText;
    public Transform timeTextHolder;

    public Text[] scoreText;
    public Transform scoreTextHolder;

    public float minScaleSize;
    public float maxScaleSize;

    public int foodWorth;

    public float timeMultiplier;

    int currentScore = 0;

    public GameObject newrecord;

	// Use this for initialization
	void Start () {
        MusicManager.Instance.PlayBGM(MusicManager.soundlist_bgm_gameover);

        Time.timeScale = 1;
        screenHider.alpha = 1;
        screenHider.blocksRaycasts = true;

        foodEaten =  PlayerPrefs.GetInt("foodeaten", 0);
        nurseHit = PlayerPrefs.GetInt("nursehit", 0);
        nurseScore = PlayerPrefs.GetInt("nursescore", 0);
        ElapsedTime = PlayerPrefs.GetInt("elapsedtime", 0);

        StartCoroutine("Fadeout");

	}
	
	// Update is called once per frame
	void Update () {

            fakeRoad.transform.position -= new Vector3(roadSpeed * Time.deltaTime, 0, 0);
	}

    IEnumerator Fadeout()
    {
        while (screenHider.alpha > 0)
        {
            yield return null;
            screenHider.alpha -= fadeTime * Time.deltaTime;
        }
        screenHider.blocksRaycasts = false;
        StartCoroutine("ScoreChangeFood", foodEaten);
    }

    IEnumerator Fadein()
    {
        screenHider.blocksRaycasts = true;
        while (screenHider.alpha < 1)
        {
            yield return null;
            screenHider.alpha += fadeTime * Time.deltaTime;
        }

        SceneManager.LoadScene("Game", LoadSceneMode.Single);
    }

    public void Menu()
    {
        MusicManager.Instance.PlaySound(MusicManager.soundlist_bgm_select);
        StartCoroutine("Fadein");
    }

    IEnumerator ScoreChangeFood()
    {
        yield return new WaitForSecondsRealtime(scoreDelay);
        int scoretoachieve = foodWorth * foodEaten;
        int currenteaten = 0;
        int currentscore = 0;

        while (currentscore != scoretoachieve)
        {
            yield return null;
            MusicManager.Instance.PlaySound(MusicManager.soundlist_resultscore);
            ++currenteaten;
            currentscore += foodWorth;
            currentScore += foodWorth;
           
            SetText(foodeatenText, currenteaten);
            SetText(scoreText, currentScore);

            float randSize = Random.Range(minScaleSize, maxScaleSize);
            foodTextHolder.localScale = new Vector3(randSize, randSize, randSize);

            randSize = Random.Range(minScaleSize, maxScaleSize);
            scoreTextHolder.localScale = new Vector3(randSize, randSize, randSize);
        }

        foodTextHolder.localScale = Vector3.one;
        StartCoroutine("ScoreChangeNurse");
    }

    IEnumerator ScoreChangeNurse()
    {
        yield return null;
        int scoretoachieve = nurseScore;
        int currentnurse = 0;
        int currentscore = 0;

        while (currentscore != scoretoachieve)
        {
            yield return null;
            MusicManager.Instance.PlaySound(MusicManager.soundlist_resultscore);
            float randSize = Random.Range(minScaleSize, maxScaleSize);
            
            if (currentnurse != nurseHit)
            {
                ++currentnurse;
                SetText(nursehitText, currentnurse);
                nurseTextHolder.localScale = new Vector3(randSize, randSize, randSize);
            }
            else
            {
                nurseTextHolder.localScale = Vector3.one;
            }

            currentscore += nurseScoreIncreaseInterval;
            currentScore += nurseScoreIncreaseInterval;

            if (currentscore > scoretoachieve)
            {
                currentscore = scoretoachieve;
                currentScore = scoretoachieve;
            }

            SetText(scoreText, currentScore);

            randSize = Random.Range(minScaleSize, maxScaleSize);
            scoreTextHolder.localScale = new Vector3(randSize, randSize, randSize);
        }

        
        StartCoroutine("ScoreChangeTime");
    }

    IEnumerator ScoreChangeTime(){
        yield return null;

        int incrementer = Mathf.RoundToInt(currentScore * (timeMultiplier - 1));
        int currenttime = 0;

        while (currenttime < ElapsedTime)
        {
            yield return null;
            MusicManager.Instance.PlaySound(MusicManager.soundlist_resultscore);
            ++currenttime;
            SetText(timeText, currenttime);

            currentScore += incrementer;
            SetText(scoreText, currentScore);

            float randSize = Random.Range(minScaleSize, maxScaleSize);
            timeTextHolder.localScale = new Vector3(randSize, randSize, randSize);

            randSize = Random.Range(minScaleSize, maxScaleSize);
            scoreTextHolder.localScale = new Vector3(randSize, randSize, randSize);
        }


        timeTextHolder.localScale = Vector3.one;
        scoreTextHolder.localScale = Vector3.one;
        if (currentScore > PlayerPrefs.GetInt("HIGHSCORE", -1))
        {
            MusicManager.Instance.PlaySound(MusicManager.soundlist_highscore);
            PlayerPrefs.SetInt("HIGHSCORE", currentScore);
            newrecord.SetActive(true);
        }
    }

    void SetText(Text[] tchange, int num)
    {
        string s = num.ToString();
        for (int j = 0; j < s.Length; ++j)
        {
            tchange[j].text = s[s.Length - 1 - j].ToString();
        }
    }
}
