using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;


public class Item_FuelBox : MonoBehaviour
{
    private bool m_bisItemInteractable = false;


    public void Update()
    {
        if(m_bisItemInteractable && Input.GetMouseButtonDown(1))
        {
            ChiefPlayerManager.Instance.BIsFlamethrowerUsable = true;

            ChiefPlayerManager.Instance.SubWeaponSlot.m_image_MainImage.color = Color.white;
            ChiefPlayerManager.Instance.SubWeaponSlot.m_image_FlamethrowerBackground.color = Color.white;
            ChiefPlayerManager.Instance.SubWeaponSlot.m_rectTransform_RemainFlamethrowerDurability.GetComponent<Image>().color = Color.white;

            ChiefPlayerManager.Instance.FlamethrowerManager.CurDurability = ChiefPlayerManager.Instance.FlamethrowerManager.FlamethrowerConfig.FlamethrowerDurability;

            Destroy(gameObject);
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