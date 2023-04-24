using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Portal : MonoBehaviour
{
    private void OnTriggerStay2D(Collider2D collision) {
        if (collision.gameObject.tag == "Player") {
            UI_Coin UI_Coin = GameObject.Find("Earned Coin").GetComponent<UI_Coin>();

            Debug.Log("È¹µæ ÄÚÀÎ : " + UI_Coin.thisGameCoinAmount);
            Managers.GameData.goldAmount += UI_Coin.thisGameCoinAmount;
            
            if (Managers.Scene.CurrentScene.SceneType == Define.Scene.Stage1) {
                Managers.GameData.gameProgress = 1;
            }

            Managers.Data.Save(Managers.GameData);

            Managers.Player.respawnObject = null;
            //player.respawnObject = null;

            Managers.Scene.LoadScene(Define.Scene.StageSelect);
        }
    }
}
