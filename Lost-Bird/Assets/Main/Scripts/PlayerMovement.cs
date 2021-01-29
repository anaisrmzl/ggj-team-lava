using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    #region FIELDS

    private const float MinMovingDistance = 0.001f;

    [Header("PLAYER MOVEMENT")]
    [SerializeField] private float playerSpeed = 2.0f;
    //[SerializeField] private Animator animator = null;

    [Header("PUSH")]
    [SerializeField] private float pushingSpeed = 3.0f;

    [Header("JUMPS")]
    [SerializeField] private float shortLength = 2.0f;
    [SerializeField] private float longLength = 3.0f;
    [SerializeField] private float jumpSpeed = 7.0f;

    private Player playerInput = null;
    private new Rigidbody rigidbody = null;
    private Vector3 playerVelocity;

    private bool pushing = false;
    private bool changedDirection = false;
    private GameObject objectToPush = null;
    private Vector3 endPosition;
    private Box movingBox = null;

    private Vector3 elevationPosition;
    private Vector3 travelingPosition;
    private float travelLength = default(float);
    private bool elevating = false;
    private bool traveling = false;

    #endregion

    #region PROPERTIES

    public Vector2 MovementInput { get => playerInput.PlayerActions.Move.ReadValue<Vector2>(); }

    #endregion

    #region BEHAVIORS   

    private void Awake()
    {
        playerInput = new Player();
        rigidbody = GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
        playerInput.Enable();
        playerInput.PlayerActions.Move.started += action => StartedAction();
        playerInput.PlayerActions.Move.performed += action => PerformedAction();
        playerInput.PlayerActions.Move.canceled += action => CanceledAction();
    }

    private void OnDisable()
    {
        playerInput.Disable();
        playerInput.PlayerActions.Move.started -= action => StartedAction();
        playerInput.PlayerActions.Move.canceled -= action => CanceledAction();
        playerInput.PlayerActions.Move.performed -= action => PerformedAction();
    }

    private void FixedUpdate()
    {
        Vector3 move = new Vector3(MovementInput.x, 0, MovementInput.y);
        rigidbody.MovePosition(rigidbody.position + move * playerSpeed * Time.fixedDeltaTime);
    }

    private void Update()
    {
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

        if (playerInput.PlayerActions.ShortJump.triggered)//Change to autoTrigger
            SetElevation(transform.position + new Vector3(0.0f, 0.5f, 0.0f), shortLength);


        if (playerInput.PlayerActions.LongJump.triggered)//Change to autoTrigger
            SetElevation(transform.position + new Vector3(0.0f, 1.0f, 0.0f), longLength);

        Vector3 move = new Vector3(MovementInput.x, 0, MovementInput.y);
        if (move != Vector3.zero)
            gameObject.transform.forward = move;
    }

    private void StartedAction()
    {
        // animator.SetTrigger("walk");
    }

    private void PerformedAction()
    {
        changedDirection = true;
    }

    private void CanceledAction()
    {
        changedDirection = true;
        //animator.SetTrigger("hide");
    }

    private void SetElevation(Vector3 elevation, float traveling)
    {
        elevationPosition = elevation;
        travelLength = traveling;
        elevating = true;
        rigidbody.useGravity = false;
    }

    private void Elevate()
    {
        float step = jumpSpeed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, elevationPosition, step);
        if (Vector3.Distance(transform.position, elevationPosition) < MinMovingDistance)
        {
            elevating = false;
            transform.position = elevationPosition;
            travelingPosition = transform.position + transform.forward * travelLength;
            traveling = true;
        }
    }

    private void Travel()
    {
        float step = jumpSpeed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, travelingPosition, step);
        if (Vector3.Distance(transform.position, travelingPosition) < MinMovingDistance)
        {
            traveling = false;
            transform.position = travelingPosition;
            rigidbody.useGravity = true;
        }
    }

    private void Push()
    {
        float step = pushingSpeed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, endPosition, step);
        if (Vector3.Distance(transform.position, endPosition) < MinMovingDistance)
        {
            pushing = false;
            transform.position = endPosition;
            movingBox.transform.parent.SetParent(null);
            if (!changedDirection && movingBox.CanPush(MovementInput))
                PushObject(movingBox);
        }
    }

    public void PushObject(Box box)
    {
        changedDirection = false;
        Vector3 boxParentPosition = box.transform.parent.transform.position;
        endPosition = new Vector3(boxParentPosition.x, transform.position.y, boxParentPosition.z);
        transform.position = new Vector3(box.PushOrigin.x, transform.position.y, box.PushOrigin.z);
        box.transform.parent.SetParent(transform);
        movingBox = box;
        pushing = true;
    }

    #endregion
}
