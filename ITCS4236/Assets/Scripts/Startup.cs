using UnityEngine;
using System.Collections;

public class Startup : MonoBehaviour
{

    public bool GenerateMap = true;
    public static int FloorNumber { get; private set; }
    public static MapGenerator Map;

    // Use this for initialization
    void Start()
    {
        ++FloorNumber;

        if (GenerateMap)
            Map = new MapGenerator(Vector3.zero);        
    }

    public static void StartupFloor()
    {
        new MapGenerator(Vector3.zero);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (GunController.UsingWeapon)
                GunController.HideWeapons();
            else
                GunController.ChangeWeapon(GunController.currentGun);
        }
        else if(Input.GetKeyDown(KeyCode.Z))
        {
            LoadingScreenManager.LoadScene(Utils.FLOOR_SCENE);
        }
    }
}
