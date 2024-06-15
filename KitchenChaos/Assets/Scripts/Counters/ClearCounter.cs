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
                if (player.KitchenObject.TryGetPlate(out PlateKitchenObject plateKitchenObject))     //Player is holding plate.
                {
                  if (plateKitchenObject.TryAddIngredient(KitchenObject.GetKitchenObjectSO()))    //Add to plate what was on this counter.
                    {
                        KitchenObject.DestroySelf();     //Destroy what was on counter.
                    }

                }
                else                          //Player not holding a plate, but something else.
                {
                    if (KitchenObject.TryGetPlate(out plateKitchenObject))      //Counter is holding a plate. This is out, so is same variable as we defined above, plateKitchenObject.
                    {
                        if (plateKitchenObject.TryAddIngredient(player.KitchenObject.GetKitchenObjectSO()))         //Add what player is holding to plate, and destroy kitchen object from players hands.
                        {
                            player.KitchenObject.DestroySelf();
                        }

                    }
                }
            }
            else          //Player carrying nothing.
            {
                KitchenObject.SetKitchenObjectParent(player);
            }
        }

    }



}
