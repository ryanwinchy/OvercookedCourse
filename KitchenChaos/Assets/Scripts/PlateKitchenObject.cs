using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlateKitchenObject : KitchenObject
{

    public event EventHandler<OnIngredientAddedEventArgs> OnIngredientAdded;
    public class OnIngredientAddedEventArgs : EventArgs       //So can send the ingredient added.
    {
        public KitchenObjectSO kitchenObjectSO;
    }


    [SerializeField] List<KitchenObjectSO> validKitchenSOList;

    List<KitchenObjectSO> kitchenObjectSOList;              //List of ingredients on plate.

    private void Awake()
    {
        kitchenObjectSOList = new List<KitchenObjectSO>();
    }

    public bool TryAddIngredient(KitchenObjectSO kitchenObjectSO)
    {

        if (!validKitchenSOList.Contains(kitchenObjectSO))        //Not a valid ingredient, like unchopped tomato.
            return false;

        if (kitchenObjectSOList.Contains(kitchenObjectSO))         //already has an ingredient of this type. Can't have two chopped tomatoes.
        {
            return false;
        }
        else                                               //Add ingredient.
        {
            kitchenObjectSOList.Add(kitchenObjectSO);
            OnIngredientAdded?.Invoke(this, new OnIngredientAddedEventArgs { kitchenObjectSO = kitchenObjectSO });     //Fire event and send ingredient added.
            return true;
        }


    }
}
