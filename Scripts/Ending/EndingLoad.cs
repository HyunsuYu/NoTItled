using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.SceneManagement;


public class EndingLoad : MonoBehaviour
{
    [SerializeField] private List<GameObject> m_boards;

    private float m_fTime = 0.0f;
    private int m_iIndex = 0;

    private bool bisEnd = false;


    public void Update()
    {
        if(bisEnd)
        {
            if(Input.anyKeyDown)
            {
                SceneManager.LoadScene(0);
            }

            return;
        }

        m_fTime += Time.deltaTime;
        if (m_fTime >= 5.0f)
        {
            if(m_iIndex + 1 < m_boards.Count)
            {
                m_boards[m_iIndex].SetActive(false);
            }
            m_iIndex++;
            m_fTime = 0.0f;
        }
        if (m_iIndex >= m_boards.Count)
        {
            bisEnd = true;
        }

        if(!bisEnd)
        {
            m_boards[m_iIndex].SetActive(true);
        }
    }
}