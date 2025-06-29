using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEngine.UI;

public class Sophia : Charcter
{
    [Header("Retrieve Settings")]
    [SerializeField] private Transform retrieveArea; // Assign this in the inspector

    public bool freezeRotation = false;
    private float originalAngularSpeed;
    private bool originalUpdateRotation;
    [SerializeField] private Button reviveButton;
    private float gameOverTimer = 0f;
    private bool isGameOverActive = false;


    private Vector3 originalCameraPosition; // Add this field to store the original camera position

    [Header("Camera Settings")]
    [SerializeField] private float cameraMoveDuration = 0.2f;
    [SerializeField] private float cameraMoveDistance = 0.33f;
    private Vector3 cameraMoveVelocity;
    private Vector3 cameraTargetPosition;
    private bool isCameraMoving;
    private float regenTimer = 0f;
    public GameObject gameOver;
    public int health;
    [Header("Health Bar")]
    [SerializeField] private Slider healthSlider; // Reference to UI Slider
    [SerializeField] private Image fillImage;     // Reference to fill area image
    //[SerializeField] private Gradient healthGradient;

    public GameObject sword; // Sword GameObject to activate
    public Rig weaponRig;    // Rig component for IK
    public Transform weaponTransform; // Transform for sword position

    public bool IsMoving { get; private set; }
    private Vector3 _initialMoveDirection;

    public Transform cameraTransform;
    public float XInput { get; private set; }
    public float ZInput { get; private set; }
    public Vector3 lastPosition;
    public bool canMove = true;
    public RectTransform cursorDot; // Reference to the UI dot, set in the inspector
    private bool restrictXMovement;
    private float lockedXPosition;
    [Header("Attack Settings")]
    [SerializeField] private Transform attackCheck;
    [SerializeField] private float attackCheckRadius = 1.5f;
    [SerializeField] private LayerMask enemyLayer;
    [SerializeField] private int attackDamage = 2;


    #region States
    public StateMachine stateMachine { get; private set; }
    public IdleState idleState { get; private set; }
    public BreathingIdleState breathingIdleState { get; private set; }
    public LookAroundIdleState lookAroundIdleState { get; private set; }
    public WalkState walkState { get; private set; }
    public RunState runState { get; private set; }
    public DrawSwordState DrawSwordState { get; private set; }
    public AttackState attackState { get; private set; }
    public Attack2State attack2State { get; private set; }
    public Attack3State attack3State { get; private set; }

    public FightIdleState FightIdle { get; private set; }
    #endregion
    public bool hasDrawTheSwrod;
    void Awake()
    {
        SetupStates();
        // InitializeCursor();
    }

    void SetupStates()
    {
        stateMachine = new StateMachine();
        idleState = new IdleState(this, stateMachine, "Idle");
        //breathingIdleState = new BreathingIdleState(this, stateMachine, "Idle II");
        //lookAroundIdleState = new LookAroundIdleState(this, stateMachine, "LookAroundIdle");
        walkState = new WalkState(this, stateMachine, "Walk");
        runState = new RunState(this, stateMachine, "Run");
        DrawSwordState = new DrawSwordState(this, stateMachine, "DrawSword");
        attackState = new AttackState(this, stateMachine, "attack");
        attack2State = new Attack2State(this, stateMachine, "attack2");
        attack3State = new Attack3State(this, stateMachine, "attack3");
        FightIdle = new FightIdleState(this, stateMachine, "IdleII");
    }

    protected override void Start()
    {
        base.Start();
        agent.updatePosition = true; // Ensure agent controls position
        agent.updateRotation = false;
        if (reviveButton != null)
        {
            reviveButton.onClick.AddListener(RevivePlayer);
        }
        originalAngularSpeed = agent.angularSpeed;
        originalUpdateRotation = agent.updateRotation;
        if (cameraTransform != null)
        {
            originalCameraPosition = cameraTransform.localPosition;
        }
        if (healthSlider != null)
        {
            healthSlider.maxValue = 20;
            healthSlider.value = health;
            UpdateHealthBarColor();
        }
        if (stateMachine != null && idleState != null)
        {
            stateMachine.Instatiate(idleState);
        }
    }
    void UpdateHealthBarColor()
    {
        if (fillImage != null && healthSlider != null)
        {
            // Use slider's current value instead of health variable
            float healthPercent = healthSlider.value / healthSlider.maxValue;
            //fillImage.color = healthGradient.Evaluate(healthPercent);

            // Debug to verify values
            Debug.Log($"Health: {healthSlider.value}/{healthSlider.maxValue} " +
                      $"Percent: {healthPercent} " +
                      $"Color: {fillImage.color}");
        }

    }

