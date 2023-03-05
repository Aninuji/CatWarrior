using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterHealthController), typeof(CharacterActionController))]
public class CharacterVFXController : MonoBehaviour
{
    [SerializeField]
    private float _damageFXStopTime = 0.1f;
    [SerializeField]
    private ParticleSystem _healParticleSystem;
    [SerializeField]
    private ParticleSystem _damageParticleSystem;


    [SerializeField]
    private SkinnedMeshRenderer _characterRenderer;

    private CharacterHealthController healthController;
    private CharacterActionController actionController;

    private Coroutine damageCoroutine;

    // Start is called before the first frame update
    void Start()
    {
        healthController = GetComponent<CharacterHealthController>();
        actionController = GetComponent<CharacterActionController>();
        healthController.OnDamage += PlayDamageFX;
        healthController.OnDeath += PlayDeathFX;
        healthController.OnHeal += PlayHealFX;
        actionController.OnAttack += PlayAttackFX;
        actionController.OnAttackEnd += StopAttackFX;
    }
    private void OnDestroy()
    {
        healthController.OnDamage -= PlayDamageFX;
        healthController.OnHeal -= PlayHealFX;
        actionController.OnAttack -= PlayAttackFX;
        actionController.OnAttackEnd -= StopAttackFX;
        healthController.OnDeath -= PlayDeathFX;

    }
    public void PlayDeathFX()
    {

        _characterRenderer.material.color = Color.red;
        _damageParticleSystem.Play();

        if (damageCoroutine != null) StopCoroutine(damageCoroutine);
        damageCoroutine = StartCoroutine(stopDamageFX());
    }
    public void PlayDamageFX()
    {
       
        _characterRenderer.material.color = Color.red;
        _damageParticleSystem.Play();

        if (damageCoroutine != null) StopCoroutine(damageCoroutine);
        damageCoroutine = StartCoroutine(stopDamageFX());
    }

    public IEnumerator stopDamageFX()
    {
        yield return new WaitForSeconds(_damageFXStopTime);
        _characterRenderer.material.color = Color.white;

    }
    public void PlayHealFX()
    {
        _healParticleSystem.Play();
    }

    public void PlayJumpFX()
    {

    }

    public void PlayAttackFX()
    {
    }

    public void StopAttackFX()
    {
    }

}
