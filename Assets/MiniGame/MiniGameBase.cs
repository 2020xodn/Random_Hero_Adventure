using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MiniGameBase : MonoBehaviour {
    protected MiniGameManager minigameManager;
    // UI - Button
    [HideInInspector]
    public RectTransform btnStart;
    [HideInInspector]
    public RectTransform btnReStart;

    // UI - Text
    public TextMeshProUGUI txtResult;

    TextMeshProUGUI txtTimer;
    Image timerBar;

    protected float timeFull;
    [HideInInspector]
    public float timeLeft;
    [HideInInspector]
    public float timeElapsed;

    // Mode
    public int timeMode;    // 1 : 남은 시간, 2 : 흐른 시간

    [HideInInspector]
    public bool isRunning = false;

    // Level
    protected const float INITIAL_LEVEL = 3f;
    public float levelAdjust = 0;
    public float time = 0.0f;

    // Joy Stick
    protected Joystick joystick;
    protected GameObject joystick_Stick;
    protected GameObject joystick_Button;


    private void Start() {
        Init();
    }

    private void Update() {
        if (isRunning) {
            timeElapsed += Time.unscaledDeltaTime;
            setTimer();
        }
    }

    public virtual void Init() {
        btnStart = gameObject.transform.Find("Button Start").GetComponent<RectTransform>();
        btnStart.anchoredPosition = Vector3.zero;
        btnReStart = gameObject.transform.Find("Button ReStart").GetComponent<RectTransform>();
        btnReStart.anchoredPosition = new Vector3(0, 1000f, 0);
        txtTimer = gameObject.transform.Find("Timer Left").Find("Text Time Float").GetComponent<TextMeshProUGUI>();
        timerBar = gameObject.transform.Find("Timer Left").Find("Timer Bar").GetComponent<Image>();
        timerBar.fillAmount = 1.0f;

        minigameManager = GameObject.Find("MainManager").GetComponent<MiniGameManager>();

        timeFull = INITIAL_LEVEL + levelAdjust;

        joystick = GameObject.FindWithTag("JoyStick").GetComponent<Joystick>();
        joystick_Stick = joystick.gameObject;
        joystick_Button = joystick.gameObject.transform.parent.GetChild(1).gameObject;

        setTimer();
    }

    public virtual void GameStart() {
        Debug.Log(name + " 게임시작");
        minigameManager.isMiniGameRunning = true;
        Time.timeScale = 0.0f;
        //Time.timeScale = 0.000001f;
        resetTimer();
        isRunning = true;

        ClearJoyStick();

        SetJoyStick();
    }

    protected virtual void SetJoyStick() { 

    }

    public virtual void ClearJoyStick() {
        joystick_Stick.GetComponent<RectTransform>().anchoredPosition = new Vector3(-800, -2000, 0);
        joystick_Button.GetComponent<RectTransform>().anchoredPosition = new Vector3(0, -2000, 0);
    }

    public void clickStartBtn() {
        btnStart.anchoredPosition = new Vector3(0, 1000f, 0);
        btnReStart.anchoredPosition = new Vector3(0, 1000f, 0);
        GameStart();
    }

    public virtual void GameEnd() {
        btnStart.anchoredPosition = Vector3.zero;
        isRunning = false;

        minigameManager.clickBackButton();

        if (SceneManager.GetActiveScene().buildIndex != 0) {
            joystick_Stick.GetComponent<RectTransform>().anchoredPosition = new Vector3(-800, -200, 0);
            joystick_Button.GetComponent<RectTransform>().anchoredPosition = new Vector3(0, 0, 0);
        }
        else {
            ClearJoyStick();
        }
    }

    public virtual void ClearEverything() {

    }


    public virtual void setReStartBtn() {
        btnReStart.anchoredPosition = new Vector3(-750, -250, 0);
    }

    protected void setTimer() {
        float elapsed = Mathf.Round(timeElapsed * 10) * 0.1f;
        float left = Mathf.Round((timeFull - elapsed) * 10) * 0.1f;

        if (timeMode == 1) {    // 남은 시간 표시
            timerBar.fillAmount = (timeFull - timeElapsed) / timeFull;
            txtTimer.text = left.ToString();

            if (left <= 0.0f) {
                GameOverByTime();
            }
        }
        else if (timeMode == 2) {  // 남은 시간 표시 (버티기면 승리)
            timerBar.fillAmount = (timeFull - timeElapsed) / timeFull;
            txtTimer.text = left.ToString();

            if (left <= 0.0f) {
                GameClear();
            }
        }
        else {  // 추후 수정..  버티기류 경과 시간 표시
            timerBar.fillAmount = timeElapsed;
            txtTimer.text = elapsed.ToString();
        }

        if (txtTimer.text.Length == 1) {
            txtTimer.text += ".0";
        }
    }

    public void resetTimer() {
        timeElapsed = 0;
        setTimer();
    }

    public void GameOverByTime() {
        minigameManager.isMiniGameRunning = false;
        isRunning = false;
        txtResult.text = "시간 초과";
        if (SceneManager.GetActiveScene().buildIndex == 0) {    // 연습 씬
            setReStartBtn();
        }
        else {                                                  // 실제 미니게임
            StartCoroutine(GameEndDelay(false));
            levelAdjust++;  // 틀리면 난이도 하향
        }
    }

    public void GameOver() {
        minigameManager.isMiniGameRunning = false;
        txtResult.text = "게임 오버";

        if (isRunning) {
            isRunning = false;
            if (SceneManager.GetActiveScene().buildIndex == 0) {    // 연습 씬
                setReStartBtn();
            }
            else {                                                  // 실제 미니게임
                StartCoroutine(GameEndDelay(false));
                levelAdjust++;  // 틀리면 난이도 하향
            }
        }
    }

    public virtual void GameClear() {
        minigameManager.isMiniGameRunning = false;
        txtResult.text = "클리어";

        if (isRunning) {
            isRunning = false;
            minigameManager.transform.Find("Sound").Find("Clear Minigame").GetComponent<AudioSource>().Play();
            if (SceneManager.GetActiveScene().buildIndex == 0) {    // 연습 씬
                setReStartBtn();
            }
            else {                                                  // 실제 미니게임
                StartCoroutine(GameEndDelay(true));
            }
        }

    }


    protected IEnumerator GameEndDelay(bool isWin) {
        yield return new WaitForSecondsRealtime(2.0f);
        Init();
        GameEnd();
        minigameManager.ActionByMiniGame(isWin);
    }
}
