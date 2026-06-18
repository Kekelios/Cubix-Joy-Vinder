using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Gère les interactions du panneau Victoire affiché entre les niveaux.
/// Ce script est attaché au GameObject Victoire_Panel dans le Canvas.
/// Aucune logique de jeu ici : uniquement la navigation entre scènes.
/// </summary>
public class VictoireUI : MonoBehaviour
{
    private const string NomLevel1 = "Level1Scene";
    private const string NomLevel2 = "Level2Scene";
    private const string NomMenu   = "MenuScene";

    /// <summary>
    /// Charge le niveau suivant en conservant le score courant.
    /// - Depuis Level1Scene : charge Level2Scene (score conservé).
    /// - Depuis Level2Scene (ou tout autre niveau) : sauvegarde le meilleur
    ///   score, remet le score à 0 et charge MenuScene.
    /// </summary>
    public void SurClicSuivant()
    {
        string sceneActuelle = SceneManager.GetActiveScene().name;

        if (sceneActuelle == NomLevel1)
        {
            // Niveau 1 terminé : passage au niveau 2 avec le score conservé
            SceneManager.LoadScene(NomLevel2);
        }
        else
        {
            // Dernier niveau terminé : sauvegarde du meilleur score avant remise à 0
            ScoreManager.Instance.SauvegarderMeilleurScore();
            ScoreManager.Instance.ReinitialiserScore();
            SceneManager.LoadScene(NomMenu);
        }
    }

    /// <summary>
    /// Sauvegarde le meilleur score dans PlayerPrefs et retourne au menu principal.
    /// Le score courant n'est pas remis à 0 — il est simplement conservé dans PlayerPrefs
    /// si supérieur au meilleur score précédent.
    /// </summary>
    public void SurClicQuitter()
    {
        ScoreManager.Instance.SauvegarderMeilleurScore();
        SceneManager.LoadScene(NomMenu);
    }
}
