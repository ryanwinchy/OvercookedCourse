using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlateCompleteVisual : MonoBehaviour
{

    [Serializable]        //This is so we can see our list of this custom type in the Unity Editor.
    public struct KitchenObjectSO_GameObject        //Structs good for small data packets. This is just to link a chopped items to their visuals. STRUCTS GOOD FOR STORING DATA WITHOUT LOGIC. This I think would be a good candidate for a dictionary.
    {
        public KitchenObjectSO kitchenObjectSO;
        public GameObject gameObject;
    }

    [SerializeField] PlateKitchenObject plateKitchenObject;

    [SerializeField] List<KitchenObjectSO_GameObject> kitchenObjectSO_GameObjectList;

    private void Start()
    {
        plateKitchenObject.OnIngredientAdded += PlateKitchenObject_OnIngredientAdded;

        foreach (KitchenObjectSO_GameObject kitchenObjectSOGameObject in kitchenObjectSO_GameObjectList)          //Set plate complete visual to have nothing at start.
        {
            kitchenObjectSOGameObject.gameObject.SetActive(false);
        }
    }

    private void PlateKitchenObject_OnIngredientAdded(object sender, PlateKitchenObject.OnIngredientAddedEventArgs e)
    {
        foreach (KitchenObjectSO_GameObject kitchenObjectSOGameObject in kitchenObjectSO_GameObjectList)
        {
            if (kitchenObjectSOGameObject.kitchenObjectSO == e.kitchenObjectSO)       //If event ingredient added matches the one in list, load that items visual.
            {
                kitchenObjectSOGameObject.gameObject.SetActive(true);
            }
        }
    }
}
