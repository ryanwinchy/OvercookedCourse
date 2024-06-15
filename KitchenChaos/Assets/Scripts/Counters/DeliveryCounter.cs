public class DeliveryCounter : BaseCounter
{

    public override void Interact(Player player)
    {
        if (player.HasKitchenObject())
        {
            if (player.KitchenObject.TryGetPlate(out PlateKitchenObject plateKitchenObject))    //If player has plate.
            {
                DeliveryManager.Instance.DeliverRecipe(plateKitchenObject);

                player.KitchenObject.DestroySelf();
            }
        }
    }
}
