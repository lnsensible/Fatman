using UnityEngine;
using System.Collections;

public class ChracterRestrict : MonoBehaviour {

    public float lowerX;
    public float upperX;
    public float lowerZ;
    public float upperZ;

    Rigidbody characterRigidbody;
    Transform characterTransform;

    TitleScreen titlemanager;

    bool restrict = false;

	void Start () {
        characterRigidbody = GetComponent<Rigidbody>();
        characterTransform = transform;
        titlemanager = FindObjectOfType<TitleScreen>();
	}

    public void startGame()
    {
        titlemanager.GameStarted = true;
        StartCoroutine("forceMove");
    }

    IEnumerator forceMove()
    {
        while (characterTransform.position.x < lowerX)
        {
            yield return null;
            if (!titlemanager.MoveRoad)
                characterRigidbody.velocity = new Vector3(5.0f, 0, 0);
        }

        titlemanager.inTitle = false;
        restrict = true;
    }
	
	void Update () {

        if (restrict)
        {
            float scale = transform.localScale.x * 0.5f;
            if (characterTransform.position.x < lowerX + scale)
            {
                characterTransform.position = new Vector3(lowerX + scale, characterTransform.position.y, characterTransform.position.z);
                characterRigidbody.velocity = new Vector3(0, characterRigidbody.velocity.y, characterRigidbody.velocity.z);
            }

            if (characterTransform.position.x > upperX - scale)
            {
                characterTransform.position = new Vector3(upperX - scale, characterTransform.position.y, characterTransform.position.z);
                characterRigidbody.velocity = new Vector3(0, characterRigidbody.velocity.y, characterRigidbody.velocity.z);
            }

            if (characterTransform.position.z < lowerZ + scale)
            {
                characterTransform.position = new Vector3(characterTransform.position.x, characterTransform.position.y, lowerZ + scale);
                characterRigidbody.velocity = new Vector3(characterRigidbody.velocity.x, characterRigidbody.velocity.y, 0);
            }

            if (characterTransform.position.z > upperZ - scale)
            {
                characterTransform.position = new Vector3(characterTransform.position.x, characterTransform.position.y, upperZ - scale);
                characterRigidbody.velocity = new Vector3(characterRigidbody.velocity.x, characterRigidbody.velocity.y, 0);
            }
        }
	}
}
