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

    public List<KitchenObjectSO> KitchenObjectSOList { get; private set; }             //List of ingredients on plate.   Property as need the getter in the UI icons for the plate.

    private void Awake()
    {
        KitchenObjectSOList = new List<KitchenObjectSO>();
    }

    public bool TryAddIngredient(KitchenObjectSO kitchenObjectSO)
    {

        if (!validKitchenSOList.Contains(kitchenObjectSO))        //Not a valid ingredient, like unchopped tomato.
            return false;

        if (KitchenObjectSOList.Contains(kitchenObjectSO))         //already has an ingredient of this type. Can't have two chopped tomatoes.
        {
            return false;
        }
        else                                               //Add ingredient.
        {
            KitchenObjectSOList.Add(kitchenObjectSO);
            OnIngredientAdded?.Invoke(this, new OnIngredientAddedEventArgs { kitchenObjectSO = kitchenObjectSO });     //Fire event and send ingredient added.
            return true;
        }


    }



}
