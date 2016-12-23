using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class LevelNameManager : MonoBehaviour
{
    [SerializeField]
    private Text LevelText;

    void Start()
    {
        LevelText.text = string.Format("{0} floor {1}", Utils.CompanyName, Startup.FloorNumber + 1);
    }
}
