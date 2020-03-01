using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject redButton;
    public GameObject greenButton;
    public GameObject yellowButton;
    public GameObject blueButton;

    private SimonButton SimonRedButton;
    private SimonButton SimonGreenButton;
    private SimonButton SimonBlueButton;
    private SimonButton SimonYellowButton;

    private SimonButton[] allButtons;

    private List<SimonButton> buttonsSequence = new List<SimonButton>();
    private List<SimonButton> playerButtonsSequence;
    // Start is called before the first frame update
    void Start()
    {
        SimonRedButton = redButton.GetComponent<SimonButton>();
        SimonGreenButton = greenButton.GetComponent<SimonButton>();
        SimonYellowButton = yellowButton.GetComponent<SimonButton>();
        SimonBlueButton = blueButton.GetComponent<SimonButton>();

        GameEvents.current.onButtonPressed += onButtonPressed;

        allButtons = new SimonButton[4] { SimonRedButton, SimonGreenButton, SimonBlueButton, SimonYellowButton };
        Play();
    }

    void Play()
    {
        GameEvents.current.BlockUserInput();
        buttonsSequence.Add(GetRandomButton());
        playerButtonsSequence = new List<SimonButton>(buttonsSequence);
        StartCoroutine(PlayButtonsSequence());
    }

    void GameOver()
    {
        buttonsSequence = new List<SimonButton>();
        Play();
    }

    IEnumerator PlayButtonsSequence()
    {
        for (int i = 0; i < buttonsSequence.Count; i++)
        {
            yield return new WaitForSeconds(0.5f);
            buttonsSequence[i].SetLight();
            yield return new WaitForSeconds(1.0f);
            buttonsSequence[i].SetDark();
        }
        GameEvents.current.AllowUserInput();
    }

    SimonButton GetRandomButton()
    {
        int randomIndex = Random.Range(0, allButtons.Length);
        return allButtons[randomIndex];
    }

    void onButtonPressed(SimonButton buttonPressed)
    {
        SimonButton currentPlay = playerButtonsSequence[0];
        playerButtonsSequence.RemoveAt(0);
        if (buttonPressed != currentPlay)
        {
            GameOver();
            return;
        }
        if (playerButtonsSequence.Count == 0)
        {
            Play();
        }
    }
}
