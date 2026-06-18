using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance { get; private set; }

    [SerializeField] private int scoreActuel;

    private const string BestScoreKey = "BestScore";

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    /// <summary>Incrémente le score et met à jour l'UI.</summary>
    public void AjouterPoint()
    {
        scoreActuel++;
        UIManager.Instance?.MettreAJourScore(scoreActuel);
    }

    /// <summary>Remet le score courant à zéro.</summary>
    public void ReinitialiserScore()
    {
        scoreActuel = 0;
    }

    /// <summary>Sauvegarde le meilleur score dans PlayerPrefs si le score actuel est supérieur.</summary>
    public void SauvegarderMeilleurScore()
    {
        int savedBest = PlayerPrefs.GetInt(BestScoreKey, 0);
        if (scoreActuel > savedBest)
        {
            PlayerPrefs.SetInt(BestScoreKey, scoreActuel);
            PlayerPrefs.Save();
        }
    }

    /// <summary>Retourne le score courant.</summary>
    public int ObtenirScore()
    {
        return scoreActuel;
    }
}
