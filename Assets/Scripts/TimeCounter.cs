using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TimeCounter : MonoBehaviour
{
  private TitleScreen titleScreen;
  private float startingTime;
  [SerializeField]
  private float elapsedTimeFromStarting;
  private Text time;

  public float ElapsedTime
  {
    get { return elapsedTimeFromStarting; }
  }

  void Start()
  {
    titleScreen = FindObjectOfType<TitleScreen>();
    time = GetComponent<Text>();
  }

  void Update()
  {
    if (titleScreen.inTitle == true)
    {
      startingTime = Time.time;
    }
    else
    {
      elapsedTimeFromStarting = Time.time - startingTime;
      int min = (int)(ElapsedTime / 60) % 60;
      int sec = (int)(ElapsedTime) % 60;
      int msec = (int)(ElapsedTime * 100) % 100;
      time.text = min.ToString("D2") + ":" + sec.ToString("D2") + ":" + msec.ToString("D2");
    }
  }

}
