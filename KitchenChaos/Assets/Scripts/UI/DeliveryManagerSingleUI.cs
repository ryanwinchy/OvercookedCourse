using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DeliveryManagerSingleUI : MonoBehaviour
{

    [SerializeField] TextMeshProUGUI recipeNameText;
    [SerializeField] Transform iconContainer;
    [SerializeField] Transform iconTemplate;

    private void Awake()
    {
        iconTemplate.gameObject.SetActive(false);
    }

    public void SetRecipeSO(RecipeSO recipeSO)
    {
        recipeNameText.text = recipeSO.recipeName;

        foreach (Transform child in iconContainer)
        {
            if (child == iconTemplate)
                continue;

            Destroy(child.gameObject);      //Destroy all children except the template.

        }

        foreach (KitchenObjectSO kitchenObjectSO in recipeSO.kitchenObjectSOList)
        {
            Transform iconTransform = Instantiate(iconTemplate, iconContainer);
            iconTransform.gameObject.SetActive(true);

            iconTransform.GetComponent<Image>().sprite = kitchenObjectSO.sprite;
        }

    }
}
