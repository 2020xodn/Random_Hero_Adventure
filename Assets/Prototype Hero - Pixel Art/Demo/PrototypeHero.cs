using UnityEngine;
using System.Collections;

public class PrototypeHero : MonoBehaviour {

    public float      m_runSpeed = 4.5f;
    public float      m_walkSpeed = 2.0f;
    public float      m_jumpForce = 7.5f;
    public float      m_dodgeForce = 8.0f;
    public float      m_parryKnockbackForce = 4.0f; 
    public bool       m_noBlood = false;
    public bool       m_hideSword = false;

    private Animator            m_animator;
    private Rigidbody2D         m_body2d;
    private SpriteRenderer      m_SR;
    private Sensor_Prototype    m_groundSensor;
    private Sensor_Prototype    m_wallSensorR1;
    private Sensor_Prototype    m_wallSensorR2;
    private Sensor_Prototype    m_wallSensorL1;
    private Sensor_Prototype    m_wallSensorL2;
    private bool                m_grounded = false;
    private bool                m_moving = false;
    private bool                m_dead = false;
    private bool                m_dodging = false;
    private bool                m_wallSlide = false;
    private bool                m_ledgeGrab = false;
    private bool                m_ledgeClimb = false;
    private bool                m_crouching = false;
    private Vector3             m_climbPosition;
    private int                 m_facingDirection = 1;
    private float               m_disableMovementTimer = 0.0f;
    private float               m_respawnTimer = 0.0f;
    private Vector3             m_respawnPosition = Vector3.zero;
    private int                 m_currentAttack = 0;
    private float               m_timeSinceAttack = 0.0f;
    private float               m_gravity;
    public float                m_maxSpeed = 4.5f;


    // Attack Collider
    GameObject AttackNormalPrefab;
    GameObject AttackUPPrefab;
    GameObject AttackAirNormalPrefab;
    GameObject AttackAirSlamPrefab;

    public bool isAirSlamAttacking = false;

    // Attack Speed
    float attackSpeed = 1.0f;

    // Attack Damage
    public float damageWeight = 0.0f;

    // Knockback
    [HideInInspector]
    public bool isInvisible = false;
    bool isKnockback = false;

    // HP Bar
    
    float MAX_HP = 10.0f;
    float maxHPPlus = 0.0f;
    float currentHP;
    PlayerHPBarController hpController;
    bool isDead = false;

    // Manager
    GameOverManager gameoverManager;
    bool gameover = false;

    // JoyStick
    public Joystick joystick;

    public static bool hasItem1;

    public GameObject respawnObject;
    
    void Start (){
        m_animator = GetComponentInChildren<Animator>();
        m_body2d = GetComponent<Rigidbody2D>();
        m_SR = GetComponentInChildren<SpriteRenderer>();
        m_gravity = m_body2d.gravityScale;

        m_groundSensor = transform.Find("GroundSensor").GetComponent<Sensor_Prototype>();
        m_wallSensorR1 = transform.Find("WallSensor_R1").GetComponent<Sensor_Prototype>();
        m_wallSensorR2 = transform.Find("WallSensor_R2").GetComponent<Sensor_Prototype>();
        m_wallSensorL1 = transform.Find("WallSensor_L1").GetComponent<Sensor_Prototype>();
        m_wallSensorL2 = transform.Find("WallSensor_L2").GetComponent<Sensor_Prototype>();

        // Attack Collider
        AttackNormalPrefab = Resources.Load<GameObject>("AttackCollider/AttackNormal");
        AttackUPPrefab = Resources.Load<GameObject>("AttackCollider/AttackUp");
        AttackAirNormalPrefab = Resources.Load<GameObject>("AttackCollider/AttackAirNormal");
        AttackAirSlamPrefab = Resources.Load<GameObject>("AttackCollider/AttackAirSlam");

        // HP
        maxHPPlus = 0.0f;
        MAX_HP += maxHPPlus;
        currentHP = MAX_HP;
        hpController = GameObject.Find("UI HP Bar").gameObject.GetComponent<PlayerHPBarController>();
        isInvisible = false;

        gameoverManager = GameObject.Find("MainManager").GetComponent<GameOverManager>();

        joystick = GameObject.FindWithTag("JoyStick").GetComponent<Joystick>();

        damageWeight = 0.0f;

        Debug.Log(GameManager.gameData.item1);
        if (GameManager.gameData.item1) {
            GameManager.gameData.item1 = false;
            damageWeight = 2.0f;
            Debug.Log("아이템 적용 데미지 상승");
        }
    }

