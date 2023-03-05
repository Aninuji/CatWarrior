using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseWeapon : MonoBehaviour
{
    public CharacterActionController wielder;
    [SerializeField]
    protected int _baseDamage;
    public int baseDamage
    {
        get { return _baseDamage; }
        set { _baseDamage = value; }
    }

    protected bool _isEquiped;

    public bool isEquiped
    {
        get { return _isEquiped; }
        set { _isEquiped = value; }
    }


    protected CharacterHealthController wielderHealth;
    public void MakeDamage(IDamageable target, int damageModifier)
    {
        int finalDamage = _baseDamage * damageModifier;
        target.Damage(_baseDamage);
    }

    public virtual void Equip(CharacterActionController newWielder)
    {
        wielder = newWielder;
        wielderHealth = wielder.GetComponent<CharacterHealthController>();
        _isEquiped = true;
    }
}
