using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;


public class Item_FuelBox : MonoBehaviour
{
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            ChiefPlayerManager.Instance.BIsFlamethrowerUsable = true;

            ChiefPlayerManager.Instance.SubWeaponSlot.m_image_MainImage.color = Color.white;
            ChiefPlayerManager.Instance.SubWeaponSlot.m_image_FlamethrowerBackground.color = Color.white;
            ChiefPlayerManager.Instance.SubWeaponSlot.m_rectTransform_RemainFlamethrowerDurability.GetComponent<Image>().color = Color.white;

            ChiefPlayerManager.Instance.FlamethrowerManager.CurDurability = ChiefPlayerManager.Instance.FlamethrowerManager.FlamethrowerConfig.FlamethrowerDurability;

            Destroy(gameObject);
        }
    }
}