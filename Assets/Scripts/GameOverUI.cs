using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverUI : MonoBehaviour
{
    private const string Level1Scene = "Level1Scene";
    private const string MenuScene = "MenuScene";

    /// <summary>Relance le niveau 1 en réinitialisant le score.</summary>
    public void SurClicRejouer()
    {
        ScoreManager.Instance.ReinitialiserScore();
        SceneManager.LoadScene(Level1Scene);
    }

    /// <summary>Retourne au menu principal en réinitialisant le score.</summary>
    public void SurClicRetourMenu()
    {
        ScoreManager.Instance.ReinitialiserScore();
        SceneManager.LoadScene(MenuScene);
    }
}
