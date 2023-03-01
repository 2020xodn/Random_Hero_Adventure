using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OneEyeBat : MonsterBase
{
    public float damage = 2.0f;

    private void Awake() {
        base.Awake();

        base.MONSTER_DAMAGE = damage;
    }

    public override void OnKnockback(GameObject weapon) {
        //Debug.Log("³Ë¹é");
    }
}
