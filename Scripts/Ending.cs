using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.SceneManagement;


public class Ending : MonoBehaviour
{
    private static Ending m_instance;


    public bool BIsBossKilled = false;
    public bool BIsPHDWithMe = false;


    public void Awake()
    {
        m_instance = this;
    }

    public void OnTriggerStay2D(Collider2D collision)
    {
        if (BIsBossKilled && !BIsPHDWithMe)
        {
            SceneManager.LoadScene(4);
        }
        else if(!BIsBossKilled && BIsPHDWithMe)
        {
            SceneManager.LoadScene(3);
        }
        else if(BIsBossKilled && BIsPHDWithMe)
        {
            SceneManager.LoadScene(5);
        }
    }

    internal static Ending Instance
    {
        get
        {
            return m_instance;
        }
    }
}