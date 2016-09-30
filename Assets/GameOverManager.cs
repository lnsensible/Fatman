using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameOverManager : MonoBehaviour {

    public float GameoverFOV;
    public float GameoverFOVSpeed;

    public float transitionDelay;
    public CanvasGroup transitionCG;
    public float transitionSpeed;

    bool isOver = false;

    private static GameOverManager instance = null;

    public static GameOverManager Instance
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
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.A))
        {
            //GameOver();
        }
	}

    public void GameOver()
    {
        if (!isOver)
        {
            isOver = true;
            Time.timeScale = 0;
            StartCoroutine("FOVChange");
            StartCoroutine("TransitionDelay");
        }
    }

    IEnumerator FOVChange()
    {
        while (Camera.main.fieldOfView > GameoverFOV)
        {
            yield return null;
            Camera.main.fieldOfView -= GameoverFOVSpeed * Time.unscaledDeltaTime;
        }
    }

    IEnumerator TransitionDelay()
    {
        yield return new WaitForSecondsRealtime(transitionDelay);
        while (transitionCG.alpha != 1)
        {
            yield return null;
            transitionCG.alpha += transitionSpeed * Time.unscaledDeltaTime;
        }

        //pass score info
        ScoreManager.Instance.SaveScoreToPlayerPref();
        SceneManager.LoadScene("GameOver");
    }
}
