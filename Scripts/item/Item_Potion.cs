using System.Collections;
using System.Collections.Generic;

using UnityEngine;


public class Item_Potion : MonoBehaviour
{
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player" && LifeManager.Instance.CurLife + 1 <= LifeManager.Instance.MaxLife)
        {
            LifeManager.Instance.CurLife += 1;

            gameObject.SetActive(false);
        }
    }
}