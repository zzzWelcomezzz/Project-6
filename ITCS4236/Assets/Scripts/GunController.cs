using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GunController : MonoBehaviour
{
    public const int DEFAULT_GUN_INDEX = 1;
    private static string[] weaponStrings = { "RayGun", "Revolver" };
    private static GameObject[] weapons = new GameObject[weaponStrings.Length];
    public static int currentGun = 0;

    public static bool UsingWeapon;
    
    public Text ammoText;
    public Text gunText;
    public Image ammoBackgroundImage;
    private bool isLerping;
    private float initialTime;
    private Vector3 lerpStart;
    private Vector3 lerpEnd;
    private Transform GunLerpTransform;
    private object isComplete;
    private bool initialAway;

    public static bool ReloadReadyToLerpBack;

    private float LerpSpeed = 12.5f;

    void Awake()
    {
        UsingWeapon = false;
    }

    void Start()
    {      
        for (int i = 0; i < weaponStrings.Length; i++)
        {
            weapons[i] = GameObject.Find(weaponStrings[i]);
        }

        UsingWeapon = false;

        if (UsingWeapon)
            ChangeWeapon(DEFAULT_GUN_INDEX);
        else
            HideWeapons();
    }

    public static void HideWeapons()
    {
        UIManager.crosshair.enabled = false;
        UsingWeapon = false;
        foreach(GameObject weapon in weapons)
            weapon.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        SetUI();

        if(!UsingWeapon)
            return;

        if(ReloadReadyToLerpBack)
        {
            ReloadReadyToLerpBack = false;
            Gun currGun = weapons[currentGun].GetComponent<Gun>();
            Transform currGunTransform = weapons[currentGun].transform;
            StartLerp(currGun.GetReloadLocation(), currGun.GetDefaultPos(), currGunTransform);
        }

        if (weapons[currentGun].GetComponent<Gun>().GetState().Equals("DEFAULT") || weapons[currentGun].GetComponent<Gun>().GetState().Equals("AIM"))
        {
            if (Input.GetButtonDown("Fire1"))
            {
                weapons[currentGun].GetComponent<Gun>().Shoot();
            }
            else if (Input.GetButtonDown("Reload"))
            {
                Gun currGun = weapons[currentGun].GetComponent<Gun>();
                currGun.Reload();

                if(currGun.GetClipSize() != currGun.GetMaxClipSize())
                    StartLerp(currGun.GetDefaultPos(), currGun.GetReloadLocation(), weapons[currentGun].transform);

            }

            if (Input.GetMouseButtonUp(1))
            {
                Gun currGun = weapons[currentGun].GetComponent<Gun>();
                currGun.Aim(false);

                Transform currGunTransform = weapons[currentGun].transform;                
                StartLerp(currGunTransform.localPosition, currGun.GetDefaultPos(), currGunTransform);
            }
        }

        if (weapons[currentGun].GetComponent<Gun>().GetState().Equals("DEFAULT"))
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                int changeTo = currentGun - 1 < 0 ? weapons.Length - 1 : currentGun - 1;
                ChangeWeapon(changeTo);
            }
            else if (Input.GetKeyDown(KeyCode.E))
            {
                int changeTo = currentGun + 1 > weapons.Length - 1 ? 0 : currentGun + 1;
                ChangeWeapon(changeTo);
            }

            if (Input.GetMouseButtonDown(1))
            {
                Gun currGun = weapons[currentGun].GetComponent<Gun>();
                currGun.Aim(true);
                StartLerp(currGun.GetDefaultPos(), currGun.GetAimingPos(), weapons[currentGun].transform);
            }
        }      
        
    }



    void StartLerp(Vector3 initial, Vector3 goal, Transform gunTransform)
    {
        isLerping = true;
        initialTime = Time.time;

        lerpStart = initial;
        lerpEnd = goal;
        GunLerpTransform = gunTransform;
    }

    void FixedUpdate()
    {
        if (isLerping)
        {
            float timeSinceStarted = Time.time - initialTime;
            float percentComplete = timeSinceStarted / (1.0f / LerpSpeed);

            GunLerpTransform.localPosition = Vector3.Lerp(lerpStart, lerpEnd, percentComplete);

            if (percentComplete >= 1.0f)
            {
                isLerping = false;
                isComplete = !initialAway;
            }
        }
    }

    private void SetUI()
    {
        if(!UsingWeapon)
        {
            ammoBackgroundImage.enabled = false;
            gunText.text = ammoText.text = string.Empty;
            return;
        }

        Gun gun = weapons[currentGun].GetComponent<Gun>();
        ammoText.text = string.Format("{0}/{1}", gun.GetClipSize(), gun.GetAmmo());
        ammoBackgroundImage.enabled = UsingWeapon;
        gunText.text = gun.GetWeaponName();
    }

    public static void ChangeWeapon(int num)
    {
        if (num < 0 || num >= weapons.Length)
            return;

        UIManager.crosshair.enabled = true;

        weapons[currentGun].SetActive(false);
        weapons[num].SetActive(true);
        currentGun = num;

        weapons[currentGun].GetComponent<Gun>().SetDefaultState();

        if (!UsingWeapon)
            UsingWeapon = true;
    }
}
