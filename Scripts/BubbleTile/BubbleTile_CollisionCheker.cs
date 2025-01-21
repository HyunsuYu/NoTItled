using System.Collections;
using System.Collections.Generic;

using UnityEngine;


public class BubbleTile_CollisionCheker : MonoBehaviour
{
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            LifeManager.Instance.CurLife = 0;
        }
    }
}