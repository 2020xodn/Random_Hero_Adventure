using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndPortal : MonoBehaviour
{
    public int stageSelectSceneNumber = 0;

    public PrototypeHero player;

    private void OnTriggerStay2D(Collider2D collision) {
        if (collision.gameObject.tag == "Player") {
            CoinManager coinManager = GameObject.Find("MainManager").GetComponent<CoinManager>();

            Debug.Log("È¹µæ ÄÚÀÎ : " + coinManager.thisGameCoinAmount);
            GameManager.gameData.goldAmount += coinManager.thisGameCoinAmount;
            

            if (SceneManager.GetActiveScene().buildIndex == 1) {
                GameManager.gameData.gameProgress = 1;
            }

            GameObject.Find("MainManager").GetComponent<DataManager>().Save(GameManager.gameData);

            player.respawnObject = null;

            SceneManager.LoadScene(stageSelectSceneNumber);
        }

        
    }
}
