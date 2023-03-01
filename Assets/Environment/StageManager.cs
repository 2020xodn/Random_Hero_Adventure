using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : MonoBehaviour
{
    GameManager gameManager;
    private void Start() {
        gameManager = GameObject.Find("MainManager").GetComponent<GameManager>();
    }

    public void ClickStageButton() {
        gameManager.MoveToEachStage((int)char.GetNumericValue(gameObject.name[0]));
    }
}
