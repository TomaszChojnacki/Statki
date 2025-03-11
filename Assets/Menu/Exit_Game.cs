using UnityEngine;

public class Exit_Game : MonoBehaviour
{
    public void ExitGame()
    {
        Application.Quit();

        // Dodatkowa linia dla edytora Unity, poniewa¿ Application.Quit() nie dzia³a w trybie edytora
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}