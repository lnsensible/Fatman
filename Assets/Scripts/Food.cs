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
            ScoreManager.Instance.ateFood();
            player.Eat(foodPoint);
            if (MusicManager.Instance)
                MusicManager.Instance.PlaySound(MusicManager.soundlist_eat);
            GetComponentInParent<LeftoverParticle>().Remove();
            Destroy(gameObject);
           
        }
    }

}
