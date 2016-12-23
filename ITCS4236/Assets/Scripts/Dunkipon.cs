using UnityEngine;
using System.Collections;

public class Dunkipon : MonoBehaviour
{

    public float range;
    public float turnSpeed;
    public float fireRate;
    public int damage;
    public AudioSource source;
    public AudioClip shootSound;
    public AudioClip reloadSound;
    public GameObject target;
    Quaternion restingRotation;

    // Use this for initialization
    void Start()
    {
        restingRotation = transform.rotation;
        target = GameObject.Find("FPSController");
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 lookAt = target.transform.position - transform.position;

        if (lookAt.magnitude < range)
        {           
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(lookAt), turnSpeed);

            RaycastHit shotHit = new RaycastHit();
            if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out shotHit))
            {
                Debug.DrawLine(transform.position, shotHit.point, Color.red, 10.0f, false);

                if(shotHit.collider.tag == "Player")
                {
                    PlayerController.DamagePlayer(damage);
                }
            }
        }
        else
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, restingRotation, turnSpeed);
        }
    }
}
