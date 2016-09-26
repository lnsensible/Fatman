﻿using UnityEngine;
using System.Collections;

public class CharacterMovement : MonoBehaviour {

    public float speed;
    public bool knockback;
	// Use this for initialization
	void Start () {
	
	}

    public void Knocked()
    {
        knockback = true;
        StartCoroutine("Knockedback");
    }

    IEnumerator Knockedback()
    {
        yield return new WaitForSeconds(0.2f);
        knockback = false;
    }
	// Update is called once per frame
	void Update () {
        if (!knockback)
        {
            float moveHorizontal = Input.GetAxis("Horizontal");
            float moveVertical = Input.GetAxis("Vertical");

            Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);
            GetComponent<Rigidbody>().velocity = movement * speed;

            if (moveHorizontal != 0 || moveVertical != 0)
                transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(movement), 0.5f);
        }
        
	}

}