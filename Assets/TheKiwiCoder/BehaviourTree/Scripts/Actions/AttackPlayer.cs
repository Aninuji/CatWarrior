using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

public class AttackPlayer : ActionNode
{
    //private float _Damage = 5;
    private bool _didAttackEnd = false;
    
    protected override void OnStart()
    {
        Debug.Log("Entering Attack State");
        context.characterActionController.Attack();
        context.characterActionController.OnAttackEnd += AttackEnd;
    }

    protected override void OnStop()
    {
        context.characterActionController.OnAttackEnd -= AttackEnd;
    }

    protected override State OnUpdate()
    {
        if (_didAttackEnd) return State.Success;

        return State.Running;
    }

    private void AttackEnd()
    {
        _didAttackEnd = true;
    }
}
