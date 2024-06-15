using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatesCounter : BaseCounter
{

    public event EventHandler OnPlateSpawned;
    public event EventHandler OnPlateRemoved;

    [SerializeField] KitchenObjectSO plateKitchenObjectSO;

    float spawnPlateTimer;
    [SerializeField] float spawnPlateTimerMax = 4f;

    int platesSpawnedAmount;
    int platesSPawnedAmountMax = 4;
    private void Update()
    {
        spawnPlateTimer += Time.deltaTime;

        if (spawnPlateTimer > spawnPlateTimerMax)
        {
            spawnPlateTimer = 0f;

            if (platesSpawnedAmount < platesSPawnedAmountMax)
            {
                platesSpawnedAmount++;

                OnPlateSpawned?.Invoke(this, EventArgs.Empty);       //Fire event for visual to update.
            }
        }
    }

    public override void Interact(Player player)
    {
        if (!player.HasKitchenObject())       //Player not holding anything.
        {
            if (platesSpawnedAmount > 0)         //At least one plate.
            {
                platesSpawnedAmount--;

                KitchenObject.SpawnKitchenObject(plateKitchenObjectSO, player);

                OnPlateRemoved?.Invoke(this, EventArgs.Empty);       //Fire event for visual to update.

            }
        }
    }




}
