using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DeliveryManager : MonoBehaviour
{

    public event EventHandler OnRecipeSpawned;                 //Events for UI.
    public event EventHandler OnRecipeCompleted;
    public event EventHandler OnRecipeSuccess;                 //Events for sounds.
    public event EventHandler OnRecipeFailed;
    public static DeliveryManager Instance {  get; private set; }

    [SerializeField] RecipeListSO recipeListSO;         //Bit of an intermediary for learning. Instead of making a new SO for the list of recipes, we could have just had a list of the recipes. THE BENEFIT of this approach is if multiple scripts need the list of recipes, a SO means you dont dupe all the references and can easily add new recipes in only one place.
    public List<RecipeSO> WaitingRecipeSOList { get; private set; }        //List of recipes customers are waiting for.

    float spawnRecipeTimer;
    float spawnRecipeTimerMax = 4f;

    int waitingRecipesMax = 4;

    public int SuccessfulRecipesAmount {  get; private set; }


    private void Awake()
    {
        Instance = this;


        WaitingRecipeSOList = new List<RecipeSO>();
    }
    private void Update()
    {
        spawnRecipeTimer -= Time.deltaTime;

        if (spawnRecipeTimer < 0f)
        {
            spawnRecipeTimer = spawnRecipeTimerMax;

            if (KitchenGameManager.Instance.IsGamePlaying() && (WaitingRecipeSOList.Count < waitingRecipesMax))
            {
                RecipeSO waitingRecipeSO = recipeListSO.recipeSOList[UnityEngine.Random.Range(0, recipeListSO.recipeSOList.Count)];   //Random recipe and add to list.
                WaitingRecipeSOList.Add(waitingRecipeSO);

                OnRecipeSpawned?.Invoke(this, EventArgs.Empty);       //Fire event.
            }

        }
    }

    public void DeliverRecipe(PlateKitchenObject plateKitchenObject)     //Receive plate.
    {
        for (int i = 0; i < WaitingRecipeSOList.Count; i++)
        {
            RecipeSO waitingRecipeSO = WaitingRecipeSOList[i];

            if (waitingRecipeSO.kitchenObjectSOList.Count == plateKitchenObject.KitchenObjectSOList.Count)     //Has same number of ingredients.
            {
                bool plateContentsMatchesRecipe = true;
                foreach (KitchenObjectSO recipeKitchenObjectSO in waitingRecipeSO.kitchenObjectSOList)       //Cycle through all ingredients in recipe.
                {
                    bool ingredientFound = false;
                    foreach (KitchenObjectSO plateKitchenObjectSO in plateKitchenObject.KitchenObjectSOList)     //Cycle through all ingredients on plate.
                    {
                        if (plateKitchenObjectSO == recipeKitchenObjectSO)   //Ingredient matches!
                        {
                            ingredientFound = true;
                            break;                     //Breaks foreach once ingredient found, more efficient.
                        }
                    }
                    if (!ingredientFound)            //Recipe ingredient not on plate.
                    {
                        plateContentsMatchesRecipe = false;
                    }

                }
                if (plateContentsMatchesRecipe)      //All ingredients found on plate. Player delivered correct recipe.
                {
                    SuccessfulRecipesAmount++;

                    WaitingRecipeSOList.RemoveAt(i);        //Recipe no longer waiting.

                    OnRecipeCompleted?.Invoke(this, EventArgs.Empty);      // Fire events for UI and sound effects.
                    OnRecipeSuccess?.Invoke(this, EventArgs.Empty);


                    return;                           //So stops these loops once found.
                }
            }
        }

        //No matches found, player did not deliver a correct recipe.
        Debug.Log("Player did not deliver a correct recipe!");
        OnRecipeFailed?.Invoke(this, EventArgs.Empty);

    }



}
