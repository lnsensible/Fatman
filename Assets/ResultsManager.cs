using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class ResultsManager : MonoBehaviour {

    public float roadSpeed;
    public GameObject fakeRoad;

    public float fadeTime;
    public CanvasGroup screenHider;

	// Use this for initialization
	void Start () {
        Time.timeScale = 1;
        screenHider.alpha = 1;
        screenHider.blocksRaycasts = true;
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
        StartCoroutine("Fadein");
    }
}
