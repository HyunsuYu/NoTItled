using System.Collections;
using System.Collections.Generic;

using UnityEngine;


public class BubbleTile : MonoBehaviour
{
    private Animator m_animator;
    private BoxCollider2D m_boxCollider2D;
    [SerializeField] private BoxCollider2D m_collisionChecker;

    private float m_bulletFileTime = 3.0f;
    private float m_bulletLifeTimer = 0.0f;

    private bool m_bisPoped = false;


    public void Awake()
    {
        m_animator = GetComponent<Animator>();
        m_boxCollider2D = GetComponent<BoxCollider2D>();
    }
    public void FixedUpdate()
    {
        if (m_bisPoped)
        {
            m_bulletLifeTimer += Time.fixedDeltaTime;
            if (m_bulletLifeTimer >= m_bulletFileTime)
            {
                m_boxCollider2D.enabled = true;
                m_collisionChecker.enabled = true;

                m_bisPoped = false;
                m_bulletLifeTimer = 0.0f;

                m_animator.Play("BubbleTile_Restore");
            }
        }
    }

    internal void Pop()
    {
        m_animator.Play("BubbleTilePop");

        m_boxCollider2D.enabled = false;
        m_collisionChecker.enabled = false;
        m_bisPoped = true;
    }
}