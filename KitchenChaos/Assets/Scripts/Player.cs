using UnityEngine;
using System;

public class Player : MonoBehaviour, IKitchenObjectParent
{

    public static Player Instance { get; private set; }

    public event EventHandler OnPickedUpSomething;

    public event EventHandler<OnSelectedCounterChangedEventArs> OnSelectedCounterChanged;
    public class OnSelectedCounterChangedEventArs: EventArgs   //One way to send info in the event. This approach good if want to send multiple things.
    {
        public BaseCounter selectedCounter;
    }

    [SerializeField] float moveSpeed = 7f;
    [SerializeField] InputManager inputManager;

    [SerializeField] LayerMask countersLayerMask;

    bool isWalking;

    Vector3 lastInteractDir;

    BaseCounter selectedCounter;

    public KitchenObject KitchenObject { get; private set; }

    [SerializeField] Transform kitchenObjectHoldPoint;

    private void Awake()                //Singleton, AS LONG AS GAME IS SINGLE PLAYER.
    {
        if (Instance != null)
            Debug.Log("More than one player?!");

        Instance = this;
    }

    private void Start()
    {
        inputManager.OnInteractPressed += InputManager_OnInteractPressed;        //Listen for interact button press.
        inputManager.OnInteractAlternatePressed += InputManager_OnInteractAlternatePressed;
    }

    private void InputManager_OnInteractAlternatePressed(object sender, EventArgs e)
    {
        if (!KitchenGameManager.Instance.IsGamePlaying())     //Can only interact when game is playing.
            return;

        if (selectedCounter != null)
            selectedCounter.InteractAlternate(this);
    }

    private void InputManager_OnInteractPressed(object sender, EventArgs e)
    {
        if (!KitchenGameManager.Instance.IsGamePlaying())     //Can only interact when game is playing.
            return;

        if (selectedCounter != null)
            selectedCounter.Interact(this);
    }

    private void Update()
    {
        HandleMovement();
        HandleInteractions();

    }

    public bool GetIsWalking() => isWalking;

    void HandleInteractions()
    {
        Vector2 inputVector = inputManager.GetMovementVector();

        Vector3 moveDir = new Vector3(inputVector.x, 0, inputVector.y);    //Needs to be vector 3. Input is vector 2.

        if (moveDir != Vector3.zero)       //If moving, update last interact dir. This is so if I stop moving but am facing the counter, it is still colliding.
            lastInteractDir = moveDir;

        float interactDistance = 2f;
        if (Physics.Raycast(transform.position, lastInteractDir, out RaycastHit raycastHit, interactDistance, countersLayerMask))    //The out raycastHit gives data about what it collides with, if it does. Out is the actual var, not a copy, like a return. This is IF HIT SOMETHING.
        {
            if (raycastHit.transform.TryGetComponent(out BaseCounter baseCounter))      //Same as GetComponent (ClearCounter clearCounter). if (clearCounter != null). IF interact with counter
            {
                if (baseCounter != selectedCounter)       //If new counter is not selected, select it.
                {
                    SetSelectedCounter(baseCounter);
                }
            }
            else             //Raycast didn't hit a counter.
            {
                SetSelectedCounter(null);
            }
        }
        else      //Raycast hit nothing.
        {
            SetSelectedCounter(null);
        }





    }
    void HandleMovement()
    {
        Vector2 inputVector = inputManager.GetMovementVector();

        Vector3 moveDir = new Vector3(inputVector.x, 0, inputVector.y);    //Needs to be vector 3. Input is vector 2.

        float playerRadius = 0.7f;
        float playerHeight = 2f;
        float moveDistance = moveSpeed * Time.deltaTime;
        bool canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDir, moveDistance);      //Fires a capsule ray from player pos, to see if collision. canMove if no collision.

        if (!canMove)   //Cannot move, like if wall. test to see if can move in one dir if multi direction input, because if I press diagonal at wall it should move along the wall.
        {
            //attempt only x movement.
            Vector3 moveDirX = new Vector3(moveDir.x, 0, 0).normalized;     //Normalized so dont get the speed issue, makes the input always 1.
            canMove = (moveDir.x < -0.5f || moveDir.x > 0.5f) && !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDirX, moveDistance);    //Only considers diagonals if significant x input. This is because with controller joystick it can be like 0.01. So we changed from 0, to 0.5 threshold.

            if (canMove)                  //Can move on X only.
                moveDir = moveDirX;
            else                        //Cannot move on X only. Check Z only movement.
            {
                Vector3 moveDirZ = new Vector3(0, 0, moveDir.z).normalized;
                canMove = (moveDir.z < -0.5f || moveDir.z > 0.5f) && !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDirZ, moveDistance);

                if (canMove)                //Can move Z only.
                    moveDir = moveDirZ;

            }
        }

        if (canMove)
            transform.position += moveDir * Time.deltaTime * moveSpeed;    //frame rate independent.

        isWalking = moveDir != Vector3.zero;

        float rotateSpeed = 10f;
        transform.forward = Vector3.Slerp(transform.forward, moveDir, Time.deltaTime * rotateSpeed);   //Player faces the move direction and interpolates.       Slerp is for rotation, lerp is for position! Great tip!
    }

    void SetSelectedCounter(BaseCounter selectedCounter)
    {
        this.selectedCounter = selectedCounter;
        OnSelectedCounterChanged?.Invoke(this, new OnSelectedCounterChangedEventArs { selectedCounter = selectedCounter });      //Fire event and send the info.
    }

    public Transform GetKitchenObjectParentTransform()
    {
        return kitchenObjectHoldPoint;
    }

    public void SetKitchenObject(KitchenObject kitchenObject)
    {
        this.KitchenObject = kitchenObject;

        if (kitchenObject != null)       //Did pick something up.
            OnPickedUpSomething?.Invoke(this, EventArgs.Empty);          //Fire event for sound effect.
    }

    public void ClearKitchenObject()
    {
        KitchenObject = null;
    }

    public bool HasKitchenObject()
    {
        return KitchenObject != null;
    }

}
