using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityStandardAssets.Characters.FirstPerson;

public class PlayerController : MonoBehaviour
{
    private static Text healthText, gameInfoText;
    private static Image healthBackgroundImage;
    private static RawImage crosshair;
    private static GameObject onDeathUI;

    public static Inventory Inventory { get; private set; }

    private static float HideTimer;

    private static int health;
    private static bool IsAlive { get { return health > 0; } }

    public static UIManager UIController;
    private FirstPersonController FPSController;

    void Start()
    {
        health = IsAlive? health : 100;
        Inventory = Inventory == null? new Inventory() : new Inventory(Inventory);

        healthBackgroundImage = GameObject.Find("UI_Background_Health").GetComponent<Image>();
        healthText = GameObject.Find("Health").GetComponent<Text>();
        //gameInfoText = GameObject.Find("GameMessageText").GetComponent<Text>();
        FPSController = GameObject.Find("FPSController").GetComponent<FirstPersonController>();
        UIController = GameObject.Find("GameUI").GetComponent<UIManager>();
        onDeathUI = GameObject.Find("OnDeath");
        crosshair = GameObject.Find("Crosshair").GetComponent<RawImage>();
        onDeathUI.SetActive(false);

        SetHealthUI();
    }

    void Update()
    {
        if(HideTimer > 0.0f)
        {
            Invoke("HideGameMessageText", HideTimer);
            HideTimer = 0.0f;            
        }

        if(Input.GetKeyDown(KeyCode.V)) // for debugging
        {
            DamagePlayer(5);
        }
        else if(Input.GetKeyDown(KeyCode.B))
        {
            Inventory.AddScrap(100);
        }
        else if(Input.GetKeyDown(KeyCode.X))
        {
            Room room = Startup.Map.GetPlayerRoom();
            Debug.Log("null? " + room == null);
            //room.GetRoom().transform.localRotation = Quaternion.Euler(0f, 90f + room.GetRoom().transform.rotation.y, 0f);
            //room.GetRoom().transform.localPosition += new Vector3(0f, 3f, 0f);
        }

        if(!IsAlive)
        {
            OnDeath();
        }
    }

    private void OnDeath()
    {
        GunController.UsingWeapon = false;
        FPSController.enabled = false;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        UIManager.crosshair.enabled = false;
        onDeathUI.SetActive(true);
    }

    private void HideGameMessageText()
    {
        gameInfoText.text = string.Empty;
    }

    private static void SetHealthUI()
    {
        healthText.text = IsAlive ? health.ToString() : "0";   

        if(HealthLessThanPercent(50))
        {
            Color background = healthBackgroundImage.color;
            float colorDifference = 1.0f / health;
            healthBackgroundImage.color = new Color(background.r + colorDifference, background.g - colorDifference, background.b - colorDifference);
        }
    }

    // TODO: There should be a UI controller responsible for this
    public static void ShowGameMessageFor(string message, float seconds)
    {
        gameInfoText.text = message;
        HideTimer = seconds;
    }

    private static bool HealthLessThanPercent(int percent)
    {
        return health < percent;
    }

    public static void DamagePlayer(int amount)
    {
        health -= amount;
        SetHealthUI();
    }
}
