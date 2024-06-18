using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{

    public static SoundManager Instance { get; private set; }     //Singleton.

    [SerializeField] AudioClipRefsSO audioClipRefsSO;         //Just another way of doing it, having refs on an SO. But basically same as having refs here, and probably makes more sense to have here to be honest.

    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        DeliveryManager.Instance.OnRecipeSuccess += DeliveryManager_OnRecipeSuccess;        //Listen to logic script.
        DeliveryManager.Instance.OnRecipeFailed += DeliveryManager_OnRecipeFailed;
        CuttingCounter.OnAnyCut += CuttingCounter_OnAnyCut;
        Player.Instance.OnPickedUpSomething += Player_OnPickedUpSomething;
        BaseCounter.OnAnyObjectPlacedHere += BaseCounter_OnAnyObjectPlacedHere;
        TrashCounter.OnAnyObjectTrashed += TrashCounter_OnAnyObjectTrashed;
    }

    private void TrashCounter_OnAnyObjectTrashed(object sender, System.EventArgs e)
    {
        TrashCounter trashCounter = sender as TrashCounter;               //So we know which trash counter it came from.
        PlaySound(audioClipRefsSO.trash, trashCounter.transform.position);
    }

    private void BaseCounter_OnAnyObjectPlacedHere(object sender, System.EventArgs e)
    {
        BaseCounter baseCounter = sender as BaseCounter;       //So we know which base counter it came from.
        PlaySound(audioClipRefsSO.objectDrop, baseCounter.transform.position);
    }

    private void Player_OnPickedUpSomething(object sender, System.EventArgs e)
    {
        PlaySound(audioClipRefsSO.objectPickup, Player.Instance.transform.position);
    }

    private void CuttingCounter_OnAnyCut(object sender, System.EventArgs e)
    {
        CuttingCounter cuttingCounter = sender as CuttingCounter;        //Static event , so fired on class itself not a specific cutting counter. This then gets which counter it was. Probably better to do as normal event?

        PlaySound(audioClipRefsSO.chop, cuttingCounter.transform.position);
    }

    private void DeliveryManager_OnRecipeFailed(object sender, System.EventArgs e)
    {
        DeliveryCounter deliveryCounter = DeliveryCounter.Instance;
        PlaySound(audioClipRefsSO.deliveryFail, deliveryCounter.transform.position);      //Plays at position of delivery counter in the world.
    }

    private void DeliveryManager_OnRecipeSuccess(object sender, System.EventArgs e)
    {
        DeliveryCounter deliveryCounter = DeliveryCounter.Instance;
        PlaySound(audioClipRefsSO.deliverySuccess, deliveryCounter.transform.position);
    }

    void PlaySound(AudioClip audioClip, Vector3 position, float volume = 1f)        //Third param is optional and has a default!
    {
        AudioSource.PlayClipAtPoint(audioClip, position, volume);
    }

    void PlaySound(AudioClip[] audioClipArray, Vector3 position, float volume = 1f)        //Third param is optional and has a default! This one takes array, so if send array this one automatically will get called! So cool :D
    {
        PlaySound(audioClipArray[Random.Range(0, audioClipArray.Length)], position, volume);    //selects random in array at plays it.
    }

    public void PlayFootStepSound(Vector3 position, float volume)        //Special function because this is called from PlayerSounds script.
    {
        PlaySound(audioClipRefsSO.footstep, position, volume);
    }




}
