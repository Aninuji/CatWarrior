using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(CharacterActionController), typeof(NavMeshAgent), typeof(CharacterHealthController))]
public class CharacterAIController : MonoBehaviour
{
    private CharacterActionController actionController;
    private NavMeshAgent nav;
    private CharacterHealthController healthController;
    private CharacterAnimatorController animatorController;

    public void Start()
    {
        actionController = GetComponent<CharacterActionController>();
        nav = GetComponent<NavMeshAgent>();
        healthController = GetComponent<CharacterHealthController>();
        animatorController = GetComponent<CharacterAnimatorController>();
    }

    public void Move(Vector3 position, bool isRunning)
    {
        nav.destination = position;
        animatorController.SetRunning(isRunning? 1 : 0.4f);
    }
}
