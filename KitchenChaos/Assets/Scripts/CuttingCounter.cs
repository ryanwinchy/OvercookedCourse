using System;
using UnityEngine;

public class CuttingCounter : BaseCounter
{

    public event EventHandler<OnProgressChangedEventArgs> OnCuttingProgressChanged;
    public class OnProgressChangedEventArgs : EventArgs
    {
        public float progressNormalized;                //as progress bars use float not int. So convert.
    }

    public event EventHandler OnCut;

    [SerializeField] CuttingRecipeSO[] cuttingRecipeSOArray;

    int cuttingProgress;
    public override void Interact(Player player)                           //Drop on cutting board IF cuttable, like tomato. But not bread.
    {
        if (!HasKitchenObject())      //No object on counter.
        {
            if (player.HasKitchenObject())      //Player is carrying something.
            {
                if (HasRecipeWithInput(player.KitchenObject.GetKitchenObjectSO()))       //Player carrying something that can be cut.
                {
                    player.KitchenObject.SetKitchenObjectParent(this);      //moves kitchen object from player to this counter.
                    cuttingProgress = 0;     //When first placed on chopping counter.

                    CuttingRecipeSO cuttingRecipeSO = GetCuttingRecipeSOWithInput(KitchenObject.GetKitchenObjectSO());
                    OnCuttingProgressChanged?.Invoke(this, new OnProgressChangedEventArgs { progressNormalized = (float)cuttingProgress / cuttingRecipeSO.cuttingProgressMax });    //So progress is a float, we have to cast one as a float.
                }
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



    public override void InteractAlternate(Player player)
    {
        if (HasKitchenObject() && HasRecipeWithInput(KitchenObject.GetKitchenObjectSO()))         //If has a kitchen object AND is cuttable, cut it.
        {

            cuttingProgress++;
            OnCut?.Invoke(this, EventArgs.Empty);      //Fire cut event for visual animation.

            CuttingRecipeSO cuttingRecipeSO = GetCuttingRecipeSOWithInput(KitchenObject.GetKitchenObjectSO());
            OnCuttingProgressChanged?.Invoke(this, new OnProgressChangedEventArgs { progressNormalized = (float)cuttingProgress / cuttingRecipeSO.cuttingProgressMax });    //So progress is a float, we have to cast one as a float.


            if (cuttingProgress >= cuttingRecipeSO.cuttingProgressMax)       //When fully cut, spawn cut object.
            {
                KitchenObjectSO outputKitchenObjectSO = GetOutputForInput(KitchenObject.GetKitchenObjectSO());      //get chopped tomatoes from tomatoes.

                KitchenObject.DestroySelf();        //Destroy full tomato.

                KitchenObject.SpawnKitchenObject(outputKitchenObjectSO, this);        //Static function on kitchen object class. Spawn new tomato, pass in what to spawn and the parent (this).
            }

        }
    }


    bool HasRecipeWithInput(KitchenObjectSO inputKitchenObjectSO)      //Check if the kitchen object has a recipe (like tomato to cut tomato). Bread would be false.
    {
        CuttingRecipeSO cuttingRecipeSO = GetCuttingRecipeSOWithInput(inputKitchenObjectSO);
        return cuttingRecipeSO != null;

    }


    KitchenObjectSO GetOutputForInput(KitchenObjectSO inputKitchenObjectSO)       //Gets chopped tomato from tomato for eg.
    {
        CuttingRecipeSO cuttingRecipeSO = GetCuttingRecipeSOWithInput(inputKitchenObjectSO);
        if (cuttingRecipeSO != null)
        {
            return cuttingRecipeSO.output;
        }
        else         //Recipe not found.
            return null;

    }

    CuttingRecipeSO GetCuttingRecipeSOWithInput(KitchenObjectSO inputKitchenObjectSO)
    {
        foreach (CuttingRecipeSO cuttingRecipeSO in cuttingRecipeSOArray)
        {
            if (cuttingRecipeSO.input == inputKitchenObjectSO)
                return cuttingRecipeSO;
        }

        return null;
    }


}
