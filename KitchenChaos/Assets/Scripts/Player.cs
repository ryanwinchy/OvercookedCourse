using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] float moveSpeed = 7f;
    [SerializeField] InputManager inputManager;

    bool isWalking;
    private void Update()
    {

        Vector2 inputVector = inputManager.GetMovementVector();

        Vector3 moveDir = new Vector3(inputVector.x, 0, inputVector.y);    //Needs to be vector 3. Input is vector 2.

        transform.position += moveDir * Time.deltaTime * moveSpeed;    //frame rate independent.

        isWalking = moveDir != Vector3.zero;

        float rotateSpeed = 10f;
        transform.forward = Vector3.Slerp(transform.forward, moveDir, Time.deltaTime * rotateSpeed);   //Player faces the move direction and interpolates.       Slerp is for rotation, lerp is for position! Great tip!
    }

    public bool GetIsWalking() => isWalking;



}
