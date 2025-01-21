using System.Collections;
using System.Collections.Generic;

using UnityEngine;


public class Item_Potion : MonoBehaviour
{
    private bool m_bisItemInteractable = false;


    public void Update()
    {
        if (m_bisItemInteractable && Input.GetMouseButtonDown(1))
        {
            LifeManager.Instance.CurLife += 1;
            gameObject.SetActive(false);
        }
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            m_bisItemInteractable = true;
        }
    }
    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            m_bisItemInteractable = false;
        }
    }
}