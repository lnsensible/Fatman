using UnityEngine;
using System.Collections;

public class ComboScoreDisplay : MonoBehaviour {

    public float fadeSpeed;
    public float showTime;
    public float upwardsSpeed;
    private CanvasGroup cg;

    Vector3 upwardsVector;

    void Start()
    {
        cg = GetComponent<CanvasGroup>();
        StartCoroutine("Fade");
        upwardsVector = new Vector3(0, upwardsSpeed, 0);
    }

    void Update()
    {
        transform.position += upwardsVector * Time.deltaTime;
    }

    IEnumerator Fade()
    {
        yield return new WaitForSeconds(showTime);
        

        while (cg.alpha > 0)
        {
            yield return null;
            cg.alpha -= fadeSpeed * Time.deltaTime;
        }

        Destroy(gameObject);
    }
}
