using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance { get; private set; }

    [SerializeField] private int coeurs = 3;
    [SerializeField] private LevelConfig config;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    /// <summary>Appelé par BulletBehavior quand un ennemi est touché.</summary>
    public void OnEnnemieDetruit(GameObject ennemi)
    {
        EnemyGrid.Instance.SupprimerEnnemi(ennemi);
        ScoreManager.Instance.AjouterPoint();

        if (EnemyGrid.Instance.NombreEnnemisRestants() == 0)
            OnVictoire();
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
    /// Affiche le panneau Victoire avec le score actuel.
    /// La navigation vers la scène suivante est entièrement gérée par VictoireUI
    /// (boutons Suivant et Quitter), sans charger directement la scène ici.
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
