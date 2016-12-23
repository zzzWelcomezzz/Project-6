using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityStandardAssets.Characters.FirstPerson;

public class ScrapStoreManager : MonoBehaviour
{
    public Text StoreName;
    public Text ScrapInfo;
    public Text ErrorMessage;
    public Canvas StoreCanvas;
    public Transform LayoutGroup;

    private FirstPersonController FPSController;
    private List<StoreItem> StoreItems;
    private bool isOpen;

    // Use this for initialization
    void Start()
    {
        isOpen = false;
        FPSController = GameObject.Find("FPSController").GetComponent<FirstPersonController>();
        UpdateStoreUI();
        ToggleStoreUI();
        

        SetStoreItems();
        SetLayoutGroupContent();
    }

    void SetStoreItems()
    {
        StoreItems = new List<StoreItem>();

        StoreItems.Add(new StoreItem(Resources.Load<Sprite>("icon-crosshair"), "Revolver Ammo", "10 pieces of ammunition for the revolver", 5, 2.5f));
        StoreItems.Add(new StoreItem(Resources.Load<Sprite>("icon-crosshair"), "RayGun Ammo", "10 pieces of ammunition for the raygun", 25, 3.0f));
        StoreItems.Add(new StoreItem(Resources.Load<Sprite>("icon-crosshair"), "Revolver Damage +1", "Enhances the Revolver to do twice the damage.", 100, 7.5f));
        StoreItems.Add(new StoreItem(Resources.Load<Sprite>("icon-crosshair"), "RayGun Damage +1", "Enhances the RayGun to do twice the damage.", 200, 12.5f));
        StoreItems.Add(new StoreItem(Resources.Load<Sprite>("icon-crosshair"), "Revolver Reload Speed +1", "Enhances the Revolver to reload in half the time.", 75, 12.5f));
        StoreItems.Add(new StoreItem(Resources.Load<Sprite>("icon-crosshair"), "RayGun Reload Speed +1", "Enhances the RayGun to reload in half the time.", 150, 12.5f));
    }

    private bool CanBuyItem(int price)
    {
        return price <= PlayerController.Inventory.ScrapCount;
    }

    private void HideError()
    {
        ErrorMessage.enabled = false;
    }

    void HandleButtonPress(int index, int price)
    {
        if(!CanBuyItem(price))
        {
            Debug.Log("can't afford");
            ErrorMessage.enabled = true;
            Invoke("HideError", 2.0f);

            return;
        }

        PlayerController.Inventory.SpendScrap(price);

        // bad, fix later
        switch (index)
        {
            case 0:
                PlayerController.Inventory.AddGunAmmo("Revolver", 10);                 
                break;
            case 1:
                PlayerController.Inventory.AddGunAmmo("Ray Gun", 10); 
                break;
            case 2:
                PlayerController.Inventory.AddGunDamage("Revolver", 2.0f); 
                break;
            case 3:
                PlayerController.Inventory.AddGunDamage("Ray Gun", 2.0f); 
                break;
            case 4:
                PlayerController.Inventory.ReduceGunReload("Revolver", 0.5f);
                break;
            case 5:
                PlayerController.Inventory.ReduceGunReload("Ray Gun", 0.5f);
                break;
        }
    }

    void SetLayoutGroupContent()
    {
        Debug.Log("in setlayoutgroupcontent");
        for (int i = 0; i < StoreItems.Count; i++)
        {
            GameObject curr = (GameObject)Instantiate(Resources.Load("StoreItem"));
            curr.name = "storeitem" + i;
            curr.transform.SetParent(LayoutGroup, false);

            StoreItem currStoreItem = StoreItems[i];

            curr.transform.Find("ItemImage").GetComponent<Image>().sprite = currStoreItem.image;                                                                              
            curr.transform.Find("ItemTitle").GetComponent<Text>().text = currStoreItem.title;
            curr.transform.Find("ItemDescription").GetComponent<Text>().text = currStoreItem.description;
            curr.transform.Find("ItemPrice").GetComponent<Text>().text = currStoreItem.price.ToString();

            int j = i;
            int price = currStoreItem.price;
            curr.transform.Find("MakeButton").GetComponent<Button>().onClick.AddListener(() => { HandleButtonPress(j, price); });
        }

        RectTransform layoutRectTransform = ((RectTransform)LayoutGroup);

        layoutRectTransform.sizeDelta = new Vector2(0.0f, 120 * StoreItems.Count);
    }

    // Update is called once per frame
    void Update()
    {
        UpdateStoreUI();

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            isOpen = !isOpen;
            ToggleStoreUI();
        }
    }

    void UpdateStoreUI()
    {
        if (isOpen)
        { 
            StoreName.text = string.Format("{0} Scrap", Utils.CompanyName);
            ScrapInfo.text = string.Format("Scrap Amount: {0}", PlayerController.Inventory.ScrapCount);
        }

    }

    void ToggleStoreUI()
    {
        GunController.UsingWeapon = !isOpen;
        FPSController.enabled = !isOpen;
        Cursor.lockState = CursorLockMode.None;//isOpen? CursorLockMode.None : Cursor.lockState;
        Cursor.visible = true;
        StoreCanvas.enabled = isOpen;
    }
}
