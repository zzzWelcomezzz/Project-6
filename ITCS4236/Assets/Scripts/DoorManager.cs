using UnityEngine;
using System.Collections;

public class DoorManager : MonoBehaviour
{
    private static readonly Vector3 AMOUNT_TO_RAISE = new Vector3(0.0f, 6.0f, 0.0f);

    private static Transform playerTransform;
    private static Vector3 rmHeight;
    private Transform door;
    private AudioSource audio;

    private Vector3 initialPosition, goalPosition;
    private Vector3 lerpStart, lerpEnd;

    [SerializeField]
    private static float RADIUS = 10.0f;

    [SerializeField]
    private float Speed = 2.0f;

    private bool isLerping, isComplete, initialAway;

    private bool doorMoved { get { return door.position != initialPosition; } }

    private float initialTime;

    // Use this for initialization
    void Start()
    {        
        playerTransform = GameObject.Find("FPSController").GetComponent<Transform>();
        audio = GetComponent<AudioSource>();
        door = transform.Find("door");
        initialPosition = door.position;
        goalPosition = initialPosition + AMOUNT_TO_RAISE;
        rmHeight = new Vector3(1.0f, 0.0f, 1.0f);
        isLerping = isComplete = initialAway = false;
    }

    void StartLerp(Vector3 initial, Vector3 goal)
    {
        isLerping = true;
        initialTime = Time.time;

        lerpStart = initial;
        lerpEnd = goal;
    }

    // Update is called once per frame
    void Update()
    {
        float distance = Utils.DistanceIgnoreHeight(transform.position, playerTransform.position);

        bool moved = false;
        
        if (distance < RADIUS)
        {
            if(!isLerping && !isComplete)
            {
                StartLerp(initialPosition, goalPosition);
                initialAway = false;
                moved = true;
            }            
        }
        else if(distance >= RADIUS)
        {
            if(!initialAway)
            {
                isLerping = isComplete = false;
                initialAway = true;

                if(doorMoved)
                {
                    StartLerp(door.position, initialPosition);
                    moved = true;
                }
                    
            } 
        }

        if (moved)
            audio.Play();
    }

    void FixedUpdate()
    {
        if(isLerping)
        {
            float timeSinceStarted = Time.time - initialTime;
            float percentComplete = timeSinceStarted / (1.0f / Speed);

            door.position = Vector3.Lerp(lerpStart, lerpEnd, percentComplete);

            if(percentComplete >= 1.0f)
            {
                isLerping = false;
                isComplete = !initialAway;
            }
        }
    }

    
}
