using UnityEngine;

public class PlateIconsUI : MonoBehaviour
{

    [SerializeField] PlateKitchenObject plateKitchenObject;
    [SerializeField] Transform iconTemplate;

    private void Awake()
    {
        iconTemplate.gameObject.SetActive(false);        //No icons by default on the plate. Only when place something on it.
    }

    private void Start()
    {
        plateKitchenObject.OnIngredientAdded += PlateKitchenObject_OnIngredientAdded;
    }

    private void PlateKitchenObject_OnIngredientAdded(object sender, PlateKitchenObject.OnIngredientAddedEventArgs e)        //This is good code. A lot better than the plateKitchenObject referencing this and calling update visual . Using an event means this is totally seperate, dont need the ref.
    {
        UpdateVisual();
    }

    void UpdateVisual()
    {

        foreach (Transform child in transform)
        {
            if (child == iconTemplate)           //Keep the iconTemplate child. Delete all others, as otherwise icons would be duplicated. I pick up lettuce, then tomato, it would so lettuce, lettuce tomato because event fires twice and its reading from list.
                continue;

            Destroy(child.gameObject);
        }

        foreach (KitchenObjectSO kitchenObjectSO in plateKitchenObject.KitchenObjectSOList)
        {
            Transform iconTransform = Instantiate(iconTemplate, transform);       //Spawn the icon as a child of this, so it does the grid layout.
            iconTransform.gameObject.SetActive(true);
            iconTransform.GetComponent<PlateIconSingleUI>().SetKitchenObjectSO(kitchenObjectSO);
        }
    }


}
