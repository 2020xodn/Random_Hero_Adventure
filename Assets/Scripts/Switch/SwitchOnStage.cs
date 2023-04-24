using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchOnStage : MonoBehaviour
{
    public TilemapBoom[] boomObjects;

    // switchMode 1 : �̴ϰ��� �� ����
    // switchMode 2 : ���� �ٸ� �̺�Ʈ ����
    int switchMode = 1;

    private void Start() {
    }

    public void TriggerSwitch() {

        if (switchMode == 1) {
            // �̴ϰ��� ����
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
