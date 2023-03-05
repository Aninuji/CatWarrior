using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(CharacterActionController), typeof(CharacterHealthController))]
public class CharacterFaceController : MonoBehaviour
{
    [SerializeField]
    private string[] animatorParameters;
    [SerializeField]
    private Animator _facesAnim;
    private Dictionary<string, int> _animatorHashes = new Dictionary<string, int>();

    private CharacterActionController actionController;
    private CharacterHealthController healthController;
    
    public void Start()
    {

        //Converting all animator variable to hashes and storing them on a dictionary to optimize.
        for (int i = 0; i < animatorParameters.Length; i++)
        {
            _animatorHashes.Add(animatorParameters[i], Animator.StringToHash(animatorParameters[i]));
        }


        actionController = GetComponent<CharacterActionController>();
        healthController = GetComponent<CharacterHealthController>();

        actionController.OnAttack += () => { ChangeFace("Angry"); };
        actionController.OnBlock += (bool isBlocking) => { 
            if(isBlocking) ChangeFace("Angry"); 
            else ChangeFace("Serious");
        };
        actionController.OnJump += () => { ChangeFace("Surprised"); };
        //actionController.OnWalk += () => { ChangeFace(IdleFace); };
        //actionController.OnRun += () => { ChangeFace(SeriousFace); };
        actionController.OnChangeCombatMode += (bool isCombating) => {
            if (isCombating) ChangeFace("Serious");
        };
        healthController.OnDamage += () => { ChangeFace("Hurt"); };
        healthController.OnDeath += () => { ChangeFace("Death"); };
        healthController.OnHeal += () => { ChangeFace("Happy"); };
    }

    public void ChangeFace(string newFace)
    {
        _facesAnim.SetTrigger(_animatorHashes[newFace]);
    }
}
