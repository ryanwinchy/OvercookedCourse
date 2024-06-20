using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashCounter : BaseCounter
{

    public static event EventHandler OnAnyObjectTrashed;     //Static in case we want multiple in future. Belongs to class, not a specific trash counter. So only listeners only listen once, more efficient.

    new public static void ResetStaticData()    //Clears all listeners. Called from main menu to reset. Have to do this as is static event, belongs to class not instance. NEW here is because base has it as well ,but we want both cleared so put 'new'.
    {
        OnAnyObjectTrashed = null;
    }
    public override void Interact(Player player)
    {
        if (player.HasKitchenObject())
        {
            player.KitchenObject.DestroySelf();

            OnAnyObjectTrashed?.Invoke(this, EventArgs.Empty);
        }
    }
}
