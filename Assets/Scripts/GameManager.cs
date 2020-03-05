using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.UIElements.Runtime;
using UnityEngine.UIElements;

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

    public PanelRenderer ui;

    private Button startBtn;
    private Label scoreValue;

    private void OnEnable()
    {
        ui.postUxmlReload = BindUI;
    }

    private IEnumerable<Object> BindUI()
    {
        var root = ui.visualTree;
        startBtn = root.Q<Button>("start-btn");
        startBtn.clickable.clicked += () =>
        {
            SetScore(0);
            Play();
            startBtn.AddToClassList("hide");
        };

        scoreValue = root.Q<Label>("score-value");
        return null;
    }

    private void ShowStartBtn()
    {
        startBtn.RemoveFromClassList("hide");
    }
    // Start is called before the first frame update
    void Start()
    {
        GameEvents.current.BlockUserInput();
        SimonRedButton = redButton.GetComponent<SimonButton>();
        SimonGreenButton = greenButton.GetComponent<SimonButton>();
        SimonYellowButton = yellowButton.GetComponent<SimonButton>();
        SimonBlueButton = blueButton.GetComponent<SimonButton>();

        GameEvents.current.onButtonPressed += onButtonPressed;

        allButtons = new SimonButton[4] { SimonRedButton, SimonGreenButton, SimonBlueButton, SimonYellowButton };
    }

    void Play()
    {
        GameEvents.current.BlockUserInput();
        buttonsSequence.Add(GetRandomButton());
        playerButtonsSequence = new List<SimonButton>(buttonsSequence);
        StartCoroutine(PlayButtonsSequence());
    }

    private void SetScore(int value)
    {
        scoreValue.text = value.ToString();
    }

    void GameOver()
    {
        GameEvents.current.BlockUserInput();
        buttonsSequence = new List<SimonButton>();
        ShowStartBtn();
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
            SetScore(buttonsSequence.Count);
            Play();
        }
    }
}
