using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField]
    private InputField mainMenuInput;

    [SerializeField]
    private Text titleText;

    private static float waitPerChar = 0.05f;
    private static string menuText = "> A.I. - The Game\n> 1. Play\n> 2. Instructions\n> 3. Credits\n> 4. Quit\n> Enter your choice\n>";
    private static string instructionsText = "> How to play\n> first\n> second\n\n> 1. Go back\n> Enter your choice\n>";
    private static string creditsText = "> Chase Schelthoff\n> Akhil Ramlakan\n> Phong Dang Nguyen\n\n> 1. Go back\n> Enter your choice\n>";

    private enum MenuState { Normal, Play, Instructions, Credits};
    private MenuState state;
    private AudioSource errorAudio;

    // Use this for initialization
    void Start()
    {
        mainMenuInput.Select();
        state = MenuState.Normal;
        errorAudio = GetComponent<AudioSource>();

        StartCoroutine(DisplayTextWithWait(menuText, waitPerChar));
    }

    // Update is called once per frame
    void Update()
    {
        HandleInput();
    }

    private void HandleInput()
    {
        if(Input.GetKeyDown(KeyCode.Return))
        {
            switch(state)
            {
                case MenuState.Normal:
                    HandleNormal();
                    break;
                case MenuState.Play:
                    return;
                case MenuState.Instructions:
                    HandleOther();
                    break;
                case MenuState.Credits:
                    HandleOther();
                    break;
                default:
                    break;
            }
        }
    }

    private void HandleOther()
    {
        switch(mainMenuInput.text)
        {
            case "1":
                state = MenuState.Normal;
                StartCoroutine(DisplayTextWithWait(menuText, waitPerChar));
                mainMenuInput.text = string.Empty;
                mainMenuInput.Select();               
                break;
            default:
                errorAudio.Play();
                break;
        }

        SelectInput();
    }

    private IEnumerator DisplayTextWithWait(string text, float waitPerChar)
    {
        mainMenuInput.enabled = false;
        titleText.text = string.Empty;
        foreach (var character in text)
        {
            titleText.text += character;
            yield return new WaitForSeconds(waitPerChar);
        }

        mainMenuInput.enabled = true;
        SelectInput();
    }

    private void HandleNormal()
    {
        switch (mainMenuInput.text)
        {
            case "1":
                LoadingScreenManager.LoadScene(Utils.LOBBY_SCENE);
                state = MenuState.Play;
                break;
            case "2":
                state = MenuState.Instructions;
                mainMenuInput.text = string.Empty;
                StartCoroutine(DisplayTextWithWait(instructionsText, waitPerChar));
                break;
            case "3":
                state = MenuState.Credits;
                mainMenuInput.text = string.Empty;
                StartCoroutine(DisplayTextWithWait(creditsText, waitPerChar));
                break;
            case "4":
                Application.Quit();
                break;
            default:
                Debug.Log("bad!!!!@@@");
                mainMenuInput.Select();
                errorAudio.Play();
                break;
        }

        SelectInput();
    }

    private void SelectInput()
    {
        mainMenuInput.ActivateInputField();
        mainMenuInput.Select();
    }
}
