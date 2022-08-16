using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayButton : MonoBehaviour
{
    // Имя загружаемой сцены
    [SerializeField] private string nextScene = "";

    private void Update()
    {
        if (Input.GetMouseButtonDown (0))
        {
           SceneManager.LoadScene(nextScene);
        }
    }
}