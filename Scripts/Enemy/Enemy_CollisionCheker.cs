using System.Collections;
using System.Collections.Generic;

using UnityEngine;


public class Enemy_CollisionCheker : MonoBehaviour
{
    private float m_bulletFileTime = 0.5f;
    [SerializeField] private float m_bulletLifeTimer = 0.0f;

    private bool m_bisTouching = false;
    private bool m_bisDead = false;

    [SerializeField] private bool m_bisPHD = false;


    public void FixedUpdate()
    {
        if (m_bisTouching && !m_bisDead)
        {
            m_bulletLifeTimer += Time.fixedDeltaTime;
            if (m_bulletLifeTimer >= m_bulletFileTime)
            {
                m_bulletLifeTimer = 0.0f;

                //if (m_bisPHD)
                //{
                //    //LifeManager.Instance.CurLife -= 1;
                //    Debug.Log("PHD_HP : " + NormalNPC.PHD_HP);
                //    NormalNPC.PHD_HP -= 1;

                //    //Debug.Log("PHD_HP : " + NormalNPC.PHD_HP);
                //}
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
        if (collision.gameObject.tag == "PHD")
        {
            NormalNPC.PHD_HP -= 1;
            Debug.Log("PHD_HP : " + NormalNPC.PHD_HP);

            m_bisPHD = true;
            m_bisDead = true;
        }
    }
    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            m_bisTouching = false;
        }
        if(collision.gameObject.tag == "PHD")
        {
            m_bisPHD = false;
            m_bisDead = false;
        }
    }

    public bool BIsDead
    {
        get { return m_bisDead; }
        set { m_bisDead = value; }
    }
}