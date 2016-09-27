using UnityEngine;
using System.Collections;

public class FakeRoadReset : MonoBehaviour {

    public float ResetPosition;
    public float ResetDistance;

    TitleScreen titlemanager;

    void Start()
    {
        titlemanager = FindObjectOfType<TitleScreen>();
    }

	// Update is called once per frame
	void Update () {
	    if (transform.position.x <= ResetPosition)
        {
            transform.position = new Vector3(ResetDistance, transform.position.y, transform.position.z);
            if (titlemanager.GameStarted)
            {
                titlemanager.DropRoad();
            }
        }
	}
}
