using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class MainMenuUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI texteMeilleurScore;

    private const string BestScoreKey = "BestScore";
    private const string Level1Scene = "Level1Scene";

    private void Start()
    {
        int meilleurScore = PlayerPrefs.GetInt(BestScoreKey, 0);
        if (texteMeilleurScore != null)
            texteMeilleurScore.text = $"Meilleur score : {meilleurScore}";
    }

    /// <summary>Réinitialise le score et charge la scène Level1Scene.</summary>
    public void SurClicJouer()
    {
        // On remet le score à 0 avant de démarrer une nouvelle partie
        ScoreManager.Instance?.ReinitialiserScore();
        SceneManager.LoadScene(Level1Scene);
    }

    /// <summary>Quitte l'application.</summary>
    public void SurClicQuitter()
    {
        Application.Quit();
    }
}
