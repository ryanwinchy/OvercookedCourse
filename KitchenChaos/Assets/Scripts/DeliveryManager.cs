using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DeliveryManager : MonoBehaviour
{

    public static DeliveryManager Instance {  get; private set; }

    [SerializeField] RecipeListSO recipeListSO;         //Bit of an intermediary for learning. Instead of making a new SO for the list of recipes, we could have just had a list of the recipes. THE BENEFIT of this approach is if multiple scripts need the list of recipes, a SO means you dont dupe all the references and can easily add new recipes in only one place.
    List<RecipeSO> waitingRecipeSOList;        //List of recipes customers are waiting for.

    float spawnRecipeTimer;
    float spawnRecipeTimerMax = 4f;

    int waitingRecipesMax = 4;


    private void Awake()
    {
        Instance = this;


        waitingRecipeSOList = new List<RecipeSO>();
    }
    private void Update()
    {
        spawnRecipeTimer -= Time.deltaTime;

        if (spawnRecipeTimer < 0f)
        {
            spawnRecipeTimer = spawnRecipeTimerMax;

            if (waitingRecipeSOList.Count < waitingRecipesMax)
            {
                RecipeSO waitingRecipeSO = recipeListSO.recipeSOList[Random.Range(0, recipeListSO.recipeSOList.Count)];   //Random recipe and add to list.
                Debug.Log(waitingRecipeSO.recipeName);
                waitingRecipeSOList.Add(waitingRecipeSO);
            }

        }
    }

    public void DeliverRecipe(PlateKitchenObject plateKitchenObject)     //Receive plate.
    {
        for (int i = 0; i < waitingRecipeSOList.Count; i++)
        {
            RecipeSO waitingRecipeSO = waitingRecipeSOList[i];

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
                    Debug.Log("Correct recipe delivered. Well done lil cunt :D");
                    waitingRecipeSOList.RemoveAt(i);        //Recipe no longer waiting.
                    return;                           //So stops these loops once found.
                }
            }
        }

        //No matches found, player did not deliver a correct recipe.
        Debug.Log("Player did not deliver a correct recipe!");
    }



}
