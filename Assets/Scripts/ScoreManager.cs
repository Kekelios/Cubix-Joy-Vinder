using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance { get; private set; }

    [SerializeField] private int scoreActuel;

    private const string BEST_SCORE_KEY = "BestScore";

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

    /// <summary>Incrémente le score de 1 et met à jour l'UI.</summary>
    public void AjouterPoint()
    {
        AjouterPoints(1);
    }

    /// <summary>Incrémente le score du montant donné et met à jour l'UI.</summary>
    public void AjouterPoints(int montant)
    {
        scoreActuel += montant;
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
        int savedBest = PlayerPrefs.GetInt(BEST_SCORE_KEY, 0);
        if (scoreActuel > savedBest)
        {
            PlayerPrefs.SetInt(BEST_SCORE_KEY, scoreActuel);
            PlayerPrefs.Save();
        }
    }

    /// <summary>Retourne le score courant.</summary>
    public int ObtenirScore()
    {
        return scoreActuel;
    }

    /// <summary>
    /// Lit le meilleur score depuis PlayerPrefs sans nécessiter d'instance.
    /// Utilisable depuis n'importe quelle scène, même si ScoreManager n'est pas encore chargé.
    /// </summary>
    public static int LireMeilleurScore()
    {
        return PlayerPrefs.GetInt(BEST_SCORE_KEY, 0);
    }
}
