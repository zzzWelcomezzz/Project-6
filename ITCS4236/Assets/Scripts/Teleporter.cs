using UnityEngine;
using System.Collections;

public class Teleporter : MonoBehaviour
{
    private const float TELEPORT_RADIUS = 2.5f;

    [SerializeField]
    private bool active;

    [SerializeField]
    private Transform teleportingObject;

    // Use this for initialization
    void Start()
    {
        active = true;
    }

    // Update is called once per frame
    void Update()
    {
        if(active && Vector3.Distance(transform.position, teleportingObject.position) < TELEPORT_RADIUS)
        {
            LoadingScreenManager.LoadScene(Utils.FLOOR_SCENE);
        }
    }
}
