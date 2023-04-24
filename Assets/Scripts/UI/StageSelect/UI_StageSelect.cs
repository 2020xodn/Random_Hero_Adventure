using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_StageSelect : UI_Base {

    enum Texts { 
        Text_Current_Stage
    }

    enum Sprites { 
        Stage_00_Forest,
        Stage_01_SnowMountain,
        Stage_02_Desert,
        Stage_03_Cave,
        Stage_04_LavaCave,
        StageLength
    }

    enum Buttons {
        Button_0_Tutorial,
        Button_1_Stage,
        Button_2_Stage,
        Button_3_Stage,
        Button_4_Stage,
        Button_Stage_Start,
    }

    SelectCharacterController charMove;

    private int SCENE_WEIGHT = 1;   // 0 -> StageSelect Scene, 1 ~ Game

    // 스테이지 변경
    Coroutine coroutineChangeImage;
    SpriteRenderer currentBackround;

    string[] stageNameString = {
        "튜토리얼",
        "눈산",
        "사막",
        "동굴",
        "보스동굴",
        "보스(임시)"
    };

    public override void Init() {
        Bind<TextMeshProUGUI>(typeof(Texts));
        Bind<SpriteRenderer>(typeof(Sprites));
        Bind<Button>(typeof(Buttons));

        charMove = GameObject.FindWithTag("Player").GetComponent<SelectCharacterController>();

        ChangeStageNameText();
        ClearAllBackground();

        // Stage Button Bind Event
        BindEvent(GetButton((int)Buttons.Button_0_Tutorial), (PointerEventData data) => MoveToEachStage(0), Define.UIEvent.Click);
        BindEvent(GetButton((int)Buttons.Button_1_Stage), (PointerEventData data) => MoveToEachStage(1), Define.UIEvent.Click);
        BindEvent(GetButton((int)Buttons.Button_2_Stage), (PointerEventData data) => MoveToEachStage(2), Define.UIEvent.Click);
        BindEvent(GetButton((int)Buttons.Button_3_Stage), (PointerEventData data) => MoveToEachStage(3), Define.UIEvent.Click);
        BindEvent(GetButton((int)Buttons.Button_4_Stage), (PointerEventData data) => MoveToEachStage(4), Define.UIEvent.Click);

        BindEvent(GetButton((int)Buttons.Button_Stage_Start), (PointerEventData data) => StartStage(), Define.UIEvent.Click);

        /*
        for (int i = 0; i < (int)Buttons.StageLength; i++)
            BindEvent(GetButton(i), (PointerEventData data) => MoveToEachStage(i), Define.UIEvent.Click);
        */
    }

    public void MoveToEachStage(int stageNumber) {
        if (stageNumber == 0 || Managers.GameData.gameProgress >= 1) { // 이전 스테이지를 클리어 해야 이동 가능

            //StopCoroutine(ChangeBackgroundTransparency());
            if (coroutineChangeImage != null)
                StopCoroutine(coroutineChangeImage);
            ClearAllBackground();

            

            if (charMove.currentSelected != stageNumber) {
                charMove.currentSelected = stageNumber;
                ChangeStageNameText();
                coroutineChangeImage = StartCoroutine(ChangeBackgroundTransparency());
            }
        }
    }

    public void StartStage() {
        int stageNumber = charMove.currentSelected + SCENE_WEIGHT;

        Debug.Log($"Scene Load : {stageNumber}");

        switch (stageNumber) {
            case 1:
                Managers.Scene.LoadScene(Define.Scene.Stage1);
                break;

            case 2:
                break;

            case 3:
                break;

            case 4:
                break;

            case 5:
                Managers.Scene.LoadScene(Define.Scene.Stage5);
                break;
        }
    }

    void ClearAllBackground() {
        Color c;

        // 이미지 하나 빼고 전부 투명 처리
        for (int idx = 0; idx < (int)Sprites.StageLength; idx++) {
            if (idx == charMove.currentSelected)
                continue;
            c = Get<SpriteRenderer>(idx).color;
            c.a = 0;
            Get<SpriteRenderer>(idx).color = c;
        }

        currentBackround = Get<SpriteRenderer>(charMove.currentSelected);
        c = currentBackround.color;
        c.a = 1;
        currentBackround.color = c;
    }

    void ChangeStageNameText() {
        GetTMP((int)Texts.Text_Current_Stage).text = stageNameString[charMove.currentSelected];
    }

    IEnumerator ChangeBackgroundTransparency() {
        SpriteRenderer newBackground = Get<SpriteRenderer>(charMove.currentSelected);
        Color newBackgroundColor = newBackground.color;
        Color oldBackgroundColor = currentBackround.color;

        yield return new WaitForSeconds(0.20f);
        for (int count = 0; count < 2; count++) {
            for (int i = 0; i < 50; i++) {
                newBackgroundColor.a += 0.01f;
                newBackground.color = newBackgroundColor;

                oldBackgroundColor.a -= 0.01f;
                currentBackround.color = oldBackgroundColor;
                yield return new WaitForSeconds(0.01f);
            }
        }

        newBackgroundColor.a = 1;
        newBackground.color = newBackgroundColor;

        oldBackgroundColor.a = 0;
        currentBackround.color = oldBackgroundColor;

        currentBackround = newBackground;
    }
}
