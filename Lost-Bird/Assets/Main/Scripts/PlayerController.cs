using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    #region FIELDS

    [SerializeField] private float playerSpeed = 2.0f;
    [SerializeField] private float pushingSpeed = 3.0f;
    [SerializeField] private float gravityValue = -9.81f;
    //[SerializeField] private Animator animator = null;

    private Player playerInput = null;
    private CharacterController controller = null;
    private Vector3 playerVelocity;
    private bool pushing = false;
    private bool changedDirection = false;
    private GameObject objectToPush = null;
    private Vector3 endPosition;
    private Box movingBox = null;

    #endregion

    #region PROPERTIES

    public Vector2 MovementInput { get => playerInput.PlayerActions.Move.ReadValue<Vector2>(); }

    #endregion

    #region BEHAVIORS   

    private void Awake()
    {
        playerInput = new Player();
        controller = GetComponent<CharacterController>();
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

    private void Update()
    {
        if (pushing)
        {
            float step = pushingSpeed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, endPosition, step);

            if (Vector3.Distance(transform.position, endPosition) < 0.001f)
            {
                pushing = false;
                transform.position = endPosition;
                movingBox.transform.parent.SetParent(null);
                if (!changedDirection && movingBox.CanPush(MovementInput))
                    PushObject(movingBox);
            }

            return;
        }

        Vector3 move = new Vector3(MovementInput.x, 0, MovementInput.y);
        controller.Move(move * Time.deltaTime * playerSpeed);

        if (move != Vector3.zero)
            gameObject.transform.forward = move;

        playerVelocity.y += gravityValue * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);
    }

    public void PushObject(Box box)
    {
        changedDirection = false;
        endPosition = box.transform.parent.transform.position;
        transform.position = box.PushOrigin;
        box.transform.parent.SetParent(transform);
        movingBox = box;
        pushing = true;
    }

    #endregion
}
