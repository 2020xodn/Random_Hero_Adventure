using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_StageSelect_Menu : UI_Base {
    enum Buttons {
        Button_Setting,
        Button_Setting_Back,
        Button_Setting_Quit,
        Button_Setting_Reset,
        Button_Shop,
        Button_Shop_Back,
        Button_GamePractice,
        Button_GamePractice_Back,
        Button_MiniGame_SignalConnect,
        Button_MiniGame_AvoidMeteor
    }

    enum Chunks { 
        Chunk_Setting,
        Chunk_Shop,
        Chunk_GamePractice
    }

    GameObject Button_StageStart;

    public override void Init() {
        Bind<Button>(typeof(Buttons));
        Bind<RectTransform>(typeof(Chunks));

        // Menu Button Bind Event
        // Setting
        BindEvent(GetButton((int)Buttons.Button_Setting), (PointerEventData data) => ClickSettingButton(), Define.UIEvent.Click);
        BindEvent(GetButton((int)Buttons.Button_Setting_Back), (PointerEventData data) => ClickSettingBackButton(), Define.UIEvent.Click);
        BindEvent(GetButton((int)Buttons.Button_Setting_Quit), (PointerEventData data) => ClickSettingQuitButton(), Define.UIEvent.Click);
        BindEvent(GetButton((int)Buttons.Button_Setting_Reset), (PointerEventData data) => ClickSettingDeleteFileButton(), Define.UIEvent.Click);

        // Shop
        BindEvent(GetButton((int)Buttons.Button_Shop), (PointerEventData data) => ClickShopButton(), Define.UIEvent.Click);
        BindEvent(GetButton((int)Buttons.Button_Shop_Back), (PointerEventData data) => ClickShopBackButton(), Define.UIEvent.Click);

        // Game Practice
        BindEvent(GetButton((int)Buttons.Button_GamePractice), (PointerEventData data) => ClickGamePracticeButton(), Define.UIEvent.Click);
        BindEvent(GetButton((int)Buttons.Button_GamePractice_Back), (PointerEventData data) => ClickGamePracticeBackButton(), Define.UIEvent.Click);

        Button_StageStart = GameObject.Find("Button_Stage_Start");
    }

    public void ClickSettingButton() {
        Get<RectTransform>((int)Chunks.Chunk_Setting).transform.position = Vector3.zero;
    }

    public void ClickSettingQuitButton() {
        Debug.Log("Quit Game");
        Application.Quit();
    }

    public void ClickSettingDeleteFileButton() {
        Debug.Log("Clear Data");
        Managers.Data.DeleteData();

        UI_StageSelect s = Util.FindChild<UI_StageSelect>(transform.parent.gameObject, "UI_StageSelect", false);
        if (s == null) {
            s = GameObject.Find("UI_StageSelect").GetComponent<UI_StageSelect>();
        }

        if (s != null)
            s.MoveToEachStage(0);
    }

    public void ClickSettingBackButton() {
        Get<RectTransform>((int)Chunks.Chunk_Setting).transform.position = new Vector3(2500, 0, 0);
    }

    public void ClickShopButton() {
        Get<RectTransform>((int)Chunks.Chunk_Shop).GetComponent<UI_Shop>().UpdateCoin();
        Get<RectTransform>((int)Chunks.Chunk_Shop).transform.position = Vector3.zero;
    }

    public void ClickShopBackButton() {
        Get<RectTransform>((int)Chunks.Chunk_Shop).transform.position = new Vector3(2500, -1000, 0);
    }

    public void ClickGamePracticeButton() {
        Get<RectTransform>((int)Chunks.Chunk_GamePractice).transform.position = Vector3.zero;
        Button_StageStart.SetActive(false);
    }

    public void ClickGamePracticeBackButton() {
        Get<RectTransform>((int)Chunks.Chunk_GamePractice).transform.position = new Vector3(4500, 1000, 0);
        Button_StageStart.SetActive(true);
    }


}
