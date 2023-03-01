using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHPBarController : MonoBehaviour {
    GameObject GreenBar;
    Image GreenBarImage;
    float initialXPosition;
    float initialYPosition;
    float initialXSize;
    float initialYSize;
    float debug;


    private void Awake() {
        GreenBar = transform.Find("Bar Green").gameObject;
        GreenBarImage = GreenBar.GetComponent<Image>();
        
        initialXPosition = GreenBar.transform.localPosition.x;
        initialYPosition = GreenBar.transform.localPosition.y;

        initialXSize = GreenBar.transform.localScale.x;
        initialYSize = GreenBar.transform.localScale.y;

        // debug = GameObject.FindWithTag("Player").transform.localScale.z;

    }

    // Update is called once per frame
    void Update() {
        /*debug = GameObject.FindWithTag("Player").transform.localScale.z;
        setHPBar(debug);*/
    }

    public void setHPBar(float ratio) {
        /*GreenBar.transform.localScale = new Vector3(ratio * initialXSize, initialYSize, 1);
        GreenBar.transform.localPosition = new Vector3(initialXPosition + (1 - ratio) * 0.005f, initialYPosition, -0.1f);*/
        GreenBarImage.fillAmount = ratio;
    }
}