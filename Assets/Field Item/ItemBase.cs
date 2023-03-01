using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemBase : MonoBehaviour
{
    private void OnTriggerStay2D(Collider2D collision) {
        if (collision.tag == "Player") {
            GetItem();
        }
    }

    protected virtual void GetItem() { 
        // 자식 클래스에서 구현
    }
}
