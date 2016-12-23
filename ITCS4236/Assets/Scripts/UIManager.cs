using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    private static Color DefaultScrapTextColor;
    private static Text ScrapCount;
    public static RawImage crosshair;

    // Use this for initialization
    void Start()
    {
        ScrapCount = GameObject.Find("ScrapText").GetComponent<Text>();
        crosshair = GameObject.Find("Crosshair").GetComponent<RawImage>();
        DefaultScrapTextColor = ScrapCount.color;
    }

    // Update is called once per frame
    void Update()
    {
        ScrapCount.text = PlayerController.Inventory.ScrapCount.ToString();
    }

    public void SetScrapColors(Color color, float seconds)
    {
        StartCoroutine(SetColorForSeconds(color, 1.0f));
    }

    private IEnumerator SetColorForSeconds(Color color, float seconds)
    {
        ScrapCount.color = color;
        yield return new WaitForSeconds(seconds);
        ScrapCount.color = DefaultScrapTextColor;
    }

    public void OnRestartYesClicked()
    {
        LoadingScreenManager.LoadScene(Utils.FLOOR_SCENE);
    }

    public void OnRestartNoClicked()
    {
        LoadingScreenManager.LoadScene(Utils.MENU_SCENE);
    }
}
