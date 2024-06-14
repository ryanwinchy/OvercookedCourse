using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KitchenObject : MonoBehaviour        //This is the item, like tomato.
{

    [SerializeField] KitchenObjectSO kitchenObjectSO;

    IKitchenObjectParent kitchenObjectParent;         //This was previously all ClearCounter. Used interface so can also be a player not just a counter. Codemonkey overcooked vid, 3hr 40 so good to see this!

    public KitchenObjectSO GetKitchenObjectSO() => kitchenObjectSO;

    public IKitchenObjectParent GetKitchenObjectParent() => kitchenObjectParent;

    public void SetKitchenObjectParent(IKitchenObjectParent kitchenObjectParent)       //if want to change counter tomato is on.
    {
        if (this.kitchenObjectParent != null)     //If previous counter not null.
        {
            this.kitchenObjectParent.ClearKitchenObject();     //Clear it.
        }

        if (kitchenObjectParent.HasKitchenObject())               //if new counter has a tomato already on it. THis should not be possible!
        {
            Debug.LogError("KitchenObjectParent already has a kitchen object!");
        }

        this.kitchenObjectParent = kitchenObjectParent;        
        kitchenObjectParent.SetKitchenObject(this);     //Set counter to new counter.

        transform.parent = kitchenObjectParent.GetKitchenObjectParentTransform();     //Set to new counter's top point.
        transform.localPosition = Vector3.zero;             //0 on new counter.
    }


    public void DestroySelf()             //Like when cutting it.
    {
        kitchenObjectParent.ClearKitchenObject();
        Destroy(gameObject);
    }


    public static KitchenObject SpawnKitchenObject(KitchenObjectSO kitchenObjectSO, IKitchenObjectParent kitchenObjectParent)    //Spawn a kitchen object. This belongs to the class itself, not an instance. Weird, I would not put here.
    {
        Transform kitchenObjectTransform = Instantiate(kitchenObjectSO.prefab);
        KitchenObject kitchenObject = kitchenObjectTransform.GetComponent<KitchenObject>();
        kitchenObject.SetKitchenObjectParent(kitchenObjectParent);               //Sets the tomato's counter to this counter or player.

        return kitchenObject;
    }




}
