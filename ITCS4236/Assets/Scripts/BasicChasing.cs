using UnityEngine;
using System.Collections;

public class BasicChasing : MonoBehaviour
{
    public float moveSpeed = 3.0f; //move speed
    public float rotationSpeed = 3.0f; //speed of turning
    public float range = 5.0f;
    public float range2 = 100.0f;
    public float stop = 0.0f;

    private Transform target; //the enemy's target
    private Transform myTransform; //current transform data of this enemy

    private float hitRadius = 1.0f;
    private bool isHittingPlayer;
    private int frameCounter = 0;


    void Start()
    {
        myTransform = transform; //cache transform data for easy access/preformance
        target = GameObject.FindWithTag("Player").transform; //target the player
    }

    void Update()
    {
        //rotate to look at the player
        var distance = Vector3.Distance(myTransform.position, target.position);

        if (distance <= range2 && distance >= range)
        {
            myTransform.rotation = Quaternion.Slerp(myTransform.rotation,
            Quaternion.LookRotation(target.position - myTransform.position), rotationSpeed * Time.deltaTime);
        }

        else if (distance <= range && distance > stop)
        {
            //move towards the player
            myTransform.rotation = Quaternion.Slerp(myTransform.rotation,
            Quaternion.LookRotation(target.position - myTransform.position), rotationSpeed * Time.deltaTime);
            myTransform.position += myTransform.forward * moveSpeed * Time.deltaTime;
        }
        else if (distance <= stop)
        {
            myTransform.rotation = Quaternion.Slerp(myTransform.rotation,
            Quaternion.LookRotation(target.position - myTransform.position), rotationSpeed * Time.deltaTime);
        }

        isHittingPlayer = distance < hitRadius;
    }

    void OnTriggerStay(Collider collider)
    {
        int msTime = (int)(Time.time * 1000f);

        if (collider.tag == "Player" && msTime % 100 == 0)
            PlayerController.DamagePlayer(20);
    }


}
