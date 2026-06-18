using UnityEngine;

/// <summary>
/// Orchestre la logique centrale du niveau : vie du joueur, destruction des ennemis,
/// drop de coeurs et transitions vers victoire/défaite.
/// Aucune logique UI ici — tout passe par UIManager.
/// </summary>
public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance { get; private set; }

    [SerializeField] private int coeurs = 3;
    [SerializeField] private int coeursMax = 3;
    [SerializeField] private LevelConfig config;

    [Header("FX de mort")]
    [SerializeField] private GameObject prefabFXMort;

    [Header("Drop de cœur")]
    [SerializeField] private GameObject prefabCoeur;

    [SerializeField][Range(0f, 1f)] private float probabiliteDropCoeur = 0.2f;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public void OnEnnemieDetruit(GameObject ennemi)
    {
        Vector3 positionDrop = ennemi.transform.position;

        EnemyShooter shooter = ennemi.GetComponent<EnemyShooter>();
        int points = (shooter != null && shooter.enabled) ? 3 : 1;

        SpawnerFXMort(positionDrop);
        EnemyGrid.Instance.SupprimerEnnemi(ennemi);
        ScoreManager.Instance.AjouterPoints(points);

        TenterDropCoeur(positionDrop);

        if (EnemyGrid.Instance.NombreEnnemisRestants() == 0)
            OnVictoire();
    }

    private void SpawnerFXMort(Vector3 position)
    {
        if (prefabFXMort != null)
            Instantiate(prefabFXMort, position, Quaternion.identity);
    }

    private void TenterDropCoeur(Vector3 position)
    {
        if (prefabCoeur != null && Random.value < probabiliteDropCoeur)
            Instantiate(prefabCoeur, position, Quaternion.identity);
    }

    public void OnRamassageCoeur()
    {
        coeurs = Mathf.Min(coeurs + 1, coeursMax);
        UIManager.Instance.MettreAJourCoeurs(coeurs);
    }

    public void OnJoueurTouche()
    {
        coeurs--;
        UIManager.Instance.MettreAJourCoeurs(coeurs);
        if (coeurs <= 0)
            OnDefaite();
    }

    public void OnEnnemisAtteintBas(GameObject ennemi)
    {
        EnemyGrid.Instance.SupprimerEnnemi(ennemi);

        if (EnemyGrid.Instance.NombreEnnemisRestants() == 0)
            OnVictoire();
        else
            OnJoueurTouche();
    }

    public void OnVictoire()
    {
        UIManager.Instance.AfficherVictoire(ScoreManager.Instance.ObtenirScore());
    }

    /// <summary>Sauvegarde le score et affiche le panneau Game Over.</summary>
    public void OnDefaite()
    {
        Time.timeScale = 0f; // ← AJOUT : fige tout le jeu
        ScoreManager.Instance.SauvegarderMeilleurScore();
        UIManager.Instance.AfficherGameOver(ScoreManager.Instance.ObtenirScore());
    }
}