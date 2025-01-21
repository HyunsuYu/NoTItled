using System.Collections;
using System.Collections.Generic;

using UnityEngine;


public class Fire : MonoBehaviour
{
    internal struct StartData
    {
        public float LifeTime;
    }


    private float m_fireFileTime = 0.0f;
    private float m_fireLifeTimer = 0.0f;

    private bool m_bisShoot = false;

    //private Rigidbody2D m_rigidbody2D;

    private float m_rotateSpped = 0.0f;


    public void FixedUpdate()
    {
        if (m_bisShoot)
        {
            m_fireLifeTimer += Time.fixedDeltaTime;
            if (m_fireLifeTimer >= m_fireFileTime)
            {
                m_bisShoot = false;
                m_fireLifeTimer = 0.0f;

                ChiefPlayerManager.Instance.FlamethrowerManager.Pool.Release(gameObject);
            }
        }
    }

    internal void StartShoot(in StartData startData)
    {
        //m_rigidbody2D = GetComponent<Rigidbody2D>();

        transform.rotation = Quaternion.Euler(0.0f, 0.0f, UnityEngine.Random.Range(0, 360.0f) + transform.rotation.z);

        m_fireFileTime = startData.LifeTime;

        m_bisShoot = true;
    }
}