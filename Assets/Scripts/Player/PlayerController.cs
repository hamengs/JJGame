using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    private Rigidbody2D playerRigidbody;
    private Animator playerAnimator;
    private Transform playerTransform;
    private Transform attackCheckPoint;
    public GameObject ghost;


    //----------Character attributes----------
    [Header("Player Attributes")]
    public int hp;
    public int attackPower;
    public float attackRadius;
    public int money;


    //-----------MoveMentParameter-------------
    [Header("Movement Parameters")]
    public float horizontalSpeed;
    public float jumpSpeed;
    public float jumpSustainSpeed;
    public float maxJumpTime;
    private bool isJumping;
    private float jumpTimeCounter;
    public int jumpTimes;
    public int facingDirection = 1;
    public float walkSlideJumpXSpeed;
    private bool canMove = true;
    private bool YvelocityReset = true;
    public float gravity = 3f;

    //----------Dash--------------
    public float dashSpeedScale;
    public float dashDuration = 0.5f;     
    public float dashCooldown = 2.0f;     
    private float dashTimer = 0f;        
    private bool isDashing = false;       
    private bool dashOnCooldown = false;

    //-----------Attack----------
    private int comboStep = 0;
    private int comboStepForDamage = 0;
    private float lastAttackTime = 0f;  
    public float comboMaxDelay = 0.5f;
    private float chargeAttackHoldTime = 0f;  // 持续按下的时间
    private bool isChargingAttack = false;    // 是否进入蓄力状态


    //---------Ghost-------------
    private Queue<GameObject> ghostQueue = new Queue<GameObject>();


    //----------Animations----------
    [Header("Animations")]
    private bool isFacingRight = true;

    //----------Ray And Layer---------
    [Header("Ray And Layer")]
    public float rayLength = 0.1f;
    public LayerMask groundLayer;
    public LayerMask enemyLayer;

    //----------Player States----------
    public enum PlayerStates
    {
        Idle, Run, Fall, Jump, WalkSlide, Attack, Block, Death, Roll, Hurt
    }
    public PlayerStates playerState;

    //----------Dust----------
    [Header("Slide Dust")]
    public GameObject slideDustPrefab;        
    public Transform slideDustSpawnPoint;     



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerRigidbody = GetComponent<Rigidbody2D>();
        playerAnimator = GetComponent<Animator>();
        playerTransform = GetComponent<Transform>();
        playerState = PlayerStates.Idle;
        attackCheckPoint = transform.Find("AttackCheckPoint");

        hp = PlayerManager.Instance.playerData.health;
        attackPower = PlayerManager.Instance.playerData.attackPower;
        money = PlayerManager.Instance.playerData.money;

    }

    // Update is called once per frame
    void Update()
    {
        RefreshPlayerData();

        if (Input.GetKeyDown(KeyCode.L))
        {
            Death();
        }

        if (playerState == PlayerStates.Death)
        {
            return;
        }
        if (canMove)
        {
            Move();
        }

        Jump();
        StateMachine();
        ProcessAttackInput();

    }

    private void FixedUpdate()
    {

    }


    public void Death()
    {
        playerAnimator.SetTrigger("Death");
        playerState = PlayerStates.Death;
        PlayerManager.Instance.isPlayerDead = true;
        gameObject.SetActive(false);
    }

    private bool DeathCheck()
    {
        if(playerState == PlayerStates.Death)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private void Move()
    {
        float h = Input.GetAxisRaw("Horizontal");

        // 处理冲刺启动：当按下Shift且不处于冷却中时启动冲刺
        if (Input.GetKey(KeyCode.LeftShift) &&
            (playerState == PlayerStates.Run || playerState == PlayerStates.Jump || playerState == PlayerStates.Fall))
        {
            if (!dashOnCooldown && !isDashing)
            {
                isDashing = true;
                dashOnCooldown = true;
                dashTimer = 0f; // 重置计时器用于记录冲刺时长
            }
        }

        // 冲刺执行逻辑
        if (isDashing)
        {
            dashTimer += Time.deltaTime;
            if (dashTimer < dashDuration)
            {
                playerRigidbody.linearVelocityX = h * horizontalSpeed * dashSpeedScale;
                GenerateGhost();
            }
            else
            {
                // 冲刺结束，重置冲刺计时器，用于后续的冷却计时
                isDashing = false;
                dashTimer = 0f;
            }
        }
        else
        {
            // 普通移动逻辑
            playerRigidbody.linearVelocityX = h * horizontalSpeed;
        }

        // 冷却逻辑：如果处于冷却中且不在冲刺状态，则更新冷却计时
        if (dashOnCooldown && !isDashing)
        {
            dashTimer += Time.deltaTime;
            if (dashTimer >= dashCooldown)
            {
                dashOnCooldown = false;
                dashTimer = 0f;
            }
        }
    }

    private void Jump()
    {
        int currentJumpTimes = jumpTimes;
        if (Input.GetButtonDown("Jump")&&isGrounded()&&jumpTimes>0)
        {

            isJumping = true;
            jumpTimeCounter = maxJumpTime;
            SetVerticalVelocity(jumpSpeed);
            currentJumpTimes--;

            
        }
        if (Input.GetButtonDown("Jump")&&playerState == PlayerStates.WalkSlide&&!isGrounded())
        {
            isJumping = true;
            jumpTimeCounter = maxJumpTime;
            WalkSlideJump(jumpSpeed, walkSlideJumpXSpeed);
            
        }
        if (Input.GetButton("Jump") && isJumping)
        {
            if(jumpTimeCounter > 0)
            {
                SetVerticalVelocity(jumpSustainSpeed);
                jumpTimeCounter -= Time.deltaTime;
            }
            else
            {
                isJumping = false;
            }
        }
        if (Input.GetButtonUp("Jump"))
        {
            isJumping = false;
        }
        if (isGrounded())
        {
            currentJumpTimes = jumpTimes;
        }


    }

    private void SetVerticalVelocity(float ySpeed)
    {
        Vector2 currentVel = playerRigidbody.linearVelocity;
        currentVel.y = ySpeed;
        playerRigidbody.linearVelocity = currentVel;
    }

    private void WalkSlideJump(float ySpeed, float xSpeed)
    {
        Vector2 currentVel = playerRigidbody.linearVelocity;
        currentVel.y = ySpeed;
        currentVel.x = xSpeed * facingDirection*-1;
        playerRigidbody.linearVelocity = currentVel;


    }
    /*
     * StateMachine, all state change must be done in this function
     * 
     */
    private void StateMachine()
    {
        //Basic check
        playerAnimator.SetFloat("AirSpeedY", playerRigidbody.linearVelocityY);
        if (isGrounded())
        {
            playerAnimator.SetBool("Grounded", true);
            playerAnimator.SetBool("WallSlide", false);
            if (YvelocityReset == true)
            {
                playerRigidbody.linearVelocityY = 0;
                YvelocityReset = false;
            }
            if (playerRigidbody.linearVelocity == Vector2.zero)
            {
                playerState = PlayerStates.Idle;
            }
        }
        else
        {
            playerAnimator.SetBool("Grounded", false);
        }

        //Facing Direction check and change base on LinearVelocityX
        if (playerRigidbody.linearVelocityX > 0)
        {
            if (isFacingRight == false)
            {
                playerTransform.Rotate(0, 180, 0);
                isFacingRight = true;
                facingDirection = 1;
            }
        }else if (playerRigidbody.linearVelocityX < 0)
        {
            if (isFacingRight == true)
            {
                playerTransform.Rotate(0, 180, 0);
                isFacingRight = false;
                facingDirection = -1;
            }
        }
        
        //Run State
        if (isGrounded())
        {
            if (playerRigidbody.linearVelocityX > 0)
            {
                playerState = PlayerStates.Run;
                

                playerAnimator.SetInteger("AnimState", 1);
                

            }
            else if (playerRigidbody.linearVelocityX < 0)
            {
                playerState = PlayerStates.Run;
                

                playerAnimator.SetInteger("AnimState", 1);
            }
            else
            {
                playerAnimator.SetInteger("AnimState", 0);
            }
        }

        //Jump State
        if (playerRigidbody.linearVelocityY > 0 && !isGrounded())
        {
            playerState = PlayerStates.Jump;
            YvelocityReset = true;
            playerAnimator.SetTrigger("Jump");
        }

        //Fall State
        
        if (playerRigidbody.linearVelocityY < 0 && !isGrounded())
        {
            playerState = PlayerStates.Fall;
            
        }

        //Walk Slide State
        if (!isGrounded()&&playerRigidbody.linearVelocityY<0)
        {
            //Right wall slide
            if (IsWallSilde() == 1)
            {

                playerState = PlayerStates.WalkSlide;
                playerAnimator.SetBool("WallSlide", true);


            }

            //Left wall slide
            if (IsWallSilde() == -1)
            {

                playerState = PlayerStates.WalkSlide;
                playerAnimator.SetBool("WallSlide", true);
            }

            if (IsWallSilde() == 0)
            {
                playerAnimator.SetBool("WallSlide", false);
            }

        }
        //Change gravity when Walking Slide
        if(playerState == PlayerStates.WalkSlide)
        {
            ChangeGravity(0.3f);
        }
        else
        {
            ChangeGravity(gravity);
        }


    }

    private void ProcessAttackInput()
    {
        // 鼠标刚按下时，重置计时器
        if (Input.GetMouseButtonDown(0))
        {
            chargeAttackHoldTime = 0f;
            isChargingAttack = false;
        }

        // 持续按住鼠标时累加时间
        if (Input.GetMouseButton(0))
        {
            chargeAttackHoldTime += Time.deltaTime;
            // 如果超过阈值且尚未进入蓄力状态，则进入蓄力攻击逻辑
            if (!isChargingAttack && chargeAttackHoldTime >= 0.15f)
            {
                playerAnimator.SetBool("ChargeAttackHolding", true);
                playerAnimator.SetTrigger("Charge");
                playerState = PlayerStates.Attack;
                isChargingAttack = true;
                canMove = false;
                playerRigidbody.linearVelocityX = 0;
            }
        }

        // 当鼠标释放时判断是蓄力攻击还是普通连击
        if (Input.GetMouseButtonUp(0))
        {
            if (isChargingAttack)
            {
                // 蓄力攻击释放：关闭蓄力动画，并触发蓄力攻击
                playerAnimator.SetBool("ChargeAttackHolding", false);
                // 重置状态
                isChargingAttack = false;
                chargeAttackHoldTime = 0f;
                canMove = true;
            }
            else
            {
                // 普通连击逻辑
                if (Time.time - lastAttackTime > comboMaxDelay)
                {
                    comboStep = 0;
                }

                comboStep++;
                lastAttackTime = Time.time;

                switch (comboStep)
                {
                    case 1:
                        playerAnimator.SetTrigger("Attack1");
                        playerState = PlayerStates.Attack;
                        comboStepForDamage = 1;
                        break;
                    case 2:
                        playerAnimator.SetTrigger("Attack2");
                        playerState = PlayerStates.Attack;
                        comboStepForDamage = 2;
                        break;
                    case 3:
                        playerAnimator.SetTrigger("Attack3");
                        playerState = PlayerStates.Attack;
                        comboStepForDamage = 3;
                        // 重置连击
                        comboStep = 0;
                        break;
                    default:
                        break;
                }
            }
        }
    }



    public void ChangeGravity(float gravityScale)
    {
        playerRigidbody.gravityScale = gravityScale;
    }

    private bool isGrounded()
    {
        RaycastHit2D hit = Physics2D.Raycast(playerTransform.position, Vector2.down, rayLength,groundLayer);
        return hit.collider != null ;
    }

    private int IsWallSilde()
    {
        Vector3 offsetRight = new Vector3(0.3f, 1.0f, 0.0f);
        Vector3 offsetLeft = new Vector3(-0.3f, 1.0f, 0.0f);
        RaycastHit2D hitRight = Physics2D.Raycast(playerTransform.position+offsetRight, Vector2.right,rayLength,groundLayer);
        RaycastHit2D hitLeft = Physics2D.Raycast(playerTransform.position+offsetLeft, Vector2.left,rayLength,groundLayer);
        if (hitLeft.collider != null)
        {
            return 1;
        }
        if (hitRight.collider != null)
        {
            return -1;
        }
        return 0;
    }

    public void Damage(int damage)
    {
        if (DeathCheck())
        {
            return;
        }
        PlayerManager.Instance.DamagePlayer(damage);
        playerAnimator.SetTrigger("Hurt");
        if (hp < 0)
        {
            Death();
        }
    }

    public void ChargeAttackEnemy()
    {
        foreach (Enemy enemy in GetAllEnemysAttacking())
        {
            int currentAttackPower = attackPower*5;

            enemy.Damage(currentAttackPower);
        }
    }

    public void AttackEnemy()
    {
            foreach(Enemy enemy in GetAllEnemysAttacking())
            {
            //combo has more damage;
            int currentAttackPower = attackPower;
            if (comboStepForDamage == 1)
            {
                currentAttackPower = attackPower;

            }else if (comboStepForDamage == 2)
            {
                currentAttackPower *= 2;
            }
            else if (comboStepForDamage == 3)
            {
                currentAttackPower *= 3;
            }
                enemy.Damage(currentAttackPower);
            }
        

    }

    private bool IsEnemyInAttackRange(Vector2 center, float radius, LayerMask layer)
    {
        Collider2D hit = Physics2D.OverlapCircle(center, radius, layer);
        if (hit != null && hit.CompareTag("Enemy"))
        {
            return true;
        }

        return false;
    }

    private Enemy[] GetAllEnemysAttacking()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(attackCheckPoint.position, attackRadius, enemyLayer);
        List<Enemy> enemies = new List<Enemy>();

        foreach(Collider2D col in hits)
        {
            Enemy enemy = col.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemies.Add(enemy);
            }
                    
        }

        return enemies.ToArray();
    }

    private void GenerateGhost()
    {
        if (ghostQueue.Count > 8)
        {
            GameObject oldGhost = ghostQueue.Dequeue();
            Destroy(oldGhost);
        }
        
        GameObject newGhost = Instantiate(ghost, playerTransform.position, playerTransform.rotation);

        
        ghostQueue.Enqueue(newGhost);

    }


    private void OnDrawGizmos()
    {
        if (playerTransform == null)
        {
            playerTransform = this.transform;
        }

        Vector3 offsetRight = new Vector3(0.3f, 1.0f, 0.0f);
        Vector3 offsetLeft = new Vector3(-0.3f, 1.0f, 0.0f);

        Vector3 startPos = playerTransform.position;
        Vector3 endPosDown = startPos + Vector3.down * rayLength;
        Vector3 endPosLeft = startPos+offsetLeft + Vector3.left * rayLength;
        Vector3 endPosRight = startPos+offsetRight + Vector3.right * rayLength;


        Gizmos.color = Color.red;
        Gizmos.DrawLine(startPos, endPosDown);
        Gizmos.DrawLine(startPos+offsetLeft, endPosLeft);
        Gizmos.DrawLine(startPos+offsetRight, endPosRight);

        RaycastHit2D hitDown = Physics2D.Raycast(startPos, Vector2.down, rayLength, groundLayer);
        if (hitDown.collider != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawSphere(hitDown.point, 0.05f);
        }


        RaycastHit2D hitRight = Physics2D.Raycast(startPos + offsetRight, Vector2.right, rayLength, groundLayer);
        if (hitRight.collider != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawSphere(hitRight.point, 0.05f);
        }

        RaycastHit2D hitLeft = Physics2D.Raycast(startPos + offsetLeft, Vector2.left, rayLength, groundLayer);
        if (hitLeft.collider != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawSphere(hitLeft.point, 0.05f);
        }

        if (attackCheckPoint == null)
        {
            attackCheckPoint = transform.Find("AttackCheckPoint");
        }
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(attackCheckPoint.position, attackRadius);
    }

    public void AE_SlideDust()
    {
        if (slideDustPrefab != null && slideDustSpawnPoint != null)
        {
            
            GameObject dust = Instantiate(slideDustPrefab, slideDustSpawnPoint.position, Quaternion.identity);
            
            Vector3 scale = dust.transform.localScale;
            scale.x = isFacingRight ? Mathf.Abs(scale.x) : -Mathf.Abs(scale.x);
            dust.transform.localScale = scale;
        }
    }


    public void RefreshPlayerData()
    {
        this.hp = PlayerManager.Instance.playerData.health;
        this.attackPower = PlayerManager.Instance.playerData.attackPower;
        this.money = PlayerManager.Instance.playerData.money;
    }

}
