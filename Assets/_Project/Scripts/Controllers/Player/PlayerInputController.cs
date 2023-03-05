using UnityEngine.InputSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(CharacterActionController))]
public class PlayerInputController : MonoBehaviour
{
    #region Public Members
    /// <summary>
    /// The larger the value, it will took more time to react the maximum input value but the value will transition more smoothly.
    /// </summary>
    public float sensitivity = 1;
    /// <summary>
    /// Used when input is null, ideally it should be less than <see cref="sensitivity"/>.
    /// </summary>
    public float stopSensitivity = 0.5f;
    /// <summary>
    /// Determinates how fast the smooth will be. 
    /// </summary>
    public float smoothRefresher = 0.01f;

    /// <summary>
    /// Because we can't change Virtual's camera directly, we have to change the follow target's rotation in order to recreate a FreeLookCamera feel. 
    /// </summary>
    [SerializeField]
    private FollowTarget followTarget;

    /// <summary>
    /// How fast the <see cref="followTransform"/> object will rotate.
    /// </summary>
    public float followTransformRotationSpeed = 0.01f;


    #endregion
    #region Private Members
    private CharacterActionController controller;
    private NekoZombieControls controls;
    private Vector2 playerInput;
    private Vector2 smoothedPlayerInput;
    private Vector2 playerLook;
    private bool isRunning;

    private Coroutine inputSmoothCoroutine;
    #endregion
    void Awake()
    {
        controller = GetComponent<CharacterActionController>();

        controls = new NekoZombieControls();

        controls.Player.Move.performed += ctx =>
        {
            playerInput = ctx.ReadValue<Vector2>();

            if(inputSmoothCoroutine != null) StopCoroutine(inputSmoothCoroutine);
            inputSmoothCoroutine = StartCoroutine(inputSmoother(playerInput, sensitivity));

        };
        controls.Player.Move.canceled += ctx =>
        {
            playerInput = Vector2.zero;

            if (inputSmoothCoroutine != null) StopCoroutine(inputSmoothCoroutine);
            inputSmoothCoroutine = StartCoroutine(inputSmoother(playerInput, stopSensitivity));
        };


        controls.Player.Run.performed += ctx => isRunning = true;
        controls.Player.Run.canceled += ctx => isRunning = false;

        controls.Player.Look.performed += ctx =>
        {
            playerLook = ctx.ReadValue<Vector2>();
        };

        controls.Player.Look.canceled += ctx =>
        {
            playerLook = Vector2.zero;
        };


        ///Trigger Actions
        controls.Player.Jump.performed += ctx => controller.Jump();
        controls.Player.Attack.performed += ctx => controller.Attack();
        controls.Player.Block.performed += ctx => controller.Block();
        controls.Player.Block.canceled += ctx => controller.Unblock();

    }
    private void FixedUpdate()
    {
        controller.Movement(smoothedPlayerInput, isRunning);
        //followTarget.RotateLook(playerLook, followTransformRotationSpeed);
    }
    private void OnEnable()
    {
        controls.Player.Enable();
    }
    private void OnDisable()
    {
        controls.Player.Disable();
    }


    /// <summary>
    /// Smoothie Time! 🥤🥤🥤
    /// </summary>
    /// <param name="waitTime"></param>
    /// <returns></returns>
    private IEnumerator inputSmoother(Vector2 targetSmoothInput, float sensitivity)
    {
        //While the smoothed value is not similar as the target, continue to smoothing!~~
        while (!Utility.FastApproximately(smoothedPlayerInput.sqrMagnitude, targetSmoothInput.sqrMagnitude, 0.1f))
        {
            Vector2.SmoothDamp(Vector2.zero, targetSmoothInput, ref smoothedPlayerInput, sensitivity);
            yield return new WaitForSeconds(smoothRefresher);
        }

        smoothedPlayerInput = targetSmoothInput;
    }
}
