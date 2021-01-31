using UnityEngine;

using Zenject;
using Utilities.Sound;

public class PlayerMovement : MonoBehaviour
{
    #region FIELDS

    private const float MinMovingDistance = 0.001f;

    [Inject] private SoundManager soundManager;

    [Header("AUDIOS")]
    [SerializeField] private AudioClip walkSound = null;
    [SerializeField] private AudioClip jumpWithBirdSound = null;
    [SerializeField] private AudioClip jumpAloneSound = null;
    [SerializeField] private AudioClip pushSound = null;
    [SerializeField] private AudioClip respawnSound = null;

    [Header("BIRD")]
    [SerializeField] private GameObject bird = null;
    [SerializeField] private Collector collector = null;
    [SerializeField] private Animator birdAnimator = null;

    [Header("PLAYER MOVEMENT")]
    [SerializeField] private float playerSpeed = 2.0f;
    [SerializeField] private Animator animator = null;

    [Header("PUSH")]
    [SerializeField] private float pushingSpeed = 3.0f;
    [SerializeField] private Pusher pusher = null;

    [Header("JUMPS")]
    [SerializeField] private float shortLength = 2.0f;
    [SerializeField] private float longLength = 3.0f;
    [SerializeField] private float shortJumpSpeed = 7.0f;
    [SerializeField] private float longJumpSpeed = 7.0f;
    [SerializeField] private float maxTravelTime = 2.0f;

    private Player playerInput = null;
    private new Rigidbody rigidbody = null;
    private Vector3 playerVelocity;

    private bool pushing = false;
    private bool startedPushing = false;
    private GameObject objectToPush = null;
    private Vector3 endPosition;
    private Box movingBox = null;

    private Vector3 elevationPosition;
    private Vector3 travelingPosition;
    private float travelLength = default(float);
    private float jumpSpeed = default(float);
    private bool elevating = false;
    private bool traveling = false;
    private float travelTimer = 0.0f;
    private Vector3 currentEdgeCenter;

    #endregion

    #region PROPERTIES

    public Vector2 MovementInput { get => playerInput.PlayerActions.Move.ReadValue<Vector2>(); }
    public bool HasBird { get; private set; }
    public bool CanMove { get => !elevating && !pushing && !traveling && !startedPushing; }
    public bool IsJumping { get => elevating || traveling; }
    public bool CanPush { get => !startedPushing; }

    #endregion

    #region BEHAVIORS   

    private void Awake()
    {
        playerInput = new Player();
        rigidbody = GetComponent<Rigidbody>();
        LooseBird();
        soundManager.PlayEffectOneShot(respawnSound);
    }

    private void OnEnable()
    {
        playerInput.Enable();
        playerInput.PlayerActions.Move.started += action => StartedAction();
        playerInput.PlayerActions.Move.canceled += action => CanceledAction();
    }

    private void OnDisable()
    {
        playerInput.Disable();
        playerInput.PlayerActions.Move.started += action => StartedAction();
        playerInput.PlayerActions.Move.canceled -= action => CanceledAction();
    }

    private void FixedUpdate()
    {
        if (!CanMove)
            return;

        Vector3 move = new Vector3(MovementInput.x, 0, MovementInput.y);
        rigidbody.MovePosition(rigidbody.position + move * playerSpeed * Time.fixedDeltaTime);
    }

    private void Update()
    {
        animator.SetFloat("speed", startedPushing ? 0.0f : Mathf.Abs(MovementInput.x) + Mathf.Abs(MovementInput.y));
        birdAnimator.SetFloat("speed", startedPushing ? 0.0f : Mathf.Abs(MovementInput.x) + Mathf.Abs(MovementInput.y));

        if (elevating || traveling)
        {
            travelTimer += Time.deltaTime;
            if (travelTimer >= maxTravelTime)
            {
                FinishElevate(true);
                FinishTravel(true);
                return;
            }
        }

        if (pushing)
        {
            Push();
            return;
        }

        if (elevating)
        {
            Elevate();
            return;
        }

        if (traveling)
        {
            Travel();
            return;
        }

        Vector3 move = new Vector3(MovementInput.x, 0, MovementInput.y);
        if (move != Vector3.zero)
            gameObject.transform.forward = move;
    }

