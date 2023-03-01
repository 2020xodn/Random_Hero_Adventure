using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiderQueenAttackRecognize : MonoBehaviour
{
    float damage = 6.0f;
    bool didAttack = false;
    void Start(){
        Destroy(gameObject, 0.6f);
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.tag == "Player" && !didAttack) {
            didAttack = true;
            collision.GetComponent<PrototypeHero>().OnDamage(damage);
            collision.GetComponent<PrototypeHero>().OnKnockback(gameObject);
            Destroy(gameObject);
        }
    }

}
