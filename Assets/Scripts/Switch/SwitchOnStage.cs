using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchOnStage : MonoBehaviour
{
    public TilemapBoom[] boomObjects;

    // switchMode 1 : 미니게임 후 폭파
    // switchMode 2 : 추후 다른 이벤트 연결
    int switchMode = 1;

    private void Start() {
    }

    public void TriggerSwitch() {

        if (switchMode == 1) {
            // 미니게임 시작
            Managers.Mini.inGameMode = 1;
            Managers.Mini.setSwitch(this);
            Managers.Mini.setRandomGameNumber();
            Managers.Mini.setPosRandomGame();
        }
    }

    public void TriggerBoom() {
        GetComponent<AudioSource>().Play();
        for (int idx = 0; idx < boomObjects.Length; idx++) {
            boomObjects[idx].Boom();
        }
    }
}
