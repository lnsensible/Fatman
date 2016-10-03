using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

public class TitleScreen : MonoBehaviour {

    public bool inTitle = true;
    public bool GameStarted = false;
    public bool MoveRoad = true;

    Transform player;
    Camera maincam;

    public float roadSpeed;
    public GameObject fakeRoad;

    public float roadScaleSpeed;

    public CanvasGroup[] UIFadeIn;
    public CanvasGroup[] UIFadeOut;
    public float FadeinSpeed;

    public TitleCountdown instructioncountdown;

    public Text Highscore;

    public CanvasGroup ScreenHider;
    public float FadeOutSpeed;

	// Use this for initialization
	void Start () {

        ScreenHider.alpha = 1;
        ScreenHider.blocksRaycasts = true;
        maincam = Camera.main;
        player = GameObject.FindGameObjectWithTag("Player").transform;
        maincam.transform.LookAt(player.transform);
        Highscore.text += PlayerPrefs.GetInt("HIGHSCORE", 0);

        StartCoroutine("Fadeout");

        MusicManager.Instance.PlayBGM(MusicManager.soundlist_bgm);
    }

    void Update()
    {
        if (MoveRoad)
        {
            fakeRoad.transform.position -= new Vector3(roadSpeed * Time.deltaTime, 0, 0);
        }
    }

    public void DropRoad()
    {
        MoveRoad = false;
        StartCoroutine("removepath");
    }

    IEnumerator removepath(){
        var roads = GameObject.FindGameObjectsWithTag("FakeRoad").AsEnumerable();
        roads = roads.OrderBy(road => road.transform.position.x);

        List<GameObject> sortedRoads = roads.ToList<GameObject>();
        while (sortedRoads.Count > 0)
        {
            yield return null;
            sortedRoads[0].transform.localScale = Vector3.MoveTowards(sortedRoads[0].transform.localScale, Vector3.zero, roadScaleSpeed * Time.deltaTime);

            if (sortedRoads[0].transform.localScale.x <= 0.0f)
            {
                Destroy(sortedRoads[0]);
                sortedRoads.RemoveAt(0);
            }
            if (sortedRoads.Count > 0 && sortedRoads[0].transform.localScale.x < 0.5f && sortedRoads.Count > 1)
            {
                sortedRoads[1].transform.localScale = Vector3.MoveTowards(sortedRoads[1].transform.localScale, Vector3.zero, roadScaleSpeed * Time.deltaTime);
            }
        }
    }

    public void StartGame()
    {
        if (!maincam.GetComponent<SmoothFollow>().enabled)
        {
            MusicManager.Instance.PlayBGM(MusicManager.soundlist_bgm_game);
            MusicManager.Instance.PlaySound(MusicManager.soundlist_bgm_select);
            maincam.GetComponent<SmoothFollow>().enabled = true;
            StartCoroutine("FadeUI");
            instructioncountdown.start();
        }
    }

    IEnumerator FadeUI()
    {
        while (UIFadeIn[0].alpha != 1)
        {
            yield return null;
            for (int i = 0; i < UIFadeIn.Length; ++i)
            {
                UIFadeIn[i].alpha += FadeinSpeed * Time.deltaTime;
            }
            for (int i = 0; i < UIFadeOut.Length; ++i)
            {
                UIFadeOut[i].alpha -= FadeinSpeed * Time.deltaTime;
            }

        }

        for (int i = 0; i < UIFadeOut.Length; ++i)
        {
            UIFadeOut[i].gameObject.SetActive(false);
        }
    }

    IEnumerator Fadeout()
    {
        while (ScreenHider.alpha > 0)
        {
            yield return null;
            ScreenHider.alpha -= FadeOutSpeed * Time.deltaTime;
        }

        ScreenHider.blocksRaycasts = false;
    }

    public void Quit()
    {
        MusicManager.Instance.PlaySound(MusicManager.soundlist_bgm_quit);
        Application.Quit();

    }
}