    public void Jump(Edge edge)
    {
        currentEdgeCenter = edge.TileCenter;
        gameObject.transform.forward = edge.Direction;
        SetElevation(transform.position + new Vector3(0.0f, HasBird ? 1.0f : 0.5f, 0.0f), HasBird ? longLength : shortLength, HasBird ? longJumpSpeed : shortJumpSpeed);
    }

    private void CanceledAction()
    {
        startedPushing = false;
        if (CanMove)
            soundManager.StopVoice();
    }

    private void StartedAction()
    {
        if (movingBox != null && movingBox == pusher.CollidingBox)
            pusher.TryPush(movingBox, transform.forward);

        if (CanMove)
            soundManager.PlayVoice(walkSound, true);
    }

    private void SetElevation(Vector3 elevation, float traveling, float duration)
    {
        jumpSpeed = duration;
        elevationPosition = elevation;
        travelLength = traveling;
        elevating = true;
        travelTimer = 0.0f;
        animator.SetBool("jump", elevating);
        birdAnimator.SetBool("jump", elevating);
        rigidbody.useGravity = false;
        soundManager.PlayVoice(HasBird ? jumpWithBirdSound : jumpAloneSound);
    }

    private void Elevate()
    {
        float step = jumpSpeed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, elevationPosition, step);
        if (Vector3.Distance(transform.position, elevationPosition) < MinMovingDistance)
            FinishElevate();
    }

    private void FinishElevate(bool canceled = false)
    {
        elevating = false;
        if (!canceled)
            transform.position = elevationPosition;

        Vector3 destination = new Vector3(currentEdgeCenter.x, transform.position.y, currentEdgeCenter.z);
        travelingPosition = destination + transform.forward * travelLength;
        traveling = true;
    }

    private void Travel()
    {
        float step = jumpSpeed * Time.deltaTime;
        Vector3 lastPosition = transform.position;
        transform.position = Vector3.MoveTowards(transform.position, travelingPosition, step);
        if (Vector3.Distance(transform.position, travelingPosition) < MinMovingDistance)
            FinishTravel();
    }

    private void FinishTravel(bool canceled = false)
    {
        traveling = false;
        if (!canceled)
            transform.position = travelingPosition;

        rigidbody.useGravity = true;
        travelTimer = 0.0f;
        animator.SetBool("jump", elevating);
        birdAnimator.SetBool("jump", elevating);
    }

    private void Push()
    {
        float step = pushingSpeed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, endPosition, step);
        Debug.Log((Vector3.Distance(transform.position, endPosition)));
        if (Vector3.Distance(transform.position, endPosition) < MinMovingDistance)
        {
            pushing = false;
            transform.position = endPosition;
            movingBox.transform.parent.SetParent(null);
            animator.SetBool("push", pushing);
        }
    }

    private void PushObject(Box box)
    {
        Vector3 boxParentPosition = box.transform.parent.transform.position;
        endPosition = new Vector3(boxParentPosition.x, transform.position.y, boxParentPosition.z);
        transform.position = new Vector3(box.PushOrigin.x, transform.position.y, box.PushOrigin.z);
        box.transform.parent.SetParent(transform);
        movingBox = box;
        pushing = true;
    }

    public void StartPushing(Box box)
    {
        startedPushing = true;
        soundManager.PlayVoice(pushSound);
        PushObject(box);
        animator.SetBool("push", pushing);
    }

    public void LooseBird()
    {
        HasBird = false;
        UpdateBird();
    }

    public bool FreeBird()
    {
        if (!collector.HasKey)
            return false;

        HasBird = true;
        collector.UseKey();
        UpdateBird();
        return true;
    }

    private void UpdateBird()
    {
        bird.SetActive(HasBird);
        animator.SetBool("bird", HasBird);
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.GetComponent<BoxMain>() == null)
            return;

        BoxMain box = other.gameObject.GetComponent<BoxMain>();
        box.FreezeConstraints(true);
    }

    private void OnCollisionExit(Collision other)
    {
        if (other.gameObject.GetComponent<BoxMain>() == null)
            return;

        BoxMain box = other.gameObject.GetComponent<BoxMain>();
        box.FreezeConstraints(false);
    }

    #endregion
}