    void Update()
    {
        stateMachine?.currentState?.Update();
        IsMoving = stateMachine.currentState is RunState || stateMachine.currentState is WalkState;
        if (health < 30) // Only regen if not at max HP
        {
            regenTimer += Time.deltaTime;

            if (regenTimer >= 60f)
            {
                regenTimer = 0f;
                int newHealth = Mathf.Min(health + 5, 20); // Cap at max HP
                //SetHealth(newHealth);
            }
        }
        else
        {
            regenTimer = 0f; // Reset timer when at max HP
        }
        if (isGameOverActive)
        {
            gameOverTimer += Time.unscaledDeltaTime;

            if (gameOverTimer >= 5f)
            {
                RevivePlayer();
                //isGameOverActive = false;
            }
        }


    }
    public void SetHealth(int newHealth)
    {
        health = Mathf.Clamp(newHealth, 0, 20);

        if (healthSlider != null)
        {
            healthSlider.value = health;
            UpdateHealthBarColor();
        }

        if (health <= 0) Die();
    }


    /*#region Physics States

    void RotatePlayer()
    {
        Vector3 inputDirection = new Vector3(XInput, 0f, ZInput).normalized;

        if (inputDirection.magnitude >= 0.1f)
        {
            // Always update movement direction based on current camera
            UpdateMoveDirection(inputDirection);

            // Calculate target rotation based on movement direction
            float targetAngle = Mathf.Atan2(MoveDirection.x, MoveDirection.z) * Mathf.Rad2Deg;
            float smoothedAngle = Mathf.SmoothDampAngle(
                transform.eulerAngles.y,
                targetAngle,
                ref rotationSpeed,
                0.1f
            );

            // Apply rotation
            transform.rotation = Quaternion.Euler(0f, smoothedAngle, 0f);
        }
        else
        {
            MoveDirection = Vector3.zero;
            //_initialMoveDirection = Vector3.zero;
        }
    }

    void UpdateMoveDirection(Vector3 inputDirection)
    {
        Vector3 cameraForward = Vector3.ProjectOnPlane(cameraTransform.forward, Vector3.up).normalized;
        Vector3 cameraRight = Vector3.ProjectOnPlane(cameraTransform.right, Vector3.up).normalized;
        MoveDirection = (inputDirection.x * cameraRight + inputDirection.z * cameraForward).normalized;
    }
    #endregion*/

    public void FreezePlayerPosition(bool freezeRot = true)
    {
        stateMachine.ChangeState(idleState);

        freezeRotation = freezeRot;

        if (freezeRot)
        {
            agent.angularSpeed = 0;
            agent.updateRotation = false;
        }
        if (rb != null)
        {
            rb.constraints = freezeRot ?
                RigidbodyConstraints.FreezePosition | RigidbodyConstraints.FreezeRotation :
                RigidbodyConstraints.FreezeRotation;
        }

        lastPosition = transform.position;
        agent.isStopped = true;
        canMove = false;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        if (cursorDot != null)
        {
            cursorDot.gameObject.SetActive(true);
        }
    }

    public void UnfreezePlayerPosition()
    {       
        stateMachine.ChangeState(idleState);
        freezeRotation = false;
        agent.angularSpeed = originalAngularSpeed;
        agent.updateRotation = originalUpdateRotation;
        if (rb != null)
        {
            rb.constraints = RigidbodyConstraints.FreezeRotation;
        }

        canMove = true;
        agent.isStopped = false;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        if (cursorDot != null)
        {
            cursorDot.gameObject.SetActive(false);
        }
    }
    public void RestrictXMovement(bool restrict)
    {
        restrictXMovement = restrict;
        if (restrict)
        {
            lockedXPosition = transform.position.x;
            agent.ResetPath();
        }
    }
    public void AnimationTrigger() => stateMachine.currentState.AnimationFinishTrigger();

