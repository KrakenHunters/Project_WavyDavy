using System.Collections;
using UnityEngine;

[RequireComponent(typeof(InputManager), typeof(PlayerHitHandler))]
public class PlayerController : MonoBehaviour
{
    private InputManager inputManager;
    private PlayerTrickHandler trickManager;

    public GameEvent Event;
    public GameObject windAnimator;
    public GamePhase currentGamePhase { get; private set; }
    public Animator animator { get; set; }
    public BaseState currentState { get; private set; }
    public bool MovementTutorial { get; set; }
    public bool TrickUPTutorial { get; set; }

    public UIAnimator shorebreakAnimator { get; private set; }
    [SerializeField] private UIAnimator movementControlUI;
    [SerializeField] private GameObject trickUPTutorialUI;



    private static readonly int Crash = Animator.StringToHash("Crash");

    public float normalSpeed { get; set; }
    public float pumpSpeed { get; private set; }
    public float currentFlow { get; private set; }
    public bool canTrick { get; private set; }

    [SerializeField] private float pumpSpeedPhase2 = 4f;
    [SerializeField] private float pumpSpeedPhase3 = 7f;

    [SerializeField] private float normalSpeedPhase2 = 2f;
    [SerializeField] private float normalSpeedPhase3 = 3f;

    [SerializeField] private Transform _phase1StartPos;
    [SerializeField] private Transform _phase2StartPos;
    [SerializeField] private Transform _phase3StartPos;
    [SerializeField] private Transform _trickStartPos;


    [SerializeField] private float _paddleSpeed;
    [SerializeField] private float _phase2MaxHeight;
    [SerializeField] private float _phase3MaxHeight;
    [SerializeField] private float _maxInclineAngle = 20f; // Maximum angle for inclination
    [SerializeField] private float _maxInclineBoostAngle = 30f; // Maximum angle for inclination

    [SerializeField] private float _inclineSpeed = 45f; // Speed of inclination change

    public Transform phase1StartPos { get => _phase1StartPos; private set => _phase1StartPos = value; }
    public Transform phase2StartPos { get => _phase2StartPos; private set => _phase2StartPos = value; }
    public Transform phase3StartPos { get => _phase3StartPos; private set => _phase3StartPos = value; }
    public Transform trickStartPos { get => _trickStartPos; private set => _trickStartPos = value; }

    public UIAnimator MovementControlUI { get => movementControlUI; private set => movementControlUI = value; }
    public GameObject TrickUPTutorialUI { get => trickUPTutorialUI; private set => trickUPTutorialUI = value; }

    public float finalPaddleSpeed { get; private set; }
    public float phase2MaxHeight { get => _phase2MaxHeight; private set => _phase2MaxHeight = value; }
    public float phase3MaxHeight { get => _phase3MaxHeight; private set => _phase3MaxHeight = value; }
    public float maxInclineAngle { get => _maxInclineAngle; private set => _maxInclineAngle = value; }
    public float maxInclineBoostAngle { get => _maxInclineBoostAngle; private set => _maxInclineBoostAngle = value; }
    public float inclineSpeed { get => _inclineSpeed; private set => _inclineSpeed = value; }

    public AudioClip scpart1Clip;
    public AudioClip scpart2Clip;
    public AudioClip hitSand;
    public AudioClip speedPlayer;
    public AudioClip paddleClip;


    private void OnEnable()
    {
        Event.OnChangeGameState.AddListener(SetPhase);
        Event.OnHitObject.AddListener(HitObject);
        Event.OnFlowChange.AddListener(FlowChange);
        Event.OnTrickCelebration += CelebrationState;
        Event.OnFinishCelebration += HandleFinishCelebration;

        Event.OnGameEnd += DisableInput;

        Event.OnIsTrickPossible += (val) => canTrick = val;
    }

    private void OnDisable()
    {
        Event.OnChangeGameState.RemoveListener(SetPhase);
        Event.OnHitObject.RemoveListener(HitObject);
        Event.OnFlowChange.RemoveListener(FlowChange);
        Event.OnTrickCelebration -= CelebrationState;
        Event.OnFinishCelebration -= HandleFinishCelebration;
        Event.OnIsTrickPossible -= (val) => canTrick = val;
        Event.OnGameEnd -= DisableInput;

    }

    private void SetPhase(GamePhase gameState)
    {

        switch (gameState)
        {
            case GamePhase.Phase1:
                if (currentGamePhase == GamePhase.Phase2)
                    animator.Play(Crash);
                break;
            case GamePhase.Phase2:
                pumpSpeed = pumpSpeedPhase2;
                normalSpeed = normalSpeedPhase2;
                break;
            case GamePhase.Phase3:
                pumpSpeed = pumpSpeedPhase3;
                normalSpeed = normalSpeedPhase3;
                break;
        }
        currentGamePhase = gameState;

        currentState?.HandleTransition();

    }

    private void HitObject(float flowAmount)
    {
        currentState?.HitObject();
    }

    public void ChangeState(BaseState newState)
    {
        StartCoroutine(WaitFixedFrame(newState));
    }
    // Start is called before the first frame update
    private void Awake()
    {
        MovementTutorial = false;
        inputManager = GetComponent<InputManager>();
        trickManager = GetComponent<PlayerTrickHandler>();
        animator = GetComponent<Animator>();
        Event.InitializeTrickUI?.Invoke(trickManager);
        shorebreakAnimator = GetComponentInChildren<UIAnimator>();
        ChangeState(new PaddleState());
    }

    // Update is called once per frame
    void Update()
    {
        HandleMove(inputManager.Movement);

        currentState?.StateUpdate();
    }

    private void FlowChange(float flow)
    {
        currentFlow = flow;
        if (currentGamePhase == GamePhase.Phase1)
        {
            finalPaddleSpeed = _paddleSpeed - _paddleSpeed * (flow/2f);
        }
    }

    private void FixedUpdate() => currentState?.StateFixedUpdate();

    public void HandlePump()
    {
        currentState?.HandlePumping();
    }

    public void HandleStopPump()
    {
        currentState?.HandleStopPumping();
    }

    public void HandleMove(Vector2 dir)
    {
        currentState?.HandleMovement(dir);
    }

    public void HandlePaddleRight(bool isPaddling)
    {
        currentState?.HandlePaddlingRight(isPaddling);
    }

    public void HandlePaddleLeft(bool isPaddling)
    {
        currentState?.HandlePaddlingLeft(isPaddling);
    }


    public void HandleTrickInput(TrickCombo direction)
    {
        Event.OnPlayerInput?.Invoke(direction);
    }

    private void CelebrationState(PlayerTrickHandler trickHandler)
    {
        currentState?.HandleCelebration();
    }

    private void HandleFinishCelebration()
    {
        currentState?.FinishCelebration();
    }

    private void DisableInput()
    {
        inputManager.DisableAllInput();
    }
    private IEnumerator WaitFixedFrame(BaseState newState)
    {

        yield return new WaitForFixedUpdate();
        if (currentState != newState)
        {
            currentState?.ExitState();
            currentState = newState;
            currentState.player = this;
            currentState.trickManager = trickManager;
            currentState.inputManager = this.inputManager;
            currentState.EnterState();
        }

    }

}
