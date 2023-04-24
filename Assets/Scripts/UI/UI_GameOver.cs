using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UI_GameOver : UI_Base {

    [HideInInspector]
    public bool isWindowZero = false;

    // SpawnPoint
    public GameObject spawnPoint;

    public LavaCaveBossManager lavaManager = null;

    enum Buttons { 
        Button_ToMain,
        Button_RestartByMoney,
        Button_RestartByAdv
    }

    enum Texts { 
        Text_Gold_Amount
    }

    enum Chunks {
        Chunk_GameOver
    }

    public override void Init() {
        Bind<Button>(typeof(Buttons));
        Bind<TextMeshProUGUI>(typeof(Texts));
        Bind<RectTransform>(typeof(Chunks));

        BindEvent(GetButton((int)Buttons.Button_ToMain), (PointerEventData data) => clickToMain(), Define.UIEvent.Click);
        BindEvent(GetButton((int)Buttons.Button_RestartByMoney), (PointerEventData data) => clickReStartByMoney(), Define.UIEvent.Click);
        // BindEvent(GetButton((int)Buttons.Button_RestartByAdv), (PointerEventData data) => clickReStartByAdv(), Define.UIEvent.Click);

        GetTMP((int)Texts.Text_Gold_Amount).text = Managers.GameData.goldAmount + "";
    }

    public void MoveWindowToCenter() {
        isWindowZero = true;
        Get<RectTransform>((int)Chunks.Chunk_GameOver).anchoredPosition = Vector3.zero;

    }

    void MoveWindowSomewehre() {
        isWindowZero = false;
        Get<RectTransform>((int)Chunks.Chunk_GameOver).anchoredPosition = new Vector3(-2000, -5000, 0);
    }

    void clickToMain() {
        Managers.Scene.LoadScene(Define.Scene.StageSelect);
    }

    void clickReStartByMoney() {
        if (Managers.GameData.goldAmount >= 20) {
            Managers.GameData.goldAmount -= 20;
            Managers.Player.RespawnHero();

            MoveWindowSomewehre();
            isWindowZero = false;

            if (lavaManager != null) {
                lavaManager.backWall();
            }
        }
        else
            Managers.Scene.LoadScene(Define.Scene.StageSelect);

    }

    void clickReStartByAdv() {

        // TODO Adv
        if (false) {
            Managers.Player.RespawnHero();
            MoveWindowSomewehre();
            isWindowZero = false;
        }
    }

    
}
