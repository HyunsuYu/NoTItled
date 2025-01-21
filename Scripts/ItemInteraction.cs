using System.Collections;
using System.Collections.Generic;

using UnityEngine;


public class ItemInteraction : MonoBehaviour
{
    [SerializeField] private GameObject m_InteractionIcon;


    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            m_InteractionIcon.SetActive(true);
        }
    }
    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            m_InteractionIcon.SetActive(false);
        }
    }
}