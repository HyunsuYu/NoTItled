using System.Collections;
using System.Collections.Generic;

using UnityEngine;


[CreateAssetMenu(fileName = "MovementConfig", menuName = "Movement/MovementConfig", order = 1)]
public class MovementConfig : ScriptableObject
{
    public float MovementSpeed = 10.0f;
}