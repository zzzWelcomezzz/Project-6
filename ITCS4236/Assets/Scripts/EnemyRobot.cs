using UnityEngine;
using System.Collections;

public class EnemyRobot : MonoBehaviour
{
    public static GameObject ScrapObject;
    public int ScrapPerKill = 10;
    private float health = 100.0f;
    private bool alive { get { return health > 0.0f; } }

    // Use this for initialization
    void Start()
    {
        if (ScrapObject == null)
            ScrapObject = (GameObject) Resources.Load("ScrapObject");
    }

    // Update is called once per frame
    void Update()
    {
        if(!alive)
        {
            Die();
        }
    }

    void Die()
    {
        int amount = ScrapPerKill / 10;

        for(int i = 0; i < amount; i++)
        {
            Vector3 scrapOffset = Vector3.zero;
            if (amount > 1)
                scrapOffset = new Vector3(Random.value, Random.value, Random.value);

            GameObject scrap = (GameObject)Instantiate(ScrapObject, transform.position + scrapOffset, Quaternion.identity);
            scrap.SendMessage("SetScrapAmount", ScrapPerKill / amount);
        }
        

        Destroy(gameObject);
    }

    void DeductHealth(float damageAmount)
    {
        health -= damageAmount;
        Debug.Log(health);
    }
}
