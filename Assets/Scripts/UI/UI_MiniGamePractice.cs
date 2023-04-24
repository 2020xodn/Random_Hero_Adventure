using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_MiniGamePractice : UI_Base
{
    enum Buttons {
        Button_MiniGame_SignalConnect,
        Button_MiniGame_AvoidMeteor
    }
    public override void Init() {
        Bind<Button>(typeof(Buttons));

        BindEvent(GetButton((int)Buttons.Button_MiniGame_SignalConnect), (PointerEventData data) => ClickSignalConnectButton(), Define.UIEvent.Click);
        BindEvent(GetButton((int)Buttons.Button_MiniGame_AvoidMeteor), (PointerEventData data) => ClickAvoidMeteorButton(), Define.UIEvent.Click);
    }

    void ClickSignalConnectButton() {
        Managers.Mini.clickMinigamePracticeSignalConnect();

    }

    void ClickAvoidMeteorButton() {
        Managers.Mini.clickMinigamePracticeAvoidMeteor();
    }
}