    [System.Obsolete]
    void Update (){
        m_respawnTimer -= Time.deltaTime;

        m_timeSinceAttack += Time.deltaTime;

        m_disableMovementTimer -= Time.deltaTime;

        if (m_dead && !gameoverManager.isWindowZero && !gameover) {
            gameover = true;
            StartCoroutine(DelayShowGameOverWindow());
        }

        if (gameover) {
            isKnockback = true;
            isInvisible = true;
            return;
        }
            

        //Check if character just landed on the ground
        if (!m_grounded && m_groundSensor.State())
        {
            m_grounded = true;
            m_animator.SetBool("Grounded", m_grounded);
        }

        //Check if character just started falling
        if (m_grounded && !m_groundSensor.State())
        {
            m_grounded = false;
            m_animator.SetBool("Grounded", m_grounded);
        }

        float inputX = 0.0f;

        if (m_disableMovementTimer < 0.0f)
            if (joystick.Direction.x == 0)
                inputX = Input.GetAxis("Horizontal");
            else
                inputX = joystick.Direction.x;

        float inputRawX = joystick.Direction.x;
        float inputRawY = joystick.Direction.y;

        

        if (joystick.Direction.x == 0)
            inputRawX = Input.GetAxis("Horizontal");
        else
            inputRawX = joystick.Direction.x;

        inputRawX = joystick.Direction.x;

        // Check if character is currently moving
        if (Mathf.Abs(inputRawX) > Mathf.Epsilon && Mathf.Sign(inputRawX) == m_facingDirection)
            m_moving = true;
        else
            m_moving = false;

        // Swap direction of sprite depending on move direction
        if (inputRawX > 0 && !m_dodging && !m_wallSlide && !m_ledgeGrab && !m_ledgeClimb)
        {
            m_SR.flipX = false;
            m_facingDirection = 1;
        }
            
        else if (inputRawX < 0 && !m_dodging && !m_wallSlide && !m_ledgeGrab && !m_ledgeClimb)
        {
            m_SR.flipX = true;
            m_facingDirection = -1;
        }
     
        // SlowDownSpeed helps decelerate the characters when stopping
        float SlowDownSpeed = m_moving ? 1.0f : 0.5f;
        // Set movement
        if(!m_dodging && !m_ledgeGrab && !m_ledgeClimb && !m_crouching && !isKnockback)
            m_body2d.velocity = new Vector2(inputX * m_maxSpeed * SlowDownSpeed, m_body2d.velocity.y);

        // Set AirSpeed in animator
        m_animator.SetFloat("AirSpeedY", m_body2d.velocity.y);

        // Set Animation layer for hiding sword
        int boolInt = m_hideSword ? 1 : 0;
        m_animator.SetLayerWeight(1, boolInt);

        // Check if all sensors are setup properly
        if (m_wallSensorR1 && m_wallSensorR2 && m_wallSensorL1 && m_wallSensorL2)
        {
            bool prevWallSlide = m_wallSlide;
            //Wall Slide
            m_wallSlide = (m_wallSensorR1.State() && m_wallSensorR2.State() && m_facingDirection == 1) || (m_wallSensorL1.State() && m_wallSensorL2.State() && m_facingDirection == -1);
            if (m_grounded)
                m_wallSlide = false;
            m_animator.SetBool("WallSlide", m_wallSlide);
            //Play wall slide sound
            if(prevWallSlide && !m_wallSlide)
                AudioManager_PrototypeHero.instance.StopSound("WallSlide");


            //Grab Ledge
            bool shouldGrab = !m_ledgeClimb && !m_ledgeGrab && ((m_wallSensorR1.State() && !m_wallSensorR2.State()) || (m_wallSensorL1.State() && !m_wallSensorL2.State()));
            //Debug.Log(shouldGrab);
            if(shouldGrab){
                Vector3 rayStart;
                if (m_facingDirection == 1)
                    rayStart = m_wallSensorR2.transform.position + new Vector3(0.2f, 0.0f, 0.0f);
                else
                    rayStart = m_wallSensorL2.transform.position - new Vector3(0.2f, 0.0f, 0.0f);

                var hit = Physics2D.Raycast(rayStart, Vector2.down, 1.0f);
                var hit2 = Physics2D.RaycastAll(rayStart, Vector2.down);
                //Debug.Log(hit2);

                for (int idx = 0; idx < hit2.Length; idx++) {
                    //Debug.Log(hit2[idx].transform.name);
                    if (hit2[idx].transform.name.Contains("Ledge")) {
                        hit = hit2[idx];
                        break;
                    }
                }

                GrabableLedge ledge = null;
                if(hit)
                    ledge = hit.transform.GetComponent<GrabableLedge>();

                //Debug.Log(hit);
                //Debug.Log(ledge);
                //Debug.Log(rayStart);
                if (ledge){
                    m_ledgeGrab = true;
                    m_body2d.velocity = Vector2.zero;
                    m_body2d.gravityScale = 0;
                    
                    m_climbPosition = ledge.transform.position + new Vector3(ledge.topClimbPosition.x, ledge.topClimbPosition.y, 0);
                    if (m_facingDirection == 1)
                        transform.position = ledge.transform.position + new Vector3(ledge.leftGrabPosition.x, ledge.leftGrabPosition.y, 0);
                    else
                        transform.position = ledge.transform.position + new Vector3(ledge.rightGrabPosition.x, ledge.rightGrabPosition.y, 0);
                }
                m_animator.SetBool("LedgeGrab", m_ledgeGrab);
            }
            
        }

        // Ledge Climb
        if(m_ledgeGrab && inputRawY > 0.6f){
            DisableWallSensors();
            m_ledgeClimb = true;
            m_body2d.gravityScale = 0;
            m_disableMovementTimer = 6.0f/14.0f;
            m_animator.SetTrigger("LedgeClimb");
        }

        // Ledge Drop
        else if (m_ledgeGrab && inputRawY < -0.6f){
            DisableWallSensors();
        }


        else if (inputRawY < -0.6f && m_grounded && !m_dodging && !m_ledgeGrab && !m_ledgeClimb){
            if (!gameObject.transform.Find("AttackAirSlam")) {
                m_crouching = true;
                m_animator.SetBool("Crouching", true);
                m_body2d.velocity = new Vector2(m_body2d.velocity.x / 2.0f, m_body2d.velocity.y);
            }
            
        }
        else if (inputRawY >= -0.6f && m_crouching){
            m_crouching = false;
            m_animator.SetBool("Crouching", false);
        }

        //Run
        else if(m_moving){
            m_animator.SetInteger("AnimState", 1);
            m_maxSpeed = m_runSpeed;
        }

        //Idle
        else
            m_animator.SetInteger("AnimState", 0);

    }

