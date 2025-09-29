using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour, IDamageable
{
    public int healthPoints = 3;

    public void TakeDamage(int damage)
    {
        healthPoints -= damage;
    }
}
