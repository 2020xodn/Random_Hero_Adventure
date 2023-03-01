using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapBase : MonoBehaviour {
    public float TRAP_DAMAGE = 2.0f;

    PrototypeHero p;
    private void Start() {
        p = GameObject.FindWithTag("Player").GetComponent<PrototypeHero>();
    }

    private void OnTriggerStay2D(Collider2D collision) {
        /*if (collision.gameObject.tag == "Player" && !p.isInvisible) {
            Debug.Log("Æ®·¦µ¥¹ÌÁö2");
            p.OnDamage(TRAP_DAMAGE);
            p.OnKnockbackTrap(gameObject);
        }*/
    }
}
