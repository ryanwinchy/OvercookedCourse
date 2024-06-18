using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class Loader         //Cannot have instances, class only. This is not a game object, if it was it would be deleted when go to a new scene.
{
    public enum Scene { MainMenuScene, GameScene, LoadingScene}

    static Scene targetScene;

    public static void Load(Scene targetScene)
    {
        Loader.targetScene = targetScene;

        SceneManager.LoadScene(Scene.LoadingScene.ToString());

    }


    public static void LoaderCallback()          //This is called from the loading scene in the first update frame.
    {
        SceneManager.LoadScene(targetScene.ToString());

    }



}
