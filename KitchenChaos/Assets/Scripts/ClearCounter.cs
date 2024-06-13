using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearCounter : BaseCounter
{


    [SerializeField] KitchenObjectSO kitchenObjectSO;

    public override void Interact(Player player)      //Pickup and drop.
    {
        if (!HasKitchenObject())      //No object on counter.
        {
            if (player.HasKitchenObject())      //Player is carrying something.
            {
                player.KitchenObject.SetKitchenObjectParent(this);      //moves kitchen object from player to this counter.
            }

        }
        else            //Object already on counter.
        {
            if (player.HasKitchenObject())        //Player is carrying something.
            {

            }
            else          //Player carrying nothing.
            {
                KitchenObject.SetKitchenObjectParent(player);
            }
        }

    }



}
