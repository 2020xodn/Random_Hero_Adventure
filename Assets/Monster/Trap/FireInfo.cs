using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireInfo : MonsterBase
{
    public float damage = 2.0f;

    private void Awake() {
        base.Awake();

        base.MONSTER_DAMAGE = damage;
        base.fadeSpeed = 0.2f;
    }

    
    public void onAttack() {
        GetComponent<BoxCollider2D>().enabled = false;
        body2d.gravityScale = 0;
        body2d.velocity = Vector2.zero;
        color.a = 1.0f;
        StartCoroutine(Fade());

        GetComponent<AudioSource>().Play();
    }

    private void Update() {
        
    }
}
