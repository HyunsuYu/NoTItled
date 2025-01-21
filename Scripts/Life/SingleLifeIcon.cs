using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;


public class SingleLifeIcon : MonoBehaviour
{
    internal enum LifeIconType
    {
        Full,
        Half,
        None,

        Changing
    }


    [SerializeField] private Image m_image_LifeIcon;


    internal void SetIconState(in LifeIconType lifeIconType)
    {
        switch(lifeIconType)
        {
            case LifeIconType.Full:
                m_image_LifeIcon.sprite = LifeManager.Instance.LifeIcons[0];
                break;

            case LifeIconType.Half:
                m_image_LifeIcon.sprite = LifeManager.Instance.LifeIcons[1];
                break;

            case LifeIconType.None:
                m_image_LifeIcon.sprite = LifeManager.Instance.LifeIcons[2];
                break;

            case LifeIconType.Changing:
                m_image_LifeIcon.sprite = LifeManager.Instance.LifeIcons[3];
                break;
        }
    }
}