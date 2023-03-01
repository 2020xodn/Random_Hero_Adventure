using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemHeart : ItemBase
{
    protected override void GetItem() {
        PrototypeHero player = GameObject.FindWithTag("Player").GetComponent<PrototypeHero>();
        player.plusHP(4.0f);
        Destroy(gameObject);
    }
}
