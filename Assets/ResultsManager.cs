using UnityEngine;
using System.Collections;

public class ResultsManager : MonoBehaviour {

    public float roadSpeed;
    public GameObject fakeRoad;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

            fakeRoad.transform.position -= new Vector3(roadSpeed * Time.deltaTime, 0, 0);
	}
}
