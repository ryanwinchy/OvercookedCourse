using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseCounter : MonoBehaviour, IKitchenObjectParent           //Because all counters can hold tomatos.
{

    public static event EventHandler OnAnyObjectPlacedHere;       //static event so belongs to class not a specific counter. So only have to listen once not to every single counter, more efficient.

    public static void ResetStaticData()    //Clears all listeners. Called from main menu to reset. Have to do this as is static event, belongs to class not instance.
    {
        OnAnyObjectPlacedHere = null;
    }

    [SerializeField] Transform counterTopPoint;


    public KitchenObject KitchenObject { get; private set; }            //Store whatever is on top of the counter.
    public virtual void Interact(Player player)          //Children to override to provide own code. But here means can call it on base, not on individual children.
    {
        Debug.LogError("BaseCounter.Interact();");
    }

    public virtual void InteractAlternate(Player player)          //Children to override to provide own code. But here means can call it on base, not on individual children. Only children that have cutting should implement this.. Like cutting counter.
    {
        
    }

    public Transform GetKitchenObjectParentTransform()
    {
        return counterTopPoint;
    }


    public void SetKitchenObject(KitchenObject kitchenObject)
    {
        KitchenObject = kitchenObject;

        if (kitchenObject != null)     //If there was a kitchen object placed.
        {
            OnAnyObjectPlacedHere?.Invoke(this, EventArgs.Empty);        //Fire event for sound effect.
        }
    }


    public void ClearKitchenObject() => KitchenObject = null;

    public bool HasKitchenObject() => KitchenObject != null;

}
