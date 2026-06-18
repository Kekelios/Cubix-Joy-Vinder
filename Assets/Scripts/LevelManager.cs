using UnityEngine;

/// <summary>
/// Orchestre la logique centrale du niveau : vie du joueur, destruction des ennemis,
/// drop de coeurs et transitions vers victoire/défaite.
/// Aucune logique UI ici — tout passe par UIManager.
/// </summary>
public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance { get; private set; }

    [SerializeField] private int coeurs    = 3;
    [SerializeField] private int coeursMax = 3;
    [SerializeField] private LevelConfig config;

    [Header("Drop de cœur")]
    /// <summary>Prefab du pickup coeur laissé par les ennemis vaincus.</summary>
    [SerializeField] private GameObject prefabCoeur;

    /// <summary>Probabilité (0 à 1) qu'un ennemi vaincu laisse tomber un coeur.</summary>
    [SerializeField] [Range(0f, 1f)] private float probabiliteDropCoeur = 0.2f;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    /// <summary>
    /// Appelé par BulletBehavior quand un ennemi est touché.
    /// La position de l'ennemi est sauvegardée avant sa destruction pour le drop.
    /// </summary>
    public void OnEnnemieDetruit(GameObject ennemi)
    {
        // Sauvegarde la position avant destruction (SupprimerEnnemi appelle Destroy)
        Vector3 positionDrop = ennemi.transform.position;

        EnemyGrid.Instance.SupprimerEnnemi(ennemi);
        ScoreManager.Instance.AjouterPoint();

        TenterDropCoeur(positionDrop);

        if (EnemyGrid.Instance.NombreEnnemisRestants() == 0)
            OnVictoire();
    }

    /// <summary>
    /// Fait apparaître un pickup coeur à la position donnée selon la probabilité configurée.
    /// </summary>
    private void TenterDropCoeur(Vector3 position)
    {
        if (prefabCoeur != null && Random.value < probabiliteDropCoeur)
            Instantiate(prefabCoeur, position, Quaternion.identity);
    }

    /// <summary>
    /// Appelé par HeartPickup quand le joueur ramasse un coeur.
    /// Le nombre de coeurs est incrémenté sans dépasser coeursMax.
    /// </summary>
    public void OnRamassageCoeur()
    {
        coeurs = Mathf.Min(coeurs + 1, coeursMax);
        UIManager.Instance.MettreAJourCoeurs(coeurs);
    }

    /// <summary>Appelé quand le joueur subit des dégâts.</summary>
    public void OnJoueurTouche()
    {
        coeurs--;
        UIManager.Instance.MettreAJourCoeurs(coeurs);
        if (coeurs <= 0)
            OnDefaite();
    }

    /// <summary>Appelé par BottomWall quand un ennemi atteint le bas de l'écran.</summary>
    public void OnEnnemisAtteintBas(GameObject ennemi)
    {
        EnemyGrid.Instance.SupprimerEnnemi(ennemi);

        if (EnemyGrid.Instance.NombreEnnemisRestants() == 0)
            OnVictoire();
        else
            OnJoueurTouche();
    }

    /// <summary>
    /// Affiche le panneau Victoire. La navigation est gérée par VictoireUI.
    /// </summary>
    public void OnVictoire()
    {
        UIManager.Instance.AfficherVictoire(ScoreManager.Instance.ObtenirScore());
    }

    /// <summary>Sauvegarde le score et affiche le panneau Game Over.</summary>
    public void OnDefaite()
    {
        ScoreManager.Instance.SauvegarderMeilleurScore();
        UIManager.Instance.AfficherGameOver(ScoreManager.Instance.ObtenirScore());
    }
}
