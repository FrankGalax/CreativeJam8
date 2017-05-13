using UnityEngine;
using UnityEngine.SceneManagement;

public class Mainmenu : MonoBehaviour
{
    public void Play()
    {
        SceneManager.LoadScene("play");
    }

    public void Quit()
    {
        Application.Quit();
    }
}