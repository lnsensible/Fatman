using UnityEngine;
using System.Collections;

public class Banana : MonoBehaviour {

    PlayerScript player;
	// Use this for initialization
	void Start () {
	    player = FindObjectOfType<PlayerScript>();
	}

    void OnTriggerEnter(Collider col)
    {
        if (col.tag == "Player" && !player.holdingFood)
        {
            player.HoldFood(gameObject);
            transform.SetParent(col.transform, true);
            Destroy(GetComponent<LookAtCamera>());
        }
        else if (col.tag == "Enemy")
        {
            if (player.foodHeld == gameObject)
                player.DroppedFood();
            Destroy(gameObject);
        }
    }
}
