using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchOnStage : MonoBehaviour
{
    MiniGameManager minigameManager;
    public TilemapBoom[] boomObjects;

    // switchMode 1 : 미니게임 후 폭파
    // switchMode 2 : 다른 이벤트 추가
    int switchMode = 1;

    private void Start() {
        minigameManager = GameObject.Find("MainManager").GetComponent<MiniGameManager>();
    }

    public void TriggerSwitch() {
        if (switchMode == 1) {
            // 미니게임 시작
            minigameManager.inGameMode = 1;
            minigameManager.setSwitch(this);
            minigameManager.setRandomGameNumber();
            minigameManager.setPosRandomGame();


        }
    }

    public void TriggerBoom() {
        GetComponent<AudioSource>().Play();
        for (int idx = 0; idx < boomObjects.Length; idx++) {
            boomObjects[idx].Boom();
        }
    }
}
