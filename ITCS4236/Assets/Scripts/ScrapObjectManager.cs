using UnityEngine;
using System.Collections;

public class ScrapObjectManager : MonoBehaviour
{
    private static readonly float PICKUP_RADIUS = 2.0f;
    private static Transform PlayerTransorm;

    private int ScrapAmount;

    void Start()
    {
        if(PlayerTransorm == null)
            PlayerTransorm = GameObject.FindWithTag("Player").transform;
    }

    void SetScrapAmount(int amount)
    {
        ScrapAmount = amount;
    }

    void FixedUpdate()
    {
        if(Vector3.Distance(transform.position, PlayerTransorm.position) < PICKUP_RADIUS)
        {
            PlayerController.Inventory.AddScrap(ScrapAmount);
            PlayerController.UIController.SetScrapColors(Color.green, 1.5f);
            Destroy(gameObject);
        }
    }
}
