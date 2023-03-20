using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Collider))]
public class MeleeWeapon : BaseWeapon
{

    public bool canDamage = false;
    public ParticleSystem WieldTrails_Particles;

    public void Start()
    {
        WieldTrails_Particles = transform.GetChild(0).GetComponent<ParticleSystem>();
        Equip(wielder);
    }

    public override void Equip(CharacterActionController newWielder)
    {
        base.Equip(newWielder);
  
        wielder.OnAttack += SetTrueCanDamage;
        wielder.OnAttackEnd += SetFalseCanDamage;
    }

    private void OnTriggerEnter(Collider other)
    {
        IDamageable damageableObj = other.GetComponent<IDamageable>();
        CharacterHealthController character = (CharacterHealthController)damageableObj;
        if (isEquiped && canDamage && damageableObj != null && character != wielderHealth)
        {
            Debug.Log(damageableObj);
            damageableObj.Damage(baseDamage);
        }
    }

    public void SetTrueCanDamage()
    {
        canDamage = true;
        WieldTrails_Particles.Play();
    }

    public void SetFalseCanDamage()
    {
        canDamage = false;
        WieldTrails_Particles.Stop();
    }

    public void OnDestroy()
    {
        wielder.OnAttack -= SetTrueCanDamage;
        wielder.OnAttackEnd -= SetFalseCanDamage;
    }
}
