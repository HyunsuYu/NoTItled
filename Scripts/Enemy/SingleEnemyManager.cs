using System.Collections;
using System.Collections.Generic;

using UnityEngine;


public class SingleEnemyManager : MonoBehaviour
{
    [SerializeField] private Transform m_transform_EnemyMoveRange_Left;
    [SerializeField] private Transform m_transform_EnemyMoveRange_Right;

    [SerializeField] private Enemy_CollisionCheker m_enemy_CollisionCheker;

    private Rigidbody2D m_rigidbody2D;
    private SpriteRenderer m_spriteRenderer;

    private PlayerMovement.MovementDirection m_movementDirection = PlayerMovement.MovementDirection.Left;

    private float speed = 3.0f;

    private Animator m_animator;
    private BoxCollider2D m_boxCollider2D;

    private bool m_bisDead = false;


    public void Awake()
    {
        m_rigidbody2D = GetComponent<Rigidbody2D>();
        m_spriteRenderer = GetComponent<SpriteRenderer>();

        m_animator = GetComponent<Animator>();
        m_boxCollider2D = GetComponent<BoxCollider2D>();

        switch (m_movementDirection)
        {
            case PlayerMovement.MovementDirection.Left:
                m_spriteRenderer.flipX = true;
                break;

            case PlayerMovement.MovementDirection.Right:
                m_spriteRenderer.flipX = false;
                break;
        }
    }

    public void FixedUpdate()
    {
        if(m_bisDead)
        {
            return;
        }

        switch (m_movementDirection)
        {
            case PlayerMovement.MovementDirection.Left:
                {
                    m_rigidbody2D.velocity = new Vector2(-speed, m_rigidbody2D.velocity.y);

                    if (transform.position.x <= m_transform_EnemyMoveRange_Left.position.x)
                    {
                        m_movementDirection = PlayerMovement.MovementDirection.Right;
                        m_spriteRenderer.flipX = false;
                    }
                }
                break;

            case PlayerMovement.MovementDirection.Right:
                {
                    m_rigidbody2D.velocity = new Vector2(speed, m_rigidbody2D.velocity.y);

                    if(transform.position.x >= m_transform_EnemyMoveRange_Right.position.x)
                    {
                        m_movementDirection = PlayerMovement.MovementDirection.Left;
                        m_spriteRenderer.flipX = true;
                    }
                }
                break;
        }
    }

    internal void SetDead()
    {
        m_animator.Play("Enemy_Dead");
        //m_boxCollider2D.enabled = false;
        m_enemy_CollisionCheker.BIsDead = true;

        m_rigidbody2D.velocity = Vector2.zero;

        Invoke("SetAlive", 2.0f);

        m_bisDead = true;
    }
    internal void SetRealDead()
    {
        m_animator.Play("Enemy_Dead");

        Invoke("DestroyThis", 1.0f);
    }

    private void SetAlive()
    {
        m_boxCollider2D.enabled = true;
        m_animator.Play("Enemy_Alive");

        m_enemy_CollisionCheker.BIsDead = false;

        //m_bisDead = false;
        Invoke("SetIsNotDead", 2.0f);
    }

    private void DestroyThis()
    {
        Destroy(gameObject);
    }

    private void SetIsNotDead()
    {
        m_bisDead = false;
    }
}