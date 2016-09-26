using UnityEngine;
using System.Collections;

public class EnemyPingpong : MonoBehaviour {

    private float originalx;
	// Use this for initialization
	void Start () {
        originalx = transform.position.x;
	}
	
	// Update is called once per frame
	void Update () {
        transform.position = new Vector3(originalx + Mathf.PingPong(Time.time, 14) - 7, transform.position.y, transform.position.z);
	}
}
