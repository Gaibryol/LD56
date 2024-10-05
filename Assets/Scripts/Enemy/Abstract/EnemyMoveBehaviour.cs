using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
public abstract class EnemyMoveBehaviour : MonoBehaviour
{
    [SerializeField] protected float speed;
    public abstract void Move();
}

public enum MoveBehaviours
{
    Linear,
    Vertical,
    Cardioid
}