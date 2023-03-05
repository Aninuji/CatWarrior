using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

public class FollowPlayer : ActionNode
{
    public float speed = 5;

    [Tooltip("Minimum distance will stop. It is used to prevent anoying collision with player.")]
    public float stoppingDistance = 0.1f;
    public bool updateRotation = true;
    public float acceleration = 40.0f;
    public float tolerance = 1.0f;
    [Tooltip("Maximum distance the AI will follow the player before it stops. Do not confuse with stopping distance.")]
    public float OutOfReachDistance = 5.0f;
    protected override void OnStart()
    {
        context.agent.stoppingDistance = stoppingDistance;
        context.agent.speed = speed;
        //context.agent.destination = blackboard.moveToPosition;
        context.agent.updateRotation = updateRotation;
        context.agent.acceleration = acceleration;
        context.characterActionController.checkGravity = false;
        context.characterController.enabled = false;
    }

    protected override void OnStop()
    {
    }

    protected override State OnUpdate()
    {
        context.characterAIController.Move(GameManager.Instance._player.transform.position, true);

        if (context.agent.pathPending)
        {
            return State.Running;
        }

        if (context.agent.remainingDistance < tolerance ||
            Vector3.Distance(context.transform.position, GameManager.Instance._player.transform.position) > OutOfReachDistance)
        {
            context.characterActionController.checkGravity = true;
            context.characterController.enabled = true;

            return State.Success;
        }

        if (context.agent.pathStatus == UnityEngine.AI.NavMeshPathStatus.PathInvalid)
        {
            return State.Failure;
        }

        return State.Running;
    }
}
