using UnityEngine;
using System.Collections;

public class Shake : MonoBehaviour {

    public float amount = 1.0f; //how much it shakes

    float myrandomseed;

    Vector3 original;

	// Use this for initialization
	void Start () {
        original = transform.localPosition;
        myrandomseed = Random.Range(0.0f, 100.0f);
	}
	
	// Update is called once per frame
	void Update () {

        transform.localPosition = new Vector3(original.x + (Mathf.Sin(myrandomseed) * amount), original.y, original.z);
        myrandomseed += Random.Range(0, 50.0f);
	}
}
