using UnityEngine;
using System.Collections;

public class Food : MonoBehaviour
{
  [SerializeField]
  private int foodPoint;
  private PlayerScript player;

  public int FoodPoint {
    get { return foodPoint; }
  }

  // Use this for initialization
  void Start()
  {
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
  }

}
