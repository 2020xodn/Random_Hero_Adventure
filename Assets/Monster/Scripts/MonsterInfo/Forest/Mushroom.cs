using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mushroom : MonsterBase {
    public float damage = 2.0f;
    
    private void Awake() {
        base.Awake();

        base.MONSTER_DAMAGE = damage;
    }
}
