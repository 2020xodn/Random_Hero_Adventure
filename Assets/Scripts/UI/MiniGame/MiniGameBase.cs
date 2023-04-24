using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MiniGameBase : UI_Base {
    protected enum Texts {
        Text_Timer_Float,
        Text_Timer
    }

    protected enum Buttons {
        Button_Mini_Start,
        Button_Mini_Restart,
        Button_Back
    }

    protected enum Images { 
        Timer_Bar
    }

    protected float timeFull;
    [HideInInspector]
    public float timeLeft;
    [HideInInspector]
    public float timeElapsed;

    // Mode
    public int timeMode;    // 1 : 남은 시간, 2 : 흐른 시간

    public bool isRunning = false;

    // Level
    protected const float INITIAL_LEVEL = 3f;
    public float levelAdjust = 0;
    public float time = 5.0f;

    // Joy Stick
    [SerializeField]
    protected Joystick joystick;
    [SerializeField]
    protected GameObject joystick_Stick;
    [SerializeField]
    protected GameObject joystick_Button;


    private void Update() {
        if (isRunning) {
            timeElapsed += Time.unscaledDeltaTime;
            setTimer();
        }
    }

    public override void Init() {
        Bind<Button>(typeof(Buttons));
        Bind<TextMeshProUGUI>(typeof(Texts));
        Bind<Image>(typeof(Images));

        GetButton((int)Buttons.Button_Mini_Start).GetComponent<RectTransform>().anchoredPosition = Vector3.zero;
        GetButton((int)Buttons.Button_Mini_Restart).GetComponent<RectTransform>().anchoredPosition = new Vector3(0, 1000f, 0);


        // Button Event Bind
        BindEvent(GetButton((int)Buttons.Button_Mini_Start), (PointerEventData data) => clickStartBtn(), Define.UIEvent.Click);
        BindEvent(GetButton((int)Buttons.Button_Mini_Restart), (PointerEventData data) => clickStartBtn(), Define.UIEvent.Click);
        BindEvent(GetButton((int)Buttons.Button_Back), (PointerEventData data) => ClickBackBtn(), Define.UIEvent.Click);


        GetImage((int)Images.Timer_Bar).fillAmount = 1.0f;

        


        // TODO Managers -> Joystick
        joystick = Managers.Joystick;
        joystick_Stick = joystick.gameObject;
        joystick_Button = joystick.gameObject.transform.parent.GetChild(1).gameObject;

        ClearEverything();

        // Button UI Move
        if (Managers.Scene.CurrentScene.SceneType != Define.Scene.StageSelect) {
            GetButton((int)Buttons.Button_Mini_Start).GetComponent<RectTransform>().anchoredPosition = new Vector3(0, 2000f, 0);
            GetButton((int)Buttons.Button_Back).GetComponent<RectTransform>().anchoredPosition = new Vector3(0, 2000f, 0);
        }
    }

    public virtual void GameStart() {
        Debug.Log($"{name} 게임시작");
        GetTMP((int)Texts.Text_Timer).text = "남은 시간";
        Managers.Mini.isMiniGameRunning= true;
        Time.timeScale = 0.0f;
        resetTimer();
        isRunning = true;

        ClearJoyStick();
        SetJoyStick();
    }

    protected virtual void SetJoyStick() {
        // TODO
    }

    public virtual void ClearJoyStick() {
        if (Managers.Scene.CurrentScene.SceneType == Define.Scene.StageSelect) {
            joystick_Stick.GetComponent<RectTransform>().anchoredPosition = new Vector3(-500, -2000, 0);
            joystick_Button.GetComponent<RectTransform>().anchoredPosition = new Vector3(0, -2000, 0);
        }
    }

    public void clickStartBtn() {
        GetButton((int)Buttons.Button_Mini_Start).GetComponent<RectTransform>().anchoredPosition = new Vector3(0, 1000f, 0);
        GetButton((int)Buttons.Button_Mini_Restart).GetComponent<RectTransform>().anchoredPosition = new Vector3(0, 1000f, 0);
        GameStart();
    }

    public void ClickBackBtn() {
        if (isRunning) return;
        GetButton((int)Buttons.Button_Mini_Start).GetComponent<RectTransform>().anchoredPosition = new Vector3(0, 0, 0);
        GetButton((int)Buttons.Button_Mini_Restart).GetComponent<RectTransform>().anchoredPosition = new Vector3(0, 1000f, 0);
        ClearJoyStick();
        Managers.Mini.clickBackButton();
    }

    public virtual void GameEnd() {
        Debug.Log("게임오버");
        GetButton((int)Buttons.Button_Mini_Start).GetComponent<RectTransform>().anchoredPosition = Vector3.zero;
        isRunning = false;

        Managers.Mini.clickBackButton();
        
        if (Managers.Scene.CurrentScene.SceneType != Define.Scene.StageSelect) {
            joystick_Stick.GetComponent<RectTransform>().anchoredPosition = new Vector3(-700, -200, 0);
            joystick_Button.GetComponent<RectTransform>().anchoredPosition = new Vector3(0, 0, 0);

            joystick.transform.parent.parent.GetComponent<Canvas>().sortingOrder = 2;
        }
        else {
            ClearJoyStick();
        }
    }

    public virtual void ClearEverything() {
        timeElapsed = 0;
        // timeFull = INITIAL_LEVEL + levelAdjust;
        timeFull = time;
        setTimer();
        GetTMP((int)Texts.Text_Timer).text = "남은 시간";
    }

    public virtual void setReStartBtn() {
        GetButton((int)Buttons.Button_Mini_Restart).GetComponent<RectTransform>().anchoredPosition = new Vector3(-750, -250, 0);
    }

    protected void setTimer() {
        float elapsed = Mathf.Round(timeElapsed * 10) * 0.1f;
        float left = Mathf.Round((timeFull - elapsed) * 10) * 0.1f;

        if (timeMode == 1) {    // 남은 시간 표시
            GetImage((int)Images.Timer_Bar).fillAmount = (timeFull - timeElapsed) / timeFull;
            GetTMP((int)Texts.Text_Timer_Float).text = left.ToString();

            if (left <= 0.0f) {
                GameOverByTime();
            }
        }
        else if (timeMode == 2) {  // 남은 시간 표시 (버티기면 승리)
            GetImage((int)Images.Timer_Bar).fillAmount = (timeFull - timeElapsed) / timeFull;
            GetTMP((int)Texts.Text_Timer_Float).text = left.ToString();

            if (left <= 0.0f) {
                GameClear();
            }
        }
        else {  // 추후 수정..  버티기류 경과 시간 표시
            GetImage((int)Images.Timer_Bar).fillAmount = timeElapsed;
            GetTMP((int)Texts.Text_Timer_Float).text = elapsed.ToString();
        }

        if (GetTMP((int)Texts.Text_Timer_Float).text.Length == 1) {
            GetTMP((int)Texts.Text_Timer_Float).text += ".0";
        }
    }

    public void resetTimer() {
        timeElapsed = 0;
        setTimer();
    }

    public void GameOverByTime() {
        Managers.Mini.isMiniGameRunning = false;
        isRunning = false;
        GetTMP((int)Texts.Text_Timer).text = "시간 초과";
        
        if (Managers.Scene.CurrentScene.SceneType == Define.Scene.StageSelect) {    // 연습 씬
            setReStartBtn();
        }
        else {                                                  // 실제 미니게임
            StartCoroutine(GameEndDelay(false));
            levelAdjust++;  // 틀리면 난이도 하향
        }
    }

    public void GameOver() {
        Managers.Mini.isMiniGameRunning = false;
        GetTMP((int)Texts.Text_Timer).text = "게임 오버";

        if (isRunning) {
            isRunning = false;
            if (Managers.Scene.CurrentScene.SceneType == Define.Scene.StageSelect) {    // 연습 씬
                setReStartBtn();
            }
            else {                                                  // 실제 미니게임
                StartCoroutine(GameEndDelay(false));
                levelAdjust++;  // 틀리면 난이도 하향
            }
        }
    }

    public virtual void GameClear() {
        Managers.Mini.isMiniGameRunning = false;
        GetTMP((int)Texts.Text_Timer).text = "클리어";

        if (isRunning) {
            isRunning = false;
            GetComponent<AudioSource>().Play();
            if (Managers.Scene.CurrentScene.SceneType == Define.Scene.StageSelect) {    // 연습 씬
                setReStartBtn();
            }
            else {                                                  // 실제 미니게임
                StartCoroutine(GameEndDelay(true));
            }
        }

    }


    protected IEnumerator GameEndDelay(bool isWin) {
        yield return new WaitForSecondsRealtime(2.0f);
        ClearEverything();
        GameEnd();
        Managers.Mini.ActionByMiniGame(isWin);
    }
}
