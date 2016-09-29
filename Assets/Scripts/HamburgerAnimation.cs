using UnityEngine;
using System.Collections;

public class HamburgerAnimation : MonoBehaviour {

    public float rotationSpeed;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        transform.localEulerAngles += new Vector3(0, rotationSpeed * Time.deltaTime, 0);
	}
}
