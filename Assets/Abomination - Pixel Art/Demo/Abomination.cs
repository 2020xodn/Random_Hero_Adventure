using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Abomination : MonoBehaviour {

    [SerializeField] float      m_speed = 1.5f;
    [SerializeField] float      m_jumpForce = 3.0f;
    [SerializeField] bool       m_noBlood = false;
    

    private Animator            m_animator;
    private Rigidbody2D         m_body2d;


    // Facing Direction
    int facingDirection;
    SpriteRenderer sp;

    // Player;
    PrototypeHero player;

    // HP Bar
    public float maxHP = 20.0f;
    float currentHP;
    MonsterHPBarController hpController;
    public bool isDead = false;
    public int bossLife = 1;


    // Manager
    MiniGameManager minigameManager;
    LavaCaveBossManager lavaCaveBossManager;

    // Attack
    GameObject AttackPrefab;
    public bool canAttack = true;
    public bool isAttacking = false;
    public float attackDelay = 3.5f;

    // Move Pattern
    int nowPattern = 1; // 1 : 1층, 2 : 2층
    Vector2 []pattern1Locations;
    Vector2 []pattern2Locations;
    int nextPatrolLocationNumber = 0;

    public IEnumerator nowMoveDelay;
    bool isJumping = false;

    // After Dead
    Vector3 deadPosition;


    // Small Spider GameObject
    List<GameObject> spiders;
    public GameObject spiderPrefab;

    void Start ()
    {
        m_animator = GetComponent<Animator>();
        m_body2d = GetComponent<Rigidbody2D>();

        AttackPrefab = Resources.Load<GameObject>("AttackCollider/BossAttack/SpiderQueen/AttackNormal");

        sp = GetComponent<SpriteRenderer>();
        //Debug.Log(AttackPrefab);

        player = GameObject.FindWithTag("Player").GetComponent<PrototypeHero>();

        currentHP = maxHP;
        hpController = transform.Find("HP Bar").gameObject.GetComponent<MonsterHPBarController>();

        minigameManager = GameObject.Find("MainManager").GetComponent<MiniGameManager>();
        lavaCaveBossManager = GameObject.Find("Spider Boss Area").GetComponent<LavaCaveBossManager>();

        pattern1Locations = new Vector2[2] {new Vector2(45f, -4.095f), new Vector2(57.5f, -4.095f) };
        pattern2Locations = new Vector2[2] {new Vector2(45f, -0.09500027f), new Vector2(57.5f, -0.09500027f) };

        spiders = new List<GameObject>();

        deadPosition = Vector3.zero;
    }

    void Update (){
        if (!isDead) {
            if (!isAttacking) {
                MovePattern();
                m_animator.SetInteger("AnimState", 1);
            }
            else if (isAttacking)
                m_animator.SetInteger("AnimState", 0);
        }

        else if (deadPosition != Vector3.zero) {
            transform.position = Vector3.MoveTowards(transform.position, deadPosition, 0.5f * Time.deltaTime);

            if (transform.position.y == deadPosition.y) {
                this.enabled = false;
            }
        }

        //Debug.Log(deadPosition);

        
        // Facing Direction
        /*if (inputX > 0) {
            facingDirection = 1;
            sp.flipX = false;
        }
        else if (inputX < 0) {
            facingDirection = -1;
            sp.flipX = true;
        }*/


        // Move
        // m_body2d.velocity = new Vector2(inputX * m_speed, m_body2d.velocity.y);


        /*if (Input.GetKeyDown("e")) {
            m_animator.SetTrigger("Jump");
        }*/

        // -- Handle Animations --
        //Death
        /*if (Input.GetKeyDown("e")) {
            Death();
        }

        //Hurt
        else if (Input.GetKeyDown("q"))
            m_animator.SetTrigger("Hurt");

        //Attack
        else if (Input.GetMouseButtonDown(0))
            AttackForward();*/

        /*//Walk
        if (Mathf.Abs(inputX) > Mathf.Epsilon)
            m_animator.SetInteger("AnimState", 1);

        //Idle
        else
            m_animator.SetInteger("AnimState", 0);*/
    }

    public void AttackForward() {
        canAttack = false;
        isAttacking = true;

        if (player.transform.position.x < transform.position.x) {
            facingDirection = -1;
            sp.flipX = true;
        }
        else if (player.transform.position.x > transform.position.x) {
            facingDirection = 1;
            sp.flipX = false;
        }

        m_animator.SetTrigger("Attack");
        StartCoroutine(createAttackColliderDelay());
        StartCoroutine(setAttackDelay());
        
    }

    IEnumerator createAttackColliderDelay() {
        yield return new WaitForSeconds(0.6f);

        GameObject attackCollider = Instantiate(AttackPrefab);
        attackCollider.transform.parent = transform;
        attackCollider.transform.position = transform.position + new Vector3(0.7f * facingDirection, 2, 0);

        yield return new WaitForSeconds(0.6f);
        isAttacking = false;
    }

    IEnumerator setAttackDelay() {
        yield return new WaitForSeconds(attackDelay);
        canAttack = true;
    }

    void Death() {
        isDead = true;
        m_animator.SetBool("noBlood", m_noBlood);
        m_animator.SetTrigger("Death");
        GetComponent<BoxCollider2D>().enabled = false;
        GetComponent<Rigidbody2D>().gravityScale = 0;
        hpController.setHPBar(0);
        
        Debug.Log(gameObject.name + " : 사망");
        transform.GetChild(0).GetComponent<SpiderQueenAttackRange>().enabled = false;
        transform.Find("Sound").Find("Dead").GetComponent<AudioSource>().Play();
        //this.enabled = false;
        m_body2d.bodyType = RigidbodyType2D.Static;

        lavaCaveBossManager.clearStageSetPortal();

        // 위치 이동

        /*pattern1Locations = new Vector2[2] { new Vector2(45f, -4.095f), new Vector2(57.5f, -4.095f) };
        pattern2Locations = new Vector2[2] { new Vector2(45f, -0.09500027f), new Vector2(57.5f, -0.09500027f) };*/

        Vector3 nextPos = Vector3.zero;
        nextPos.x = transform.position.x;
        nextPos.y = pattern1Locations[nextPatrolLocationNumber].y;


        if (transform.position.y > nextPos.y + 0.3f) {
            deadPosition = nextPos;
        }
        
    }

    

    public virtual void onAttack(GameObject weapon, float damage) {
        currentHP -= damage;

        if (!isDead) {
            if (currentHP <= 0.0f) {
                // 미니게임 시작
                BossMiniGameStart();
            }
            else {
                hpController.setHPBar(currentHP / maxHP);
            }
        }
        
    }

    void BossMiniGameStart() {
        minigameManager.inGameMode = 3;
        minigameManager.setBoss(this);
        minigameManager.setRandomGameNumber();
        minigameManager.setPosRandomGame();
    }

    public void ClearMiniGameDamageToBoss() {
        m_animator.SetTrigger("Hurt");
        bossLife--;

        if (bossLife == 0) {

            Death();
        }
        else {
            currentHP = maxHP / 3 * 2;
            hpController.setHPBar(currentHP / maxHP);
        }
    }

    public void failMiniGameRestoreBossHP() {
        currentHP = maxHP / 3 * 2;
        hpController.setHPBar(currentHP / maxHP);
    }

    void MovePattern() {
        if (nowMoveDelay == null) {
            nowMoveDelay = changeStairDelay();
            StartCoroutine(nowMoveDelay);
        }

        Vector3 nextPos = Vector3.zero;
        nextPos.x = pattern1Locations[nextPatrolLocationNumber].x;
        nextPos.y = transform.position.y;

        if (nextPos.x < transform.position.x) {
            sp.flipX = true;
        }
        else {
            sp.flipX = false;
        }

        transform.position = Vector3.MoveTowards(transform.position, nextPos, m_speed * Time.deltaTime);

        if (transform.position.x == nextPos.x) {
            nextPatrolLocationNumber++;
            if (nextPatrolLocationNumber == 2) {
                nextPatrolLocationNumber = 0;
            }
        }
    }

    IEnumerator changeStairDelay() {
        yield return new WaitForSeconds(2.0f);

        while (isAttacking) {
            yield return new WaitForSeconds(3.0f);
        }

        if (isDead) {
            Debug.Log("사망했으니 종료");
            yield break;
        }
            


        isAttacking = true;
        canAttack = false;

        
        if (nowPattern == 1) {  // 1층에서 2층으로
            //Debug.Log("점프");
            nowPattern = 2;
            m_animator.SetTrigger("Jump");
            yield return new WaitForSeconds(0.5f);
            
            m_body2d.AddForce(new Vector2(0, m_jumpForce * 40 * Time.deltaTime));
            
        }

        else if (nowPattern == 2) { // 2층에서 1층으로
            //Debug.Log("아래층으로");
            nowPattern = 1;
            StartCoroutine(MoveFadeOut());

        }

        yield return new WaitForSeconds(2.0f);
        isAttacking = false;
        canAttack = true;

        if (spiders.Count < 3) {
            GameObject spider = Instantiate(spiderPrefab);
            spiders.Add(spider);
            spider.transform.parent = transform.parent;
            spider.transform.position = transform.position + new Vector3(0, 1, 0);
            //Debug.Log(spiders);
        }

        if (!isDead) {
            StartCoroutine(changeStairDelay());
        }
        
    }

    IEnumerator MoveFadeOut() {
        SpriteRenderer sp = GetComponentInChildren<SpriteRenderer>();
        Color c = sp.color;

        yield return new WaitForSeconds(0.20f);
        for (int i = 0; i < 50; i++) {
            c.a -= 0.01f;
            sp.color = c;
            yield return new WaitForSeconds(0.01f);
        }

        transform.position = new Vector3(transform.position.x, -4.0f, transform.position.z);

        for (int i = 0; i < 50; i++) {
            c.a += 0.01f;
            sp.color = c;
            yield return new WaitForSeconds(0.01f);
        }
    }
}
