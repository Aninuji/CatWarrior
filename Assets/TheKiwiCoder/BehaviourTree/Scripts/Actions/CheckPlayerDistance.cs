using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

public class CheckPlayerDistance : ActionNode
{
    public float CloseDistance = 1.0f;
  

    protected override void OnStart() {

    }

    protected override void OnStop() {
    }

    protected override State OnUpdate() {

        if(Vector3.Distance(context.transform.position, GameManager.Instance._player.transform.position) < CloseDistance)
        {
            return State.Success;
        }
        else
        {
            return State.Running;
        }
    }
}
