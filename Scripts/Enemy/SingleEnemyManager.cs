using System.Collections;
using System.Collections.Generic;

using UnityEngine;


public class SingleEnemyManager : MonoBehaviour
{
    [SerializeField] private Transform m_transform_EnemyMoveRange_Left;
    [SerializeField] private Transform m_transform_EnemyMoveRange_Right;

    private Rigidbody2D m_rigidbody2D;
    private SpriteRenderer m_spriteRenderer;

    private PlayerMovement.MovementDirection m_movementDirection = PlayerMovement.MovementDirection.Left;

    private float speed = 3.0f;


    public void Awake()
    {
        m_rigidbody2D = GetComponent<Rigidbody2D>();
        m_spriteRenderer = GetComponent<SpriteRenderer>();

        switch(m_movementDirection)
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
        switch(m_movementDirection)
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
}