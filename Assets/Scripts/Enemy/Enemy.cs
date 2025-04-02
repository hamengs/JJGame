using System;
using UnityEngine;
using static PlayerController;

public class Enemy : MonoBehaviour
{

    protected Rigidbody2D enemyRigidbody;
    protected Animator enemyAnimator;
    protected Transform enemyTransform;
    protected SpriteRenderer enemySpriteRenderer;
    protected Transform attackCheckPoint;
   [SerializeField] protected Transform edgeTestPoint;

    //----------Character Attributes----------
    [Header("Enemy Attributes")]
    public int hp;
    public int attackPower;
    public float attackRadius;

    //-----------MoveMentParameter-------------
    [Header("Movement Parameters")]
    public float horizontalSpeed;
    public float dashSpeedScale;
    public float jumpSpeed;
    public float jumpSustainSpeed;
    public float maxJumpTime;
    private bool isJumping;
    private float jumpTimeCounter;
    public int jumpTimes;
    public int facingDirection = 1;
    public float walkSlideJumpXSpeed;

    //----------Animations----------
    [Header("Animations")]
    protected bool isFacingRight = true;

    //----------Ray And Layer---------
    [Header("Ray And Layer")]
    public float rayLength = 0.1f;
    public LayerMask groundLayer;
    public LayerMask playerLayer;

    //----------Player States----------
    public enum EnemyStates
    {
        Idle, Walk, Fall,Attack, Block, Death, Hurt, Chase, Patrol
    }
    public EnemyStates enemyState;

    protected virtual void Start()
    {
        enemyRigidbody = GetComponent<Rigidbody2D>();
        enemyAnimator = GetComponent<Animator>();
        enemyTransform = GetComponent<Transform>();
        enemySpriteRenderer = GetComponent<SpriteRenderer>();
        edgeTestPoint = transform.Find("EdgeTestPoint");
    }

    protected virtual void Update()
    {
        if (DeathCheck())
        {
            return;
        }
        Move();
        StateMachine();
    }

    protected virtual void Move()
    {
        enemyRigidbody.linearVelocityX = facingDirection * horizontalSpeed;
        Debug.Log(IsHitWall());
        if (IsHitWall() == 1)
        {
            if(facingDirection == 1)
            {
                enemySpriteRenderer.flipX = true;
            }
            facingDirection = -1;

        }else if (IsHitWall() == -1)
        {
            if (facingDirection == -1)
            {
                enemySpriteRenderer.flipX = false;
            }
            facingDirection = 1;

        }

    }

    protected virtual bool Attack()
    {
        return false;
    }

    public virtual void Damage(int damage)
    {
        if(enemyState == EnemyStates.Death)
        {
            return;
        }
        else
        {
            hp -= damage;
            enemyAnimator.SetTrigger("Hurt");
            if (hp <= 0)
            {
                Death();
            }
        }
    }

    protected virtual void Death()
    {
        enemyAnimator.SetTrigger("Death");
        enemyState = EnemyStates.Death;
        Destroy(this.gameObject, 5f);
    }

    protected virtual bool DeathCheck()
    {
        if(enemyState == EnemyStates.Death)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    protected virtual void StateMachine()
    {
        if (enemyRigidbody.linearVelocityX != 0)
        {
            enemyState = EnemyStates.Walk;
            enemyAnimator.SetBool("Walk", true);
        } else if(enemyRigidbody.linearVelocityX == 0)
        {
            enemyAnimator.SetBool("Walk", false);
        }
        
    }

    protected int IsHitWall()
    {
        RaycastHit2D hitRight = Physics2D.Raycast(enemyTransform.position, Vector2.right, rayLength*7, groundLayer);
        RaycastHit2D hitLeft = Physics2D.Raycast(enemyTransform.position, Vector2.left, rayLength*5, groundLayer);
        if (hitRight.collider != null)
        {
            return 1;
        }

        if (hitLeft.collider != null)
        {
            return -1;
        } 

        return 0;
    }

    //Help method that check if near the edge
    protected bool IsEdge()
    {
        RaycastHit2D edge = Physics2D.Raycast(edgeTestPoint.position, Vector2.down, rayLength, groundLayer);

        if (edge.collider == null)
        {
            return true;
        }


        return false;
    }

    //Check if someone ahead of.
    protected bool IsPlayerInAttackRange(Vector2 center, float radius, LayerMask layer)
    {
        Collider2D hit = Physics2D.OverlapCircle(center, radius, layer);
        if (hit != null && hit.gameObject.tag == ("Player"))
        {
            return true;
        }

        return false;
    }


    protected virtual void OnDrawGizmos()
    {
        if (enemyTransform == null)
        {
            enemyTransform = this.transform;
        }
        if (edgeTestPoint == null)
        {
            edgeTestPoint = transform.Find("EdgeTestPoint");
        }

        Vector2 startPos = enemyTransform.position;
        Vector2 endPosDown = startPos + Vector2.down + new Vector2(0,-rayLength);
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

    }
}
