using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemCoin : ItemBase
{
    protected override void GetItem() {
        CoinManager coinmanager = GameObject.Find("MainManager").GetComponent<CoinManager>();
        coinmanager.getCoinAtField(1);

        // TODO 소리 재생 추가

        coinmanager.transform.Find("Sound").Find("Get Coin").GetComponent<AudioSource>().Play();
        Destroy(gameObject);
    }
    
}
