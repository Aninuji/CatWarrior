using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterAnimatorController))]
public class CharacterHealthController : MonoBehaviour, IDamageable
{
    /// <summary>
    /// After getting damage; How many time will pass before the character can take more damage.
    /// </summary>
    public float DamageInvincibilityTime;
    /// <summary>
    /// After character's death; How many time will pass before the character dissapears from scene.
    /// </summary>
    public float DeathDisappearTime;

    [SerializeField]
    private int _maxHealth;
    [SerializeField]
    private int _health;

    /// <summary>
    /// Current's character health.
    /// </summary>
    public int Health 
    { 
        get 
        {
            return _health;
        }
        set 
        {
            _health = value;
        }
    }

    /// <summary>
    /// Maximum character health.
    /// </summary>
    public int MaxHealth
    {
        get
        {
            return _maxHealth;
        }
        set
        {
            _maxHealth = value;
        }
    }

    [SerializeField]
    private bool canBeDamaged = true;

    #region Dependencies
    private CharacterAnimatorController animatorController;
    #endregion

    public delegate void CharacterHealthEventTrigger();
    public event CharacterHealthEventTrigger OnDamage;
    public event CharacterHealthEventTrigger OnDeath;
    public event CharacterHealthEventTrigger OnHeal;


    private Coroutine damageTimer;
    // Start is called before the first frame update
    void Start()
    {
        animatorController = GetComponent<CharacterAnimatorController>();

        _health = _maxHealth;
    }

    public void Damage(int damageAmount)
    {
        if (!canBeDamaged) return;
        _health -= damageAmount;


        if (OnDamage != null) OnDamage();

        if (_health <= 0)
        {
            Death();
            return;
        }

        animatorController.TriggerDamageHit();
        canBeDamaged = false;



        if (damageTimer != null) StopCoroutine(damageTimer);
        damageTimer = StartCoroutine(DamageInvincibilityTimer());


    }

    public void Death()
    {
        if (damageTimer != null) StopCoroutine(damageTimer);

        animatorController.TriggerRandomDeath();
        canBeDamaged = false;

        if (OnDeath != null) OnDeath();

        Destroy(this.gameObject, DeathDisappearTime);
    }

    public void Heal(int healAmount)
    {
        _health += healAmount;

        if (_health > _maxHealth) _health = _maxHealth;
        if (OnHeal != null) OnHeal();
    }

    public void IncreaseHealth(int increaseAmount)
    {
        _maxHealth += increaseAmount;
    }

    public void DecreaseHealth(int decreaseAmount)
    {
        _maxHealth -= decreaseAmount;

    }


    public IEnumerator DamageInvincibilityTimer()
    {
        yield return new WaitForSeconds(DamageInvincibilityTime);
        canBeDamaged = true;
    }
}