    public void SpawnDustEffect(GameObject dust, float dustXOffset = 0, float dustYOffset = 0){
        if (dust != null){
            // Set dust spawn position
            Vector3 dustSpawnPosition = transform.position + new Vector3(dustXOffset * m_facingDirection, dustYOffset, 0.0f);
            GameObject newDust = Instantiate(dust, dustSpawnPosition, Quaternion.identity) as GameObject;
            // Turn dust in correct X direction
            newDust.transform.localScale = newDust.transform.localScale.x * new Vector3(m_facingDirection, 1, 1);
        }
    }

    void DisableWallSensors(){
        m_ledgeGrab = false;
        m_wallSlide = false;
        m_ledgeClimb = false;
        m_wallSensorR1.Disable(0.8f);
        m_wallSensorR2.Disable(0.8f);
        m_wallSensorL1.Disable(0.8f);
        m_wallSensorL2.Disable(0.8f);
        m_body2d.gravityScale = m_gravity;
        m_animator.SetBool("WallSlide", m_wallSlide);
        m_animator.SetBool("LedgeGrab", m_ledgeGrab);
    }

    // Called in AE_resetDodge in PrototypeHeroAnimEvents
    public void ResetDodging(){
        m_dodging = false;
    }

    public void SetPositionToClimbPosition(){
        transform.position = m_climbPosition;
        m_body2d.gravityScale = m_gravity;
        m_wallSensorR1.Disable(3.0f / 14.0f);
        m_wallSensorR2.Disable(3.0f / 14.0f);
        m_wallSensorL1.Disable(3.0f / 14.0f);
        m_wallSensorL2.Disable(3.0f / 14.0f);
        m_ledgeGrab = false;
        m_ledgeClimb = false;
    }

    public bool IsWallSliding(){
        return m_wallSlide;
    }

    public void DisableMovement(float time = 0.0f){
        m_disableMovementTimer = time;
    }

    public void RespawnHero(){
        transform.position = respawnObject.transform.position;
        m_dead = false;
        m_animator.Rebind();
        isInvisible = false;
        isKnockback = false;
        gameover = false;

        currentHP = MAX_HP;
        hpController.setHPBar(currentHP / MAX_HP);

    }
    
