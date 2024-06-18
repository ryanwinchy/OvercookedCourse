public class DeliveryCounter : BaseCounter
{

    public static DeliveryCounter Instance { get; private set; }        //Singleton as only one delivery counter. Makes easier to get reference to it in sound effect manager.

    private void Awake()
    {
        Instance = this;
    }
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
