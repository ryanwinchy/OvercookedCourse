using System;
using UnityEngine;

public class ContainerCounter : BaseCounter
{

    public event EventHandler OnPlayerGrabbedObject;

    [SerializeField] KitchenObjectSO kitchenObjectSO;            //Transform & GameObject interchangeable here.


    public override void Interact(Player player)
    {

        if (!player.HasKitchenObject())      //if player not carrying anything, give object on this counter.
        {
            KitchenObject.SpawnKitchenObject(kitchenObjectSO, player);        //Static function on kitchen object class. Spawn new tomato, pass in what to spawn and the parent (this).


            OnPlayerGrabbedObject?.Invoke(this, EventArgs.Empty);      //Fire event.

        }


    }






}
