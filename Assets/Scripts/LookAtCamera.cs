﻿using UnityEngine;
using System.Collections;

public class LookAtCamera : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        transform.LookAt(Camera.main.transform.position);
        transform.eulerAngles = new Vector3(-90, transform.eulerAngles.y, 0);
    }
}
