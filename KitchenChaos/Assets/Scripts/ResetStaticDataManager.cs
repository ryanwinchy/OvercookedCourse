using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetStaticDataManager : MonoBehaviour
{

    private void Awake()
    {
        CuttingCounter.ResetStaticData();          //Resets static events on scene change.
        BaseCounter.ResetStaticData();
        TrashCounter.ResetStaticData();
    }
}
