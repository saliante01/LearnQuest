using UnityEditor;
using UnityEngine;

public class BotonesMenu : MonoBehaviour
{
    public void StartGame() {

        LevelLoader.LoadLevel("SampleScene");
    }

    public void Exit()
    {

#if UNITY_EDITOR
        Debug.Log("Saliendo del modo Play (Editor)");
        EditorApplication.isPlaying = false;
#else

        Debug.Log("Saliendo del juego (Build)");
        Application.Quit();
#endif
    }
}
