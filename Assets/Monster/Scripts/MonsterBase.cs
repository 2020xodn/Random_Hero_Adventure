using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterBase : MonoBehaviour {
    // Damage
    [HideInInspector]
    public float MONSTER_DAMAGE = 1.0f;

    // HP Bar
    public float maxHP = 10.0f;
    protected float currentHP;
    MonsterHPBarController hpController;

    // Knockback
    protected Rigidbody2D body2d;
    bool isInvisible = false;
    bool isKnockback = false;

    // Death
    protected SpriteRenderer sp;
    protected Color color;
    protected float fadeSpeed = 0.05f;
    bool isDead = false;

    // Patrol
    public float patrolSpeed = 0.01f;
    public List<Vector2> patrolLocations;
    public int patrolMode = 1; // 1 : Horizontal, 2 : Vertical, 3 : Mixed, 4 : Nothing

    // Player Trace
    public bool isTracing = false;
    public GameObject traceTarget;

    // Sprite


    int nextPatrolLocationNumber = 0;

    protected void Awake() {
        currentHP = maxHP;
        if (!name.Contains("Fire")) {
            hpController = transform.Find("HP Bar").gameObject.GetComponent<MonsterHPBarController>();
            
        }
        sp = GetComponent<SpriteRenderer>();
        color = sp.color;

        body2d = GetComponent<Rigidbody2D>();

        gameObject.tag = "Monster";
    }

    private void Update() {
        if (!isDead && !isKnockback && patrolMode != 4) {
            PatrolMonster();
        }
    }

    void PatrolMonster() {
        Vector3 next = Vector3.zero;
        // patrolmode 1 -> Horizontal Move
        // patrolmode 2 -> Vertical Move
        // patrolmode 3 -> Mixed Move
        switch (patrolMode) {
            case 1:
                next.x = patrolLocations[nextPatrolLocationNumber].x;
                next.y = transform.position.y;
                break;

            case 2:
                next.x = transform.position.x;
                next.y = patrolLocations[nextPatrolLocationNumber].y;
                break;
            case 3:
                next.x = patrolLocations[nextPatrolLocationNumber].x;
                next.y = patrolLocations[nextPatrolLocationNumber].y;
                break;
        }

        if (next.x < transform.position.x) {
            sp.flipX = false;
        }
        else {
            sp.flipX = true;
        }

        transform.position = Vector3.MoveTowards(transform.position, next, patrolSpeed * Time.deltaTime);

        if (transform.position.x == next.x && transform.position.y == next.y) {
            nextPatrolLocationNumber++;
            if (nextPatrolLocationNumber == patrolLocations.Count) {
                nextPatrolLocationNumber = 0;
            }
        }
    }

    public virtual void MoveToPlayer() { 

    }


    public virtual void onAttack(GameObject weapon, float damage) {
        currentHP -= damage;

        if (currentHP <= 0.0f) {
            isDead = true;
            // 사망 애니메이션 추가

            GetComponent<BoxCollider2D>().enabled = false;
            GetComponent<Rigidbody2D>().gravityScale = 0;
            color.a = 1.0f;
            hpController.setHPBar(0);
            StartCoroutine(Fade());
            GetComponent<AudioSource>().Play();
        }
        else {
            OnKnockback(weapon);
            isInvisible = true;
            StartCoroutine(Invisible());
            hpController.setHPBar(currentHP / maxHP);
        }
    }

    protected IEnumerator Fade() {
        yield return new WaitForSeconds(0.05f);
        color.a -= fadeSpeed;
        sp.color = color;

        if (color.a >= 0) {
            StartCoroutine(Fade());
        }
        else {
            // gameObject.SetActive(false);
            Destroy(gameObject);
        }
    }

    public virtual void OnKnockback(GameObject weapon) {
        isKnockback = true;
        StartCoroutine(KnowkbackDelay());

        Vector2 reactVec = transform.position - weapon.transform.position;
        reactVec = reactVec.normalized;

        if (weapon.transform.name.Contains("Slam")) {
            reactVec.x *= 5.0f;
        }
        reactVec.y = 0.5f;

        // Debug.Log(reactVec);
        
        //m_body2d.velocity = Vector2.zero;
        float mass = body2d.mass;

        body2d.velocity = Vector2.zero;

        if (patrolLocations.Count == 0) {
            body2d.AddForce(reactVec * mass * 1.5f, ForceMode2D.Impulse);
        }
        else {
            body2d.AddForce(reactVec * mass * 1.5f, ForceMode2D.Impulse);
        }

        //m_body2d.velocity = reactVec * 5.0f;
    }

    IEnumerator Invisible() {
        yield return new WaitForSeconds(0.5f);

        isInvisible = false;
    }

    IEnumerator InvisibleBlink() {
        Color c = sp.color;

        for (int count = 0; count < 1; count++) {
            for (int i = 0; i < 50; i++) {
                c.a -= 0.01f;
                sp.color = c;
                yield return new WaitForSeconds(0.0001f);
            }

            for (int i = 0; i < 50; i++) {
                c.a += 0.01f;
                sp.color = c;
                yield return new WaitForSeconds(0.0001f);
            }
        }

        isInvisible = false;
    }

    IEnumerator KnowkbackDelay() {
        yield return new WaitForSeconds(0.3f);
        isKnockback = false;
    }

    
}