    private void OnCollisionStay2D(Collision2D collision) {
        if ((collision.gameObject.tag == "Monster" || collision.gameObject.tag == "Trap") && !isInvisible) {
            if (collision.gameObject.tag == "Monster") {
                OnDamage(collision.gameObject.GetComponent<MonsterBase>().MONSTER_DAMAGE);
            }
            else if (collision.gameObject.tag == "Trap") {
                OnDamage(collision.gameObject.GetComponent<TrapBase>().TRAP_DAMAGE);
            }

            OnKnockback(collision.gameObject);
        }
    }

    private void OnTriggerStay2D(Collider2D collision) {
        if (!isInvisible) {
            if (collision.gameObject.tag == "Trap") {
                OnDamage(collision.gameObject.GetComponent<TrapBase>().TRAP_DAMAGE);
                OnKnockbackTrap(collision.gameObject);
                transform.Find("Sound").Find("Trap Spikes").GetComponent<AudioSource>().Play();
            }
            
            // Fire
            else if (collision.gameObject.tag == "Monster" && collision.gameObject.name.Contains("Fire")) {
                OnDamage(collision.gameObject.GetComponent<MonsterBase>().MONSTER_DAMAGE);
                OnKnockbackTrap(collision.gameObject);
                Destroy(collision.gameObject);

                transform.Find("Sound").Find("Fire Hit").GetComponent<AudioSource>().Play();
            }
        }
    }

    public void OnKnockback(GameObject collision) { 
        isKnockback = true;

        // Knockback
        Vector2 reactVec = transform.position - collision.transform.position;
        reactVec = reactVec.normalized;
        reactVec.y = 0.5f;

        m_body2d.AddForce(reactVec * 7, ForceMode2D.Impulse);
    }

    public void OnKnockbackTrap(GameObject collision) {
        isKnockback = true;

        // Knockback
        Vector2 reactVec = transform.position - collision.transform.position;
        if (reactVec.x > 0) {
            reactVec.x = 1.0f;
        }
        else {
            reactVec.x = -1.0f;
        }
        reactVec.y = 1.0f;

        m_body2d.velocity = Vector2.zero;
        m_body2d.AddForce(reactVec * 5, ForceMode2D.Impulse);
    }

    public void OnDamage(float damage) {
        isInvisible = true;
        StartCoroutine(InvisibleBlink());
        currentHP -= damage;


        if (currentHP <= 0.0f) {
            isDead = true;

            m_animator.SetBool("noBlood", m_noBlood);
            m_animator.SetTrigger("Death");
            m_respawnTimer = 2.5f;
            DisableWallSensors();
            m_dead = true;

            hpController.setHPBar(0);

        }
        else {
            isInvisible = true;
            hpController.setHPBar(currentHP / MAX_HP);
        }
    }

    public void setHPByMinigame() {
        isInvisible = true;
        StartCoroutine(InvisibleBlink());
        currentHP *= 0.8f;
        hpController.setHPBar(currentHP / MAX_HP);
    }

    IEnumerator DelayShowGameOverWindow() {
        yield return new WaitForSeconds(2.0f);
        gameoverManager.MoveWindowToCenter();
    }
    


    IEnumerator InvisibleBlink() {
        SpriteRenderer sp = GetComponentInChildren<SpriteRenderer>();
        Color c = sp.color;

        yield return new WaitForSeconds(0.20f);
        isKnockback = false;
        for (int count = 0; count < 2; count++) {
            for (int i = 0; i < 50; i++) {
                c.a -= 0.01f;
                sp.color = c;
                yield return new WaitForSeconds(0.01f);
            }
            

            for (int i = 0; i < 50; i++) {
                c.a += 0.01f;
                sp.color = c;
                yield return new WaitForSeconds(0.01f);
            }
        }

        isInvisible = false;
    }

    public void invisibleDelaySlam() {
        StartCoroutine(InvisibleForSlam());
    }

    IEnumerator InvisibleForSlam() {
        isInvisible = true;
        yield return new WaitForSeconds(0.3f);
        isInvisible = false;
    }

    public void plusHP(float amount) {
        currentHP += amount;
        if (currentHP > MAX_HP)
            currentHP = MAX_HP;
        hpController.setHPBar(currentHP / MAX_HP);
    }

