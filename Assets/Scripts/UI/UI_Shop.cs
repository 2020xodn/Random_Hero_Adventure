using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_Shop : UI_Base
{
    enum Buttons {
        Button_Confirm_Buy,
        Button_Confirm_Already,
        Button_Sword_Damage_Up
    }

    enum Chunks { 
        Chunk_Coin_Amount,
        Chunk_Buy_Item_Confirm,
        Chunk_Already_Buy_Item
    }

    enum Texts { 
        Text_Coin_Amount
    }

    enum GameObjects { 
        Item_List
    }

    [SerializeField]
    int PriceItem1 = 10;    // TODO: Load From Json

    public override void Init() {
        // Bind Objects
        Bind<Button>(typeof(Buttons));
        Bind<RectTransform>(typeof(Chunks));
        Bind<TextMeshProUGUI>(typeof(Texts));
        Bind<GameObject>(typeof(GameObjects));

        // Item Button Bind Event
        BindEvent(GetButton((int)Buttons.Button_Sword_Damage_Up), (PointerEventData data) => Buy_Item_01_DamageUp(), Define.UIEvent.Click);

        // Button Bind Event
        BindEvent(GetButton((int)Buttons.Button_Confirm_Buy), (PointerEventData data) => ClickBuyItemChunk(), Define.UIEvent.Click);
        BindEvent(GetButton((int)Buttons.Button_Confirm_Already), (PointerEventData data) => ClickItemAlreadyBuyChunk(), Define.UIEvent.Click);       
    }

    public void UpdateCoin() {
        Debug.Log((int)Texts.Text_Coin_Amount);
        Debug.Log(Managers.GameData.goldAmount);
        GetTMP((int)Texts.Text_Coin_Amount).text = Managers.GameData.goldAmount + "";
    }

    public void Buy_Item_01_DamageUp() {
        if (Managers.GameData.goldAmount >= PriceItem1 && Managers.GameData.item1 == false) {
            Managers.GameData.goldAmount -= PriceItem1;
            UpdateCoin();


            AudioSource audio = GetButton((int)Buttons.Button_Sword_Damage_Up).gameObject.transform.parent.GetComponent<AudioSource>();
            if (audio != null) audio.Play();

            Managers.GameData.item1 = true;
            Managers.Data.Save(Managers.GameData);
            Get<RectTransform>((int)Chunks.Chunk_Buy_Item_Confirm).anchoredPosition = Vector3.zero;
        }
        else if (Managers.GameData.goldAmount >= PriceItem1) {
            Get<RectTransform>((int)Chunks.Chunk_Already_Buy_Item).anchoredPosition = Vector3.zero;
        }
    }

    public void ClickBuyItemChunk() {
        Get<RectTransform>((int)Chunks.Chunk_Buy_Item_Confirm).anchoredPosition = new Vector3(0, 1000, 0);
    }

    public void ClickItemAlreadyBuyChunk() {
        Get<RectTransform>((int)Chunks.Chunk_Buy_Item_Confirm).anchoredPosition = new Vector3(0, 1000, 0);
        Get<RectTransform>((int)Chunks.Chunk_Already_Buy_Item).anchoredPosition = new Vector3(0, 1000, 0);
    }
}
