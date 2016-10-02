using UnityEngine;
using System.Collections;

///<summary>
///Looks at a target
///</summary>
public class SmoothFollow : MonoBehaviour
{
    public GameObject character;
    public float height;
    public float damp;
    public float dist;

    float transitioning;
    public float transitionSpeed;

    public static Vector3 wantedPos;
    public static bool shaking;
    ChracterRestrict[] cr;

    void OnEnable()
    {
        transitioning = 0.0f;
    }

    void Start()
    {
        cr = FindObjectsOfType<ChracterRestrict>();
    }

    void LateUpdate()
    {
        Vector3 currentPos = character.transform.position;

        float currentHeight = this.transform.position.y;

        //wantedHeight should be the height of the car's position plus some distance
        float wantedHeight = character.transform.position.y + height;

        //lerp from currentHeight to wantedHeight
        currentHeight = Mathf.Lerp(currentHeight, wantedHeight, damp * Time.deltaTime);

        //add a distance between the car's position and the camera's desired position, in the x coordinate because
        //of how the x/z axes on the cars are reversed
        
        wantedPos = new Vector3(currentPos.x, currentHeight, currentPos.z - dist);

        //finally, set the camera's position to its desired position
        if (transitioning < 1.0f)
        {
            transitioning += Time.deltaTime * transitionSpeed;
            this.transform.position = Vector3.Lerp(this.transform.position, wantedPos, transitioning);
            if (transitioning >= 1.0f)
            {
                foreach (ChracterRestrict cr2 in cr)
                {
                    cr2.startGame();
                }
            }
        }
        else
        {
            if (!shaking)
            this.transform.position = wantedPos;
        }

        this.transform.LookAt(character.transform);
    }
}