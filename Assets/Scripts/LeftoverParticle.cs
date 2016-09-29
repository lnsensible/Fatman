using UnityEngine;
using System.Collections;

public class LeftoverParticle : MonoBehaviour {

    public float scaleSpeed;
    public float appearTime;
    public float appearSpeed;

    public float starteffectTime;

    [SerializeField]
    ParticleSystem leftoverPS;
    [SerializeField]
    ParticleSystem spawnPS;

    [SerializeField]
    GameObject food;

    public void Remove()
    {
        StartCoroutine("ScaleOut");
    }

    public void spawn()
    {
        food.transform.localScale = new Vector3(0, 0, 0);

        leftoverPS.Stop();
        spawnPS.Play();
        StartCoroutine("ScaleIn");
        StartCoroutine("EffectAppear");
    }

    IEnumerator EffectAppear()
    {
        while (starteffectTime > 0)
        {
            yield return null;
            starteffectTime -= Time.deltaTime;
        }

        leftoverPS.Play();
    }

    IEnumerator ScaleIn()
    {
        while (appearTime > 0)
        {
            yield return null;
            appearTime -= Time.deltaTime;
        }

        Vector3 scale = new Vector3(appearSpeed, appearSpeed, appearSpeed);
        while (food.transform.localScale.x < 1)
        {
            yield return null;
            food.transform.localScale += scale * Time.deltaTime;
        }

        spawnPS.Stop();
        spawnPS.loop = false;
    }

    IEnumerator ScaleOut()
    {
        Vector3 scale = new Vector3(scaleSpeed, scaleSpeed, scaleSpeed);
        while (transform.localScale.x > 0)
        {
            yield return null;
            transform.localScale -= scale * Time.deltaTime;
        }

        Destroy(gameObject);
    }
}
