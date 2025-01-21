using System.Collections;
using System.Collections.Generic;

using UnityEngine;


public class Enemy_CollisionCheker : MonoBehaviour
{
    private float m_bulletFileTime = 0.5f;
    [SerializeField] private float m_bulletLifeTimer = 0.0f;

    private bool m_bisTouching = false;


    public void FixedUpdate()
    {
        if (m_bisTouching)
        {
            m_bulletLifeTimer += Time.fixedDeltaTime;
            if (m_bulletLifeTimer >= m_bulletFileTime)
            {
                m_bulletLifeTimer = 0.0f;

                LifeManager.Instance.CurLife -= 1;
            }
        }
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            m_bisTouching = true;
        }
    }
    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            m_bisTouching = false;
        }
    }
}