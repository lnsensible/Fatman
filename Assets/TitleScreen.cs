using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

public class TitleScreen : MonoBehaviour {

    public bool inTitle = true;
    public bool GameStarted = false;
    public bool MoveRoad = true;

    public Button startbutton;

    Transform player;
    Camera maincam;

    public float roadSpeed;
    public GameObject fakeRoad;

    public float roadScaleSpeed;

	// Use this for initialization
	void Start () {
        maincam = Camera.main;
        player = GameObject.FindGameObjectWithTag("Player").transform;
        maincam.transform.LookAt(player.transform);
    }

    void Update()
    {
        if (MoveRoad)
        {
            fakeRoad.transform.position -= new Vector3(roadSpeed, 0, 0);
        }
    }

    public void DropRoad()
    {
        MoveRoad = false;
        StartCoroutine("removepath");
    }

    IEnumerator removepath(){
        var roads = GameObject.FindGameObjectsWithTag("FakeRoad").AsEnumerable();
        roads = roads.OrderBy(road => road.transform.position.x);

        Vector3 scalevector = new Vector3(roadScaleSpeed, 0, roadScaleSpeed);
        List<GameObject> sortedRoads = roads.ToList<GameObject>();
        while (sortedRoads.Count > 0)
        {
            yield return null;
            sortedRoads[0].transform.localScale -= scalevector;

            if (sortedRoads[0].transform.localScale.x <= 0.0f)
            {
                Destroy(sortedRoads[0]);
                sortedRoads.RemoveAt(0);
            }
            if (sortedRoads.Count > 0 && sortedRoads[0].transform.localScale.x < 0.5f && sortedRoads.Count > 1)
            {
                sortedRoads[1].transform.localScale -= scalevector;
            }
        }
    }

    public void StartGame()
    {
        startbutton.gameObject.SetActive(false);
        maincam.GetComponent<SmoothFollow>().enabled = true;
    }
}