    void LateUpdate()
    {
        HandleCameraMovement();
        if (freezeRotation)
        {
            // Freeze all rotation
            transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0);
            rb.angularVelocity = Vector3.zero; // If using rigidbody
        }

        if (stateMachine.currentState is AttackState ||
            stateMachine.currentState is Attack2State ||
            stateMachine.currentState is Attack3State)
        {
            // Force position sync during attacks
            transform.position = animator.rootPosition;
            transform.rotation = animator.rootRotation;
        }
        if (restrictXMovement)
        {
            agent.velocity = Vector3.zero;
            agent.isStopped = true;

            Vector3 pos = transform.position;
            pos.x = lockedXPosition;
            transform.position = pos;
        }
        else
        {
            agent.isStopped = !canMove; // Resume agent movement if canMove allows
        }
    }
    private void HandleCameraMovement()
    {
        if (isCameraMoving)
        {
            cameraTransform.localPosition = Vector3.SmoothDamp(
                cameraTransform.localPosition,
                cameraTargetPosition,
                ref cameraMoveVelocity,
                cameraMoveDuration
            );

            if (Vector3.Distance(cameraTransform.localPosition, cameraTargetPosition) < 0.001f)
            {
                isCameraMoving = false;
            }
        }
    }

    public void MoveCameraForward()
    {
        cameraTargetPosition = cameraTransform.localPosition + Vector3.forward * cameraMoveDistance;
        isCameraMoving = true;
        cameraMoveVelocity = Vector3.zero;
    }

    public void MoveCameraBack()
    {
        cameraTargetPosition = originalCameraPosition;
        isCameraMoving = true;
        cameraMoveVelocity = Vector3.zero;
    }

    public State GetRandomAttackState()
    {
        int randomAttack = Random.Range(0, 2);
        switch (randomAttack)
        {
            //case 0: return attackState;
            case 0: return attack2State;
            case 1: return attack3State;
            default: return attackState;
        }
    }
    public void PerformAttack()
    {
        Collider[] hits = Physics.OverlapSphere(
            attackCheck.position,
            attackCheckRadius,
            enemyLayer
        );

        foreach (var hit in hits)
        {
            if (hit.CompareTag("Enemy"))
            {
                Orgre enemy = hit.GetComponent<Orgre>();
                if (enemy != null)
                {
                    enemy.TakeDamage(attackDamage);
                }
            }
        }
    }
    public void TakeDamage(int damage)
    {
        health -= damage;
        health = Mathf.Max(0, health); // Ensure health doesn't go below 0

        if (healthSlider != null)
        {
            healthSlider.value = health;
            UpdateHealthBarColor();
        }

        if (health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        if (isGameOverActive) return;
        gameOver.SetActive(true);
        Time.timeScale = 0f;

        // Enable cursor
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        if (cursorDot != null)
        {
            cursorDot.gameObject.SetActive(true);
        }
        isGameOverActive = true;
        gameOverTimer = 0f;

    }
    private void RevivePlayer()
    {
        isGameOverActive = false;
        gameOverTimer = 0f;
        DaySystem daySystem = FindFirstObjectByType<DaySystem>();
        if (daySystem != null)
        {
            daySystem.currentTimeOfDay += 400f / daySystem.dayDurationInSeconds;
            daySystem.currentTimeOfDay %= 1; // Wrap around
        }

        // Deduct coins
        Finance.Instance?.SpendMoney(20);

        // Reset time scale
        Time.timeScale = 1f;

        // Hide game over UI
        gameOver.SetActive(false);

        // Reset player state
        SetHealth(20);
        stateMachine.ChangeState(idleState);
        UnfreezePlayerPosition();
        if (retrieveArea != null)
        {
            agent.Warp(retrieveArea.position);
        }

    }
}