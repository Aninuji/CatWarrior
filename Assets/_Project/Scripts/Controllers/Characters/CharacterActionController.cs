using UnityEngine;
using Cinemachine;

using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(CharacterAnimatorController))]
public class CharacterActionController : MonoBehaviour
{
    #region Public Variables
    public bool checkGravity  = true;
    #endregion
    #region Serialized Members
    [SerializeField]
    private float walkingSpeed = 2.0f;
    [SerializeField]
    private float runningSpeed = 5.0f;
    [SerializeField]
    private float jumpHeight = 5.0f;
    [SerializeField]
    private float turnSmoothTime = 0.5f;
    /// <summary>
    /// Max amount that <see cref="jumpCounter"/> can react. The larger the value, the player will took more time between jumps.
    /// </summary>
    [SerializeField]
    private float jumpTimer = 1.0f;
    /// <summary>
    /// Amount of time that passes without any combat action to return to a non-combatibe idle state.
    /// </summary>
    [SerializeField]
    private float combatModeDuration = 5.0f;
    /// <summary>
    /// Whenever you perform an attack, how much time the character will stay locked in his position befor it can move again.
    /// </summary>
    [SerializeField]
    private float attackLockDuration = 0.5f;
    #endregion

    #region Dependencies
    private CharacterController controller;
    private CharacterAnimatorController animatorController;

    private Camera _camera;
    #endregion

    #region Private Variables

    private Vector3 playerVelocity;
    public bool groundedPlayer;
    private bool isAttacking;
    private bool isBlocking;

    private float jumpCounter;
    private float turnSmoothVelocity = 1.0f;
    private const float gravityValue = -9.81f;
    private Coroutine combatModeTimer;
    private Coroutine attackLockTimer;

    #endregion


    #region Events]
    public delegate void CharacterActionEventTrigger();
    public delegate void CharacterActionEventBool(bool value);

    public event CharacterActionEventTrigger OnAttackEnd;
    public event CharacterActionEventTrigger OnAttack;
    public event CharacterActionEventTrigger OnJump;
    public event CharacterActionEventTrigger OnWalk;
    public event CharacterActionEventTrigger OnRun;

    public event CharacterActionEventBool OnBlock;
    public event CharacterActionEventBool OnChangeCombatMode;

    #endregion


    #region Unity Methods

    private void Start()
    {
        controller = GetComponent<CharacterController>();
        animatorController = GetComponent<CharacterAnimatorController>();
        _camera = GameManager.Instance._mainCamera;
        Utility.LockMouse();
    }

    void FixedUpdate()
    {
       if(checkGravity) GroundCheck();
    }

    #endregion

    #region Private Class Methods
    private void GroundCheck()
    {
        // Gravity BS
        playerVelocity.y += gravityValue * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);

        groundedPlayer = controller.isGrounded;
        if (groundedPlayer && playerVelocity.y < 0)
        {
            playerVelocity.y = 0;
            animatorController.SetGrounded(true);
            if (jumpCounter < jumpTimer) jumpCounter += Time.deltaTime;
        }
    }

    #endregion

    #region Public Class Methods
    public void Movement(Vector2 direction, bool isRunning)
    {
        if(direction.magnitude >= 0.1f)
        {
            //World coordinates rotation
            float targetAngle = Mathf.Atan2(direction.x, direction.y) * Mathf.Rad2Deg + _camera.transform.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.rotation.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            Quaternion targetRotation = Quaternion.Euler(0, angle, 0);
            transform.rotation = targetRotation;

            //While attacking or blocking, player cannot move but it will perform rotation
            if (!isAttacking && !isBlocking)
            {
                Vector3 moveDir = Quaternion.Euler(0, targetAngle, 0) * Vector3.forward;
                float finalSpeed = isRunning ? runningSpeed : walkingSpeed;
                controller.Move(moveDir.normalized * Time.deltaTime * finalSpeed);
            }
            //Rotate around 
            //Vector3 playerRotation = Vector3.up * playerInput.x * rotationSpeed;
            //transform.Rotate(playerRotation);
        }
        float TargetDirection = Utility.Normalize(direction.x, -1, 1);
        animatorController.SetDirection(TargetDirection);

        if (isRunning) {
            if (OnRun != null) OnRun();
        }
        else { 
            if (OnWalk != null) OnWalk(); 
        }

        float runValue = Mathf.Clamp(Mathf.Abs(direction.magnitude), 0, isRunning ? 1 : 0.4f);
        animatorController.SetRunning(runValue);
    }

    public void Jump()
    {
        // Changes the height position of the player..
        if (groundedPlayer && jumpCounter > jumpTimer && !isAttacking)
        {
            playerVelocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravityValue);
            jumpCounter = 0;
            animatorController.TriggerJump();
            animatorController.SetGrounded(false);

            if (OnJump != null) OnJump();

        }
    }

    public void Attack()
    {
        if (!groundedPlayer) return;
        isAttacking = true;
        animatorController.SetCombatMode(true);
        animatorController.TriggerAttack();
        if (combatModeTimer != null) StopCoroutine(combatModeTimer);
        combatModeTimer = StartCoroutine(CombatModeTimer());
        if (attackLockTimer != null) StopCoroutine(attackLockTimer);
        attackLockTimer = StartCoroutine(AttackLock());
        if (OnAttack != null) OnAttack();
        if (OnChangeCombatMode != null) OnChangeCombatMode(true);
    }
    public void Block()
    {
        if (!groundedPlayer) return;
        isBlocking = true;
        animatorController.SetCombatMode(true);
        animatorController.SetBlocking(true);
        if (combatModeTimer != null) StopCoroutine(combatModeTimer);
        combatModeTimer = StartCoroutine(CombatModeTimer());
        if (OnBlock != null) OnBlock(true);
        if (OnChangeCombatMode != null) OnChangeCombatMode(true);
    }
    public void Unblock()
    {
        if (!groundedPlayer) return;
        isBlocking = false;
        animatorController.SetCombatMode(false);
        animatorController.SetBlocking(false);
        if (combatModeTimer != null) StopCoroutine(combatModeTimer);
        combatModeTimer = StartCoroutine(CombatModeTimer());
        if (OnBlock != null) OnBlock(false);
        if (OnChangeCombatMode != null) OnChangeCombatMode(true);
    }
    #endregion

    #region Coroutines
    private IEnumerator CombatModeTimer()
    {
        yield return new WaitForSeconds(combatModeDuration);
        animatorController.SetCombatMode(false);
        if (OnChangeCombatMode != null) OnChangeCombatMode(false);
    }
    private IEnumerator AttackLock()
    {
        yield return new WaitForSeconds(attackLockDuration);
        isAttacking = false;
        if (OnAttackEnd != null) OnAttackEnd();

    }
    #endregion
}
