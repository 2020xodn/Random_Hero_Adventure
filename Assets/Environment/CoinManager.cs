using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CoinManager : MonoBehaviour
{
    [HideInInspector]
    public GameObject uiCoinChunk;

    TextMeshProUGUI coinText;

    [HideInInspector]
    public int thisGameCoinAmount = 0;

    void Start(){
        uiCoinChunk = GameObject.Find("Earned Coin");
        
        coinText = uiCoinChunk.transform.Find("Text").GetComponent<TextMeshProUGUI>();

        /*Debug.Log(uiCoinChunk);
        Debug.Log(coinText);*/

        thisGameCoinAmount = 0;
        coinText.text = thisGameCoinAmount + "";
    }


    public void getCoinAtField(int amount) {
        thisGameCoinAmount += amount;
        coinText.text = thisGameCoinAmount + "";
    }
}
