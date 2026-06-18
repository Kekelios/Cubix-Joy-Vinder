using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class MainMenuUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI texteMeilleurScore;

    private const string LEVEL1_SCENE = "Level1Scene";

    private void Start()
    {
        if (texteMeilleurScore != null)
            texteMeilleurScore.text = $"Meilleur score : {ScoreManager.LireMeilleurScore()}";
    }

    /// <summary>Réinitialise le score et charge la scène Level1Scene.</summary>
    public void SurClicJouer()
    {
        ScoreManager.Instance?.ReinitialiserScore();
        SceneManager.LoadScene(LEVEL1_SCENE);
    }

    /// <summary>Quitte l'application.</summary>
    public void SurClicQuitter()
    {
        Application.Quit();
    }
}
