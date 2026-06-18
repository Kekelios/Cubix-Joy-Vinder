using UnityEngine;
using TMPro;

/// <summary>
/// Gère l'affichage de tous les éléments UI du HUD et des panneaux de fin de partie.
/// Un seul script, une seule responsabilité : l'interface visuelle.
/// Aucune logique de jeu ici.
/// </summary>
public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [SerializeField] private TextMeshProUGUI texteScore;

    /// <summary>
    /// Tableau des 3 blocs visuels représentant les cœurs du joueur.
    /// Indice 0 = Heart1 (gauche), 1 = Heart2 (milieu), 2 = Heart3 (droite).
    /// </summary>
    [SerializeField] private GameObject[] blocsCoeurs;

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

    /// <summary>
    /// Met à jour les blocs cœurs visuels selon le nombre de cœurs restants.
    /// Les blocs sont masqués de droite à gauche : 3 cœurs → tous visibles,
    /// 2 cœurs → Heart3 caché, 1 cœur → Heart2 et Heart3 cachés, 0 → tous cachés.
    /// </summary>
    public void MettreAJourCoeurs(int coeurs)
    {
        if (blocsCoeurs == null) return;

        int count = Mathf.Max(0, coeurs);

        for (int i = 0; i < blocsCoeurs.Length; i++)
        {
            if (blocsCoeurs[i] != null)
                blocsCoeurs[i].SetActive(i < count);
        }
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
