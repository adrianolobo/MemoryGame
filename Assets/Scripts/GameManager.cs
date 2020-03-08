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

    public AudioClip errorSound;
    private AudioSource audioSource;

    public PanelRenderer ui;
    public PanelScaler panelScaler;

    private VisualElement screenElem;
    private Button startBtn;
    private Button closeBtn;
    private Label scoreValue;
    private Label highScoreValue;

    private void Awake()
    {
        ui.postUxmlReload = BindUI;
    }

    private IEnumerable<Object> BindUI()
    {
        var root = ui.visualTree;
        screenElem = root.Q<VisualElement>("screen");

        startBtn = root.Q<Button>("start-btn");
        startBtn.clickable.clicked += () =>
        {
            SetScore(0);
            Play();
            screenElem.AddToClassList("in-game");
        };

        closeBtn = root.Q<Button>("close-btn");
        closeBtn.clickable.clicked += () =>
        {
            Application.Quit();
        };

        scoreValue = root.Q<Label>("current-score");
        highScoreValue = root.Q<Label>("high-score");
        CheckAndSetHighScore();
        return null;
    }

    private void RemoveInGame()
    {
        screenElem.RemoveFromClassList("in-game");
    }
    // Start is called before the first frame update
    void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
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

    private void CheckAndSetHighScore()
    {
        int currentHighScore = PlayerPrefs.GetInt("high-score");
        int currentScore = getCurrentScore() - 1;
        int highScore = currentScore > currentHighScore ? currentScore : currentHighScore;
        PlayerPrefs.SetInt("high-score", highScore);
        highScoreValue.text = $"High Score: {highScore.ToString()}";
    }

    private void SetScore(int value)
    {
        scoreValue.text = $"Score: {value.ToString()}";
    }

    void GameOver()
    {
        CheckAndSetHighScore();
        GameEvents.current.BlockUserInput();
        audioSource.PlayOneShot(errorSound);
        buttonsSequence = new List<SimonButton>();
        RemoveInGame();
    }

    IEnumerator PlayButtonsSequence()
    {
        yield return new WaitForSeconds(0.2f);
        for (int i = 0; i < buttonsSequence.Count; i++)
        {
            yield return new WaitForSeconds(0.3f);
            buttonsSequence[i].SetLight();
            yield return new WaitForSeconds(0.5f);
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
            SetScore(getCurrentScore());
            Play();
        }
    }

    private int getCurrentScore()
    {
        return buttonsSequence.Count;
    }
}
