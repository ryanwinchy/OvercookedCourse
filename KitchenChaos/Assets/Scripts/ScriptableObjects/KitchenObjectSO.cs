using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class KitchenObjectSO : ScriptableObject
{

    public Transform prefab;                    //Scriptable objects are read only, so feel free to use public here, the one exception.
    public Sprite sprite;
    public string objectName;





}
