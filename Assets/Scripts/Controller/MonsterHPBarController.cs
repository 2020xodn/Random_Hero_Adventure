using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterHPBarController: MonoBehaviour 
{
    GameObject GreenBar;
    float initialXPosition;
    float initialYPosition;
    float initialXSize;
    float initialYSize;

    private void Start() {
        GreenBar = transform.Find("Image_Green").gameObject;
        initialXPosition = GreenBar.transform.localPosition.x;
        initialYPosition = GreenBar.transform.localPosition.y;
        
        initialXSize = GreenBar.transform.localScale.x;
        initialYSize = GreenBar.transform.localScale.y;
    }

    public void setHPBar(float ratio) {
        GreenBar.transform.localScale = new Vector3(ratio * initialXSize, initialYSize, 1);
        GreenBar.transform.localPosition = new Vector3(initialXPosition + (1- ratio) * 0.005f, initialYPosition, -0.1f);
    }
}
