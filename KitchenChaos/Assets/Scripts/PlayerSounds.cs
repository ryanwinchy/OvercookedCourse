using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSounds : MonoBehaviour
{
    Player player;

    float footstepTimer;
    float footstepTimerMax = 0.1f;   //Footsteps per second.

    private void Awake()
    {
        player = GetComponent<Player>();
    }

    private void Update()
    {
        footstepTimer -= Time.deltaTime;

        if (footstepTimer < 0)
        {
            footstepTimer = footstepTimerMax;

            if (player.GetIsWalking())       //Only play as walking.
            {
                float volume = 1f;
                SoundManager.Instance.PlayFootStepSound(player.transform.position, volume);
            }

        }
    }


}
