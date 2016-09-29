using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TitleCountdown : MonoBehaviour {

    public float HoldDTime;
    public float timerInterval;

    public float biggestScale;
    public float smallestScale;

    public float fadeoutSpeed;

    public void start()
    {
        StartCoroutine("Countdown");
    }

    IEnumerator Countdown()
    {
        Text text = GetComponent<Text>();
        while (HoldDTime > 0.0f)
        {
            yield return null;
            HoldDTime -= Time.deltaTime;
        }

        text.text = "3";

        while (text.text.ToString() != "0")
        {
            float t = 0;
            float scale = biggestScale;
            while (transform.localScale.x > smallestScale)
            {
                yield return null;
                t += Time.deltaTime / timerInterval;
                scale = Mathf.MoveTowards(biggestScale, smallestScale, t);
                transform.localScale = new Vector3(scale, scale, scale);
            }

            transform.localScale = new Vector3(biggestScale, biggestScale, biggestScale);
            text.text = (System.Int32.Parse(text.text) - 1).ToString();
        }

        CanvasGroup cg = GetComponentInParent<CanvasGroup>();
        while (cg.alpha > 0)
        {
            yield return null;
            cg.alpha -= Time.deltaTime * fadeoutSpeed;
        }

        gameObject.SetActive(false);
    }
}
