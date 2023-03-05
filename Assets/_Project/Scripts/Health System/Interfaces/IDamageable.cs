using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageable 
{
    int MaxHealth { get; set; }
    int Health { get; set; }

    void Damage(int damageAmount);

    void Heal(int healAmount);

    /// <summary>
    /// Increases <see cref="MaxHealth"/> variable so character can have a larger pool of health.
    /// </summary>
    /// <param name="increaseAmount"></param>
    void IncreaseHealth(int increaseAmount);

    void DecreaseHealth(int decreaseAmount);

    void Death();

}
