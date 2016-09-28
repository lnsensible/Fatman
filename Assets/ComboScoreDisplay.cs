using UnityEngine;
using System.Collections;

public class ComboScoreDisplay : MonoBehaviour {

    public float fadeSpeed;
    public float showTime;
    private CanvasGroup cg;

    void Start()
    {
        cg = GetComponent<CanvasGroup>();
        StartCoroutine("Fade");
    }

    IEnumerator Fade()
    {
        yield return new WaitForSeconds(showTime);
        while (cg.alpha > 0)
        {
            yield return null;
            cg.alpha -=fadeSpeed * Time.deltaTime;
        }

        Destroy(gameObject);
    }
}
