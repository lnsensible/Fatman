using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HungerManager : MonoBehaviour {

    public Image hungerbar;
    public float hungrySpeed;

	// Update is called once per frame
	void Update () {

        hungerbar.fillAmount -= hungrySpeed * Time.deltaTime;
	}

    public void Eat()
    {
        hungerbar.fillAmount = 1;
    }
    
    public void Damaged()
    {
        hungerbar.fillAmount -= 0.25f;
    }
}
