using System;
using UnityEngine;

public class StoveCounter : BaseCounter, IHasProgress
{
    public event EventHandler<IHasProgress.OnProgressChangedEventArgs> OnProgressChanged;          //Getting EventArgs from interface.

    public enum State { Idle, Frying, Fried, Burned }

    public event EventHandler<OnStateChangedEvent> OnStateChanged;
    public class OnStateChangedEvent : EventArgs      //One method to send info in event. If only sending one thing, I prefer the more compact method in my notes.
    {
        public State state;
    }

    [SerializeField] FryingRecipeSO[] fryingRecipeSOArray;
    [SerializeField] BurningRecipeSO[] burningRecipeSOArray;

    State state;

    float fryingTimer = 0f;
    float burningTimer = 0f;

    FryingRecipeSO fryingRecipeSO;
    BurningRecipeSO burningRecipeSO;

    private void Start()
    {
        state = State.Idle;
    }

    private void Update()
    {
        if (HasKitchenObject())
        {
            switch (state)
            {
                case State.Idle:
                    break;
                case State.Frying:
                    fryingTimer += Time.deltaTime;
                    OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs { progressNormalized = fryingTimer / fryingRecipeSO.fryingTimerMax });

                    if (fryingTimer > fryingRecipeSO.fryingTimerMax)       //Meat has been fried.
                    {
                        KitchenObject.DestroySelf();

                        KitchenObject.SpawnKitchenObject(fryingRecipeSO.output, this);         //Because this is a static function.

                        burningRecipeSO = GetBurningRecipeSOWithInput(KitchenObject.GetKitchenObjectSO());

                        state = State.Fried;
                        burningTimer = 0f;

                        OnStateChanged?.Invoke(this, new OnStateChangedEvent { state = state });    //Fire state change event.
                    }
                    break;
                case State.Fried:
                    burningTimer += Time.deltaTime;

                    OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs { progressNormalized = burningTimer / burningRecipeSO.burningTimerMax });
                    if (burningTimer > burningRecipeSO.burningTimerMax)       //Meat has been fried.
                    {
                        KitchenObject.DestroySelf();

                        KitchenObject.SpawnKitchenObject(burningRecipeSO.output, this);         //Because this is a static function.

                        state = State.Burned;
                        OnStateChanged?.Invoke(this, new OnStateChangedEvent { state = state });    //Fire state change event.

                        OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs { progressNormalized = 0f });
                    }
                    break;
                case State.Burned:
                    break;
            }
        }


    }

    public override void Interact(Player player)
    {
        if (!HasKitchenObject())      //No object on counter.
        {
            if (player.HasKitchenObject())      //Player is carrying something.
            {
                if (HasRecipeWithInput(player.KitchenObject.GetKitchenObjectSO()))       //Player carrying something that can be fried.
                {
                    player.KitchenObject.SetKitchenObjectParent(this);      //moves kitchen object from player to this counter.

                    fryingRecipeSO = GetFryingRecipeSOWithInput(KitchenObject.GetKitchenObjectSO());

                    state = State.Frying;
                    fryingTimer = 0f;
                    OnStateChanged?.Invoke(this, new OnStateChangedEvent { state = state });    //Fire state change event.

                    OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs { progressNormalized = fryingTimer / fryingRecipeSO.fryingTimerMax });
                }
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

                        state = State.Idle;
                        OnStateChanged?.Invoke(this, new OnStateChangedEvent { state = state });    //Fire state change event.
                        OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs { progressNormalized = 0f });     //Fire progress bar update.
                    }

                }
            }
            else          //Player carrying nothing.
            {
                KitchenObject.SetKitchenObjectParent(player);

                state = State.Idle;
                OnStateChanged?.Invoke(this, new OnStateChangedEvent { state = state });    //Fire state change event.
                OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs { progressNormalized = 0f });     //Fire progress bar update.

            }
        }
    }




    bool HasRecipeWithInput(KitchenObjectSO inputKitchenObjectSO)      //Check if the kitchen object has a recipe (like tomato to cut tomato). Bread would be false.
    {
        FryingRecipeSO fryingRecipeSO = GetFryingRecipeSOWithInput(inputKitchenObjectSO);
        return fryingRecipeSO != null;

    }


    KitchenObjectSO GetOutputForInput(KitchenObjectSO inputKitchenObjectSO)       //Gets chopped tomato from tomato for eg.
    {
        FryingRecipeSO fryingRecipeSO = GetFryingRecipeSOWithInput(inputKitchenObjectSO);
        if (fryingRecipeSO != null)
        {
            return fryingRecipeSO.output;
        }
        else         //Recipe not found.
            return null;

    }

    FryingRecipeSO GetFryingRecipeSOWithInput(KitchenObjectSO inputKitchenObjectSO)
    {
        foreach (FryingRecipeSO fryingRecipeSO in fryingRecipeSOArray)
        {
            if (fryingRecipeSO.input == inputKitchenObjectSO)
                return fryingRecipeSO;
        }

        return null;
    }

    BurningRecipeSO GetBurningRecipeSOWithInput(KitchenObjectSO inputKitchenObjectSO)
    {
        foreach (BurningRecipeSO burningRecipeSO in burningRecipeSOArray)
        {
            if (burningRecipeSO.input == inputKitchenObjectSO)
                return burningRecipeSO;
        }

        return null;
    }

    public bool IsFried() => state == State.Fried;

}
