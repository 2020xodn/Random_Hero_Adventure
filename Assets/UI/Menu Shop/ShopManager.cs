using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopManager : MonoBehaviour
{
    [HideInInspector]
    public GameObject uiCoinChunk;

    TextMeshProUGUI coinText;

    int thisGameCoinAmount = 0;

    public Button btnPlus;
    public Button btnSave;

    public Button btnAdvertisement;

    public Button btnItem_1_DamageUp;

    GameManager gameManager;
    DataManager dataManager;

    public RectTransform buyItemConfirmChunk;
    public Button btnItemConfirm;
    public RectTransform buyItemAlreadyBuyConfirmChunk;
    public Button btnItemAlreadyBuyConfirm;

    void Start() {
        uiCoinChunk = GameObject.Find("Coin Amount");

        coinText = uiCoinChunk.transform.Find("Text").GetComponent<TextMeshProUGUI>();

        dataManager = GameObject.Find("MainManager").GetComponent<DataManager>();
        
        btnPlus.onClick.AddListener(() => clickPlusCoin());
        btnSave.onClick.AddListener(() => clickSaveButton());
        btnItem_1_DamageUp.onClick.AddListener(() => buyItem_1_DamageUp());
        btnItemConfirm.onClick.AddListener(() => clickBuyItemChunk());

        btnItemAlreadyBuyConfirm.onClick.AddListener(() => clickItemAlreadyBuyChunk());


        btnAdvertisement.onClick.AddListener(() => clickItemAlreadyBuyChunk());
    }

    public void clickSaveButton() {
        dataManager.Save(GameManager.gameData);
    }

    public void updateCoin() {
        coinText.text = GameManager.gameData.goldAmount + "";
    }

    public void clickPlusCoin() {
        GameManager.gameData.goldAmount++;
        updateCoin();
    }

    public void buyItem_1_DamageUp() {
        if (GameManager.gameData.goldAmount >= 10 && GameManager.gameData.item1 == false) {
            GameManager.gameData.goldAmount -= 10;
            updateCoin();
            Debug.Log("칼 구매");

            transform.GetChild(1).GetChild(0).GetComponent<AudioSource>().Play();

            GameManager.gameData.item1 = true;
            GameObject.Find("MainManager").GetComponent<DataManager>().Save(GameManager.gameData);
            buyItemConfirmChunk.anchoredPosition = Vector3.zero;
        }
        else if (GameManager.gameData.goldAmount >= 10) {
            Debug.Log("이미 구매함");
            buyItemAlreadyBuyConfirmChunk.anchoredPosition = Vector3.zero;
        }
    }

    public void clickBuyItemChunk() {
        buyItemConfirmChunk.anchoredPosition = new Vector3(0, 1000, 0);
    }

    public void clickItemAlreadyBuyChunk() {
        buyItemConfirmChunk.anchoredPosition = new Vector3(0, 1000, 0);
        buyItemAlreadyBuyConfirmChunk.anchoredPosition = new Vector3(0, 1000, 0);
    }

    public void clickAdvertisementButton() { 

    }
}
