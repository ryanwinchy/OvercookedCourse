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
            Transform kitchenObjectTransform = Instantiate(kitchenObjectSO.prefab);
            kitchenObjectTransform.GetComponent<KitchenObject>().SetKitchenObjectParent(player);               //Sets the tomato's counter to this counter.

            OnPlayerGrabbedObject?.Invoke(this, EventArgs.Empty);      //Fire event.

        }


    }






}
