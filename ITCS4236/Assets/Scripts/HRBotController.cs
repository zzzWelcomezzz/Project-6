using UnityEngine;
using System.Collections;

public class HRBotController : MonoBehaviour
{
    [SerializeField]
    private Transform playerTransform;
    [SerializeField]
    private GameObject neck;
    public Vector3 angles;

    private float rotationSpeed = 0.25f;

    // Use this for initialization
    void Start()
    {
        //neck = transform.Find("Neck").GetComponent<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
        //transform.rotation = Quaternion.LookRotation(playerTransform.forward);
        //neck.transform.rotation = Quaternion.LookRotation(playerTransform.localPosition);
        //neck.transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(playerTransform.position),
        //   rotationSpeed * Time.deltaTime);

        neck.transform.LookAt(playerTransform.position, playerTransform.up);
        neck.transform.Rotate(angles);
    }
}
