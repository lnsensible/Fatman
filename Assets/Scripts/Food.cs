using UnityEngine;
using System.Collections;

public class Food : MonoBehaviour
{
    public int foodPoint;
    private PlayerScript player;

    // Use this for initialization
    void Start()
    {
        player = FindObjectOfType<PlayerScript>();
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.tag == "Player")
        {
            player.Eat(foodPoint);
            MusicManager.Instance.PlaySound(MusicManager.soundlist_eat);
            Destroy(gameObject);
        }
    }

}
