using System;
using UnityEngine;

public class Skeleton : Enemy
{
    //Attack
    private Transform attackCheckPoint;
    public float attackCooldown = 1.0f;  
    private float attackTimer = 0f;

    //Force move reboot
    private float moveCooldown = 2.0f;
    private float moveTimer = 0f;

    public PlayerController player; 

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        base.Start();
        attackCheckPoint = transform.Find("AttackCheckPoint");

    }

    private void Update()
    {
        if (DeathCheck())
        {
            return;
        }
        TimerUpdate();
        Move();
        StateMachine();
    }

    public override void Damage(int damage)
    {
        if (enemyState == EnemyStates.Death)
        {
            return;
        }
        else
        {
            hp -= damage;
            if (hp <= 0)
            {
                Death();
            }
            else
            {
                enemyAnimator.SetTrigger("Hurt");
            }


        }
    }

    protected override void Death()
    {
        enemyAnimator.SetBool("Walk", false);
        enemyAnimator.SetTrigger("Death");

        enemyState = EnemyStates.Death;
        Destroy(this.gameObject, 5f);
    }

    protected override bool DeathCheck()
    {
        if (enemyState == EnemyStates.Death)
        {
            return true;
        }
        else
        {
            return false;
        }
    }


    protected override void Move()
    {
        if (moveTimer > moveCooldown)
        {
            enemyState = EnemyStates.Walk;
        }


        AnimatorStateInfo stateInfo = enemyAnimator.GetCurrentAnimatorStateInfo(0);
        if (stateInfo.IsName("Attack")||enemyState == EnemyStates.Attack)
        {
            return;
        }
        
        

        Vector3 scale = enemyTransform.localScale;
        if (IsHitWall() == 1)
        {
            if (facingDirection == 1)
            {
                scale.x = -Mathf.Abs(scale.x);
                enemyTransform.localScale = scale;
            }
            facingDirection = -1;

        }
        else if (IsHitWall() == -1)
        {
            if (facingDirection == -1)
            {
                scale.x = Mathf.Abs(scale.x);
                enemyTransform.localScale = scale;
            }
            facingDirection = 1;

        }

        if (IsEdge())
        {
            if (facingDirection == 1)
            {
                scale.x = -Mathf.Abs(scale.x);
                enemyTransform.localScale = scale;
                facingDirection = -1;

            }
            else if (facingDirection == -1)
            {
                scale.x = Mathf.Abs(scale.x);
                enemyTransform.localScale = scale;
                facingDirection = 1;
            }

            enemyState = EnemyStates.Idle;
            enemyRigidbody.linearVelocity = Vector2.zero;
            moveTimer = 0f;


            
        }

        if (enemyState != EnemyStates.Idle)
        {
            enemyRigidbody.linearVelocityX = facingDirection * horizontalSpeed;
        }

    }

    private void TimerUpdate()
    {
        attackTimer += Time.deltaTime;
        if (enemyState != EnemyStates.Walk)
        {
            moveTimer += Time.deltaTime;
        }

    }



    protected override void StateMachine()
    {
        
        if (AttackStateCheck())
        {
            enemyState = EnemyStates.Attack;
            moveTimer = 0f;
            enemyRigidbody.linearVelocity = Vector2.zero;
           
            enemyAnimator.SetBool("Walk", false);
        }
        else
        {
            
            if (Mathf.Abs(enemyRigidbody.linearVelocity.x) > 0)
            {
                enemyState = EnemyStates.Walk;
                enemyAnimator.SetBool("Walk", true);
            }
            else
            {
                enemyAnimator.SetBool("Walk", false);
            }
        }

    }


    protected bool AttackStateCheck()
    {
        if (attackTimer < attackCooldown)
        {
            
            return false;
        }


        if (IsPlayerInAttackRange(attackCheckPoint.position, attackRadius, playerLayer))
        {
            enemyAnimator.SetTrigger("Attack1");

            attackTimer = 0f;
            return true;
        }
        else
        {

            return false;
        }
    }

    public void AttackPlayer()
    {
        if(IsPlayerInAttackRange(attackCheckPoint.position, attackRadius, playerLayer))
        {
            player.Damage(attackPower);
        }

    }



    protected override void OnDrawGizmos()
    {
        if (enemyTransform == null)
        {
            enemyTransform = this.transform;
        }

        Vector2 startPos = enemyTransform.position;
        Vector2 endPosDown = startPos + Vector2.down + new Vector2(0, -rayLength);
        Vector2 endPosRight = startPos + Vector2.right * rayLength * 7;
        Vector2 endPosLeft = startPos + Vector2.left * rayLength * 5;

        Vector2 startPosEdge = edgeTestPoint.position;
        Vector2 endPosEdge = edgeTestPoint.position + Vector3.down * rayLength;

        Gizmos.color = Color.red;
        Gizmos.DrawLine(startPos, endPosDown);
        Gizmos.DrawLine(startPos, endPosRight);
        Gizmos.DrawLine(startPos, endPosLeft);
        Gizmos.DrawLine(startPosEdge, endPosEdge);

        RaycastHit2D hitRight = Physics2D.Raycast(enemyTransform.position, Vector2.right, rayLength * 7, groundLayer);
        RaycastHit2D hitLeft = Physics2D.Raycast(enemyTransform.position, Vector2.left, rayLength * 5, groundLayer);
        RaycastHit2D edge = Physics2D.Raycast(edgeTestPoint.position, Vector2.down, rayLength, groundLayer);
        Gizmos.color = Color.green;
        if (hitRight.collider != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawSphere(hitRight.point, 0.05f);
        }
        if (hitLeft.collider != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawSphere(hitLeft.point, 0.05f);
        }

        if (edge.collider != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawSphere(edge.point, 0.05f);
        }

        if (attackCheckPoint == null)
        {
            attackCheckPoint = transform.Find("AttackCheckPoint");
        }
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(attackCheckPoint.position, attackRadius);
    }
}
