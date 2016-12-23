using UnityEngine;
using System.Collections;

public class TopDownCharacterController : MonoBehaviour 
{
    private const float MIN_HEIGHT = 5.0f, MAX_HEIGHT = 15.0f;
    private float speed = 7.5f;
    private float cameraHeight = 10.0f;
    private CharacterController character;
    public Camera playerCamera;

	// Use this for initialization
	void Start () 
    {
        character = GetComponent<CharacterController>();
        playerCamera.transform.Rotate(new Vector3(90.0f, 0.0f, 0.0f));
	}

    // Update is called once per frame
    void Update()
    {
        float zoom = Input.GetAxis("Mouse ScrollWheel");
        if(zoom < 0)
        {
            cameraHeight = Mathf.Clamp(cameraHeight + 1.0f, MIN_HEIGHT, MAX_HEIGHT);
        }
        else if(zoom > 0)
        {
            cameraHeight = Mathf.Clamp(cameraHeight - 1.0f, MIN_HEIGHT, MAX_HEIGHT);
        }

        character.SimpleMove(new Vector3(speed * Input.GetAxis("Horizontal"), 0.0f, speed * Input.GetAxis("Vertical")));


        //if(playerCamera.transform.position.x >= -12.0f)

        playerCamera.transform.position = new Vector3(this.transform.position.x, this.transform.position.y + cameraHeight, this.transform.position.z);

        if (Input.GetKeyDown(KeyCode.E))
            character.transform.position = new Vector3(this.transform.position.x, this.transform.position.y + 10.0f, this.transform.position.z);

        if (Input.GetKeyDown(KeyCode.Q))
            Debug.Log(character.transform.position);


    }
}
