using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class CharacterAnimatorController : MonoBehaviour
{
    [SerializeField]
    private string[] animatorParameters;

    private Animator _anim;
    private Dictionary<string, int> _animatorHashes = new Dictionary<string, int>();

    void Start()
    {
        _anim = GetComponent<Animator>();

        //Converting all animator variable to hashes and storing them on a dictionary to optimize.
        for(int i = 0; i < animatorParameters.Length; i++)
        {
            _animatorHashes.Add(animatorParameters[i], Animator.StringToHash(animatorParameters[i]));
        }
    }
    #region Float Parameters
    public void SetRunning(float value)
    {
        _anim.SetFloat(_animatorHashes["Running"], value);
    }
    public void SetDirection(float value)
    {
        _anim.SetFloat(_animatorHashes["Direction"], value);
    }
    #endregion
    #region Boolean Parameters
    public void SetGrounded(bool value)
    {
        _anim.SetBool(_animatorHashes["isGrounded"], value);
    }
    public void SetCombatMode(bool value)
    {
        _anim.SetBool(_animatorHashes["isCombating"], value);
    }
    public void SetBlocking (bool value)
    {
        _anim.SetBool(_animatorHashes["isBlocking"], value);
    }
    #endregion
    #region Trigger Parameters
    public void TriggerJump()
    {
        _anim.SetTrigger(_animatorHashes["Jump"]);
    }
    public void TriggerAttack()
    {
        _anim.SetTrigger(_animatorHashes["Attack"]);
    }
    /// <summary>
    /// Used when the player blocked an attack while blocking
    /// </summary>
    public void TriggerBlockHit()
    {
        _anim.SetTrigger(_animatorHashes["BlockHit"]);
    }
    public void TriggerDamageHit()
    {
        _anim.SetTrigger(_animatorHashes["DamageHit"]);
    }
    public void TriggerRandomDeath()
    {
        //Choosing between random Death1 and Death2 animations.
        _anim.SetTrigger(_animatorHashes["Death"+Random.Range(1,3)]);
    }
    #endregion
}
