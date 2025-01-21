using System.Collections;
using System.Collections.Generic;

using UnityEngine;


public class Bullet : MonoBehaviour
{
    internal struct StartData
    {
        public float LifeTime;
    }


    private float m_bulletFileTime = 0.0f;
    private float m_bulletLifeTimer = 0.0f;

    private bool m_bisShoot = false;


    public void FixedUpdate()
    {
        if (m_bisShoot)
        {
            m_bulletLifeTimer += Time.fixedDeltaTime;
            if (m_bulletLifeTimer >= m_bulletFileTime)
            {
                m_bisShoot = false;
                m_bulletLifeTimer = 0.0f;

                ChiefPlayerManager.Instance.GunManager.Pool.Release(gameObject);
            }
        }
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.GetComponent<BubbleTile>() != null)
        {
            collision.gameObject.GetComponent<BubbleTile>().Pop();
        }
    }

    internal void StartShoot(in StartData startData)
    {
        m_bulletFileTime = startData.LifeTime;

        m_bisShoot = true;
    }
}