using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
 
    Animator animator;

    Player player;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        player = GetComponentInParent<Player>();

        
    }

    private void Update()
    {
        animator.SetBool("IsWalking", player.GetIsWalking());
    }


}
