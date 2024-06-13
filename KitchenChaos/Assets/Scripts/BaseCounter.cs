using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseCounter : MonoBehaviour, IKitchenObjectParent           //Because all counters can hold tomatos.
{
    [SerializeField] Transform counterTopPoint;


    public KitchenObject KitchenObject { get; private set; }            //Store whatever is on top of the counter.
    public virtual void Interact(Player player)          //Children to override to provide own code. But here means can call it on base, not on individual children.
    {
        Debug.LogError("BaseCounter.Interact();");
    }

    public Transform GetKitchenObjectParentTransform()
    {
        return counterTopPoint;
    }


    public void SetKitchenObject(KitchenObject kitchenObject) => this.KitchenObject = kitchenObject;

    public void ClearKitchenObject() => KitchenObject = null;

    public bool HasKitchenObject() => KitchenObject != null;

}
