using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MiniGameManager
{
    PrototypeHero player;
    SwitchOnStage switchStage;

    public MiniGameBase[] gameList;

    [HideInInspector]
    public bool isMiniGameRunning = false;

    int nowSelected;

    public int inGameMode; // 0 : 대기, 1 : 맵 열기, 2 : 보스 데미지

    Abomination bossStage;

    public void Init(){
        //setRandomGameNumber();
        //RandomGameStart();

        FindPlayer();


        if (Managers.Scene.CurrentScene.SceneType != Define.Scene.StageSelect) {
            GameObject go = GameObject.Find("@MiniGame");
                
            // TODO
            if (go == null) { 
                go = Managers.Resource.Instantiate("MiniGame/MiniGame");
                go.name = "@MiniGame";
                go.GetComponent<Canvas>().worldCamera = Camera.main;
                go.transform.SetParent(Managers.UI.Root.transform);
            }
        }
        

        gameList = new MiniGameBase[2];
        gameList[0] = GameObject.Find("MiniGame_SignalConnect").GetComponent<MiniGameBase>();
        gameList[1] = GameObject.Find("MiniGame_AvoidMeteor").GetComponent<MiniGameBase>();
    }

    public void FindPlayer() {
        player = GameObject.FindWithTag("Player").GetComponent<PrototypeHero>();
    }

    public void setSwitch(SwitchOnStage s) {
        switchStage = s;
    }

    public void setBoss(Abomination a) {
        bossStage = a;
    }

    public void setRandomGameNumber() {
        nowSelected = Random.Range(0, gameList.Length);
    }

    public void setPosRandomGame() {

        gameList[nowSelected].GetComponent<RectTransform>().anchoredPosition = Vector3.zero;
        gameList[nowSelected].ClearEverything();

        if (Managers.Scene.CurrentScene.SceneType == Define.Scene.StageSelect) { // 연습 씬
        }
        else {                                                  // 실제 랜덤 미니게임
            gameList[nowSelected].clickStartBtn();
        }
        
    }

    public void MiniGameCancle() {
        //Debug.Log("cancle | " + isMiniGameRunning);
        //if (!isMiniGameRunning) {
        if (!gameList[nowSelected].isRunning) {
            gameList[nowSelected].GetComponent<RectTransform>().anchoredPosition = new Vector3(4500, -5000, 0);
            gameList[nowSelected].ClearJoyStick();
        }
    }

    public void clickBackButton() {
        if (!isMiniGameRunning) {
            gameList[nowSelected].GetComponent<RectTransform>().anchoredPosition = new Vector3(4500, -5000, 0);
            gameList[nowSelected].ClearEverything();
            // TODO
            //gameList[nowSelected].btnStart.anchoredPosition = Vector3.zero;
            gameList[nowSelected].isRunning = false;
            Time.timeScale = 1.0f;
        }
    }

    public void clickMinigamePracticeSignalConnect() {
        nowSelected = 0;
        setPosRandomGame();
    }

    public void clickMinigamePracticeAvoidMeteor() {
        nowSelected = 1;
        setPosRandomGame();
    }

    public void clickReStartButton() {
        gameList[nowSelected].GameStart();
    }

    public void ActionByMiniGame(bool didWin) {
        clickBackButton();
        if (didWin) {
            if (inGameMode == 1) {  // 함정 파괴
                switchStage.TriggerBoom();
            }
            else if (inGameMode == 2) { // 

            }
            else if (inGameMode == 3) { // 보스전 데미지 !
                bossStage.ClearMiniGameDamageToBoss();
            }
        }
        else {
            if (inGameMode == 3) {
                bossStage.failMiniGameRestoreBossHP();
            }
            else {
                // 플레이어 데미지
                // 난이도 다운
                player.OnKnockbackTrap(switchStage.gameObject);
                player.setHPByMinigame();
            }
            
            
        }
    }
    
}
