using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatesCounterVisual : MonoBehaviour           //This is basically to visually show stacks of plates on the counter. The actual kitchen object logic is on the PlatesCounter.
{
    [SerializeField] Transform counterTopPoint;
    [SerializeField] Transform plateVisualPrefab;
    [SerializeField] PlatesCounter platesCounter;

    List<GameObject> plateVisualGameObjectList;      //List of plate visuals spawned.


    private void Awake()
    {
        plateVisualGameObjectList = new List<GameObject>();
    }

    private void Start()
    {
        platesCounter.OnPlateSpawned += PlatesCounter_OnPlateSpawned;
        platesCounter.OnPlateRemoved += PlatesCounter_OnPlateRemoved;
    }

    private void PlatesCounter_OnPlateSpawned(object sender, System.EventArgs e)
    {
        Transform plateVisualTransform = Instantiate(plateVisualPrefab, counterTopPoint);       //Spawn plate visual.

        float plateOffsetY = 0.1f;                //Raises each new plate up this much so sits on top of the previous.
        plateVisualTransform.localPosition = new Vector3(0, plateOffsetY * plateVisualGameObjectList.Count, 0);

        plateVisualGameObjectList.Add(plateVisualTransform.gameObject);
    }

    void PlatesCounter_OnPlateRemoved(object sender, System.EventArgs e)
    {
        GameObject plateGameObject = plateVisualGameObjectList[plateVisualGameObjectList.Count - 1];        //Last plate in list.

        plateVisualGameObjectList.Remove(plateGameObject);

        Destroy(plateGameObject);
    }




}