    public void Attack() {
        float vertical = joystick.Direction.y;

        //Up Attack
        if (vertical >= 0.5f && !m_dodging && !m_ledgeGrab && !m_ledgeClimb && !m_crouching && m_grounded && m_timeSinceAttack > 0.2f) {
            m_animator.SetTrigger("UpAttack");

            // Reset timer
            m_timeSinceAttack = 0.0f;

            // Disable movement 
            m_disableMovementTimer = 0.35f;

            // Attack Collider
            GameObject attackCollider = Instantiate(AttackUPPrefab);
            attackCollider.transform.parent = transform;
            attackCollider.transform.position = transform.position + new Vector3(0, 0.5f, 0);

            Destroy(attackCollider, 0.37f);
        }

        //Attack
        else if (!m_dodging && !m_ledgeGrab && !m_ledgeClimb && !m_crouching && m_grounded && m_timeSinceAttack > 0.2f) {
            // Reset timer
            m_timeSinceAttack = 0.0f;

            m_currentAttack++;

            // Loop back to one after second attack
            if (m_currentAttack > 2)
                m_currentAttack = 1;

            // Reset Attack combo if time since last attack is too large
            if (m_timeSinceAttack > 1.0f)
                m_currentAttack = 1;

            // Call one of the two attack animations "Attack1" or "Attack2"
            m_animator.SetTrigger("Attack" + m_currentAttack);

            // Disable movement 
            m_disableMovementTimer = 0.35f;

            // Attack Collider
            GameObject attackCollider = Instantiate(AttackNormalPrefab);
            attackCollider.transform.parent = transform;
            attackCollider.transform.position = transform.position + new Vector3(0.7f * m_facingDirection, 0, 0);

            Destroy(attackCollider, 0.37f);

        }

        //Air Slam Attack
        else if (vertical <= -0.5f && !m_ledgeGrab && !m_ledgeClimb && !m_grounded) {
            m_animator.SetTrigger("AttackAirSlam");
            m_body2d.velocity = new Vector2(0.0f, -m_jumpForce);
            m_disableMovementTimer = 0.8f;

            // Reset timer
            m_timeSinceAttack = 0.0f;

            // Attack Collider
            if (!gameObject.transform.Find("AttackAirSlam")) {
                GameObject attackCollider = Instantiate(AttackAirSlamPrefab);
                attackCollider.transform.parent = transform;
                attackCollider.transform.position = transform.position + new Vector3(0, -0.5f, 0);
                //isAirSlamAttacking = true;
            }

        }

        // Air Attack Up
        else if (vertical >= 0.5f && !m_dodging && !m_ledgeGrab && !m_ledgeClimb && !m_crouching && !m_grounded && m_timeSinceAttack > 0.2f) {
            // Debug.Log("Air attack up");
            m_animator.SetTrigger("AirAttackUp");

            // Reset timer
            m_timeSinceAttack = 0.0f;

            // Attack Collider
            GameObject attackCollider = Instantiate(AttackUPPrefab);
            attackCollider.transform.parent = transform;
            attackCollider.transform.position = transform.position + new Vector3(0, 0.5f, 0);

            Destroy(attackCollider, 0.37f);
        }

        // Air Attack
        else if (!m_dodging && !m_ledgeGrab && !m_ledgeClimb && !m_crouching && !m_grounded && m_timeSinceAttack > 0.2f) {
            m_animator.SetTrigger("AirAttack");

            // Reset timer
            m_timeSinceAttack = 0.0f;

            // Attack Collider
            GameObject attackCollider = Instantiate(AttackAirNormalPrefab);
            attackCollider.transform.parent = transform;
            attackCollider.transform.position = transform.position + new Vector3(0.7f * m_facingDirection, 0, 0);

            Destroy(attackCollider, 0.37f);
        }
    }

    public void Jump() {
        if ((m_grounded || m_wallSlide) && !m_dodging && !m_ledgeGrab && !m_ledgeClimb && !m_crouching && m_disableMovementTimer < 0.0f) {
            // Check if it's a normal jump or a wall jump
            if (!m_wallSlide)
                m_body2d.velocity = new Vector2(m_body2d.velocity.x, m_jumpForce);
            else {
                m_body2d.velocity = new Vector2(-m_facingDirection * m_jumpForce / 2.0f, m_jumpForce);
                m_facingDirection = -m_facingDirection;
                m_SR.flipX = !m_SR.flipX;
            }

            m_animator.SetTrigger("Jump");
            m_grounded = false;
            m_animator.SetBool("Grounded", m_grounded);
            m_groundSensor.Disable(0.2f);
        }
    }

}
