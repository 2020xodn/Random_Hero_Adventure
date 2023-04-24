using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AvoidMeteorMeteorDestroyCollider : MonoBehaviour
{
    bool inArea = false;

    void Update(){
        if (Time.timeScale == 0) {
            Collider2D[] colls = Physics2D.OverlapBoxAll(transform.position, new Vector2(1, 1), 1);

            if (!inArea) {
                for (int idx = 0; idx < colls.Length; idx++) {
                    if (colls[idx].name.Contains("Meteor Destroy")) {
                        inArea = true;
                        break;
                    }
                }
            }
            else {
                bool isIn = false;
                for (int idx = 0; idx < colls.Length; idx++) {
                    if (colls[idx].name.Contains("Meteor Destroy")) {
                        isIn = true;
                        break;
                    }
                }
                if (!isIn) {
                    Destroy(transform.parent.gameObject);
                }
            }
        }
    }
}
