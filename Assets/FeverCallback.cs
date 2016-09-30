using UnityEngine;
using System.Collections;

public class FeverCallback : MonoBehaviour {

    public Shake[] shakes;
    public float stayTime;

    public void FinishedUnfever()
    {
        for (int i = 0; i < shakes.Length; ++i)
        {
            shakes[i].enabled = false;
        }

        StartCoroutine("UnfeverDisappear");
    }


    IEnumerator UnfeverDisappear()
    {
        float actualStayTime = stayTime;
        while (actualStayTime > 0.0f)
        {
            yield return null;
            actualStayTime -= Time.unscaledDeltaTime;
        }

        CharacterManager.Instance.endFever();
        gameObject.SetActive(false);
    }

    public void FinishedAnim()
    {
        for (int i = 0; i < shakes.Length; ++i)
        {
            shakes[i].enabled = true;
        }

        StartCoroutine("Disappear");
    }

    IEnumerator Disappear()
    {
        float actualStayTime = stayTime;
        while (actualStayTime > 0.0f)
        {
            yield return null;
            actualStayTime -= Time.unscaledDeltaTime;
        }

        for (int i = 0; i < shakes.Length; ++i)
        {
            shakes[i].enabled = false;
        }
        
        CharacterManager.Instance.StartFever();
        gameObject.SetActive(false);
    }
}
