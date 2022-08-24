using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private GameObject _loadScreen;
    public void ToMenu()
    {
        _loadScreen.SetActive(true);
        SceneManager.LoadScene(0);
        
    }
    public void Restart()
    {
        _loadScreen.SetActive(true);
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
    }
    public void LoadScene(int num)
    {
        _loadScreen.SetActive(true);
        SceneManager.LoadScene(num);
    }
    public void LoadNextLvl()
    {
        _loadScreen.SetActive(true);
        Scene scen = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scen.buildIndex+1);
    }
}
