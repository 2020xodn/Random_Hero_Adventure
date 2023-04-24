using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_Coin : UI_Base
{
    [HideInInspector]
    public int thisGameCoinAmount = 0;

    enum Texts { 
        Text_Coin_Amount
    }


    public override void Init() {
        Bind<TextMeshProUGUI>(typeof(Texts));

        thisGameCoinAmount = 0;
        GetTMP((int)Texts.Text_Coin_Amount).text = thisGameCoinAmount + "";
    }

    public void getCoinAtField(int amount) {
        thisGameCoinAmount += amount;
        GetTMP((int)Texts.Text_Coin_Amount).text = thisGameCoinAmount + "";
    }
}
