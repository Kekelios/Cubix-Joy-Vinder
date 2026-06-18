using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverUI : MonoBehaviour
{
    private const string LEVEL1_SCENE = "Level1Scene";
    private const string MENU_SCENE   = "MenuScene";

    /// <summary>Relance le niveau 1 en réinitialisant le score.</summary>
    public void SurClicRejouer()
    {
        ScoreManager.Instance.ReinitialiserScore();
        SceneManager.LoadScene(LEVEL1_SCENE);
    }

    /// <summary>Retourne au menu principal en réinitialisant le score.</summary>
    public void SurClicRetourMenu()
    {
        ScoreManager.Instance.ReinitialiserScore();
        SceneManager.LoadScene(MENU_SCENE);
    }
}
