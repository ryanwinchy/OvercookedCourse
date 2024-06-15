using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu]
public class RecipeListSO : ScriptableObject           //This isn't really needed, he jsut did it for teaching. He said making a list of recipeSOs and then selecting randomly and adding to waitingList is another good approach.
{
    public List<RecipeSO> recipeSOList;
}
