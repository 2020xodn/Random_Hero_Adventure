using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHPBarController : UI_Base {
    enum Images { 
        Image_Green
    }

    public override void Init() {
        Bind<Image>(typeof(Images));
    }

    public void setHPBar(float ratio) {
        GetImage((int)Images.Image_Green).fillAmount = ratio;
    }

    
}