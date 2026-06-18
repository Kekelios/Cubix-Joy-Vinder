using System.Linq;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [SerializeField] private TextMeshProUGUI texteScore;
    [SerializeField] private TextMeshProUGUI texteCoeurs;
    [SerializeField] private GameObject panneauGameOver;
    [SerializeField] private GameObject panneauVictoire;
    [SerializeField] private TextMeshProUGUI texteScoreGameOver;
    [SerializeField] private TextMeshProUGUI texteScoreVictoire;

    private const int CoeursInitiaux = 3;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void Start()
    {
        MettreAJourScore(0);
        MettreAJourCoeurs(CoeursInitiaux);
        panneauGameOver?.SetActive(false);
        panneauVictoire?.SetActive(false);
    }

    /// <summary>Met à jour le texte du score dans le HUD.</summary>
    public void MettreAJourScore(int score)
    {
        if (texteScore != null)
            texteScore.text = $"Score : {score}";
    }

    /// <summary>Met à jour l'affichage des coeurs dans le HUD.</summary>
    public void MettreAJourCoeurs(int coeurs)
    {
        if (texteCoeurs == null) return;
        int count = Mathf.Max(0, coeurs);
        texteCoeurs.text = string.Join(" ", Enumerable.Repeat("❤", count));
    }

    /// <summary>Affiche le panneau Game Over avec le score final.</summary>
    public void AfficherGameOver(int score)
    {
        panneauGameOver?.SetActive(true);
        if (texteScoreGameOver != null)
            texteScoreGameOver.text = $"Score final : {score}";
    }

    /// <summary>Affiche le panneau Victoire avec le score final.</summary>
    public void AfficherVictoire(int score)
    {
        panneauVictoire?.SetActive(true);
        if (texteScoreVictoire != null)
            texteScoreVictoire.text = $"Score final : {score}";
    }
}
