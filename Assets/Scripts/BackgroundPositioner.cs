using UnityEngine;

/// <summary>
/// Redimensionne et positionne automatiquement le BackgroundPlane
/// pour couvrir exactement le viewport de la caméra orthographique.
/// Formule : hauteur = orthographicSize * 2 ; largeur = hauteur * aspect.
/// </summary>
[RequireComponent(typeof(Renderer))]
public class BackgroundPositioner : MonoBehaviour
{
    [SerializeField] private Camera cameraJeu;

    private void Awake()
    {
        // Utilise la caméra principale si aucune n'est assignée dans l'Inspector
        if (cameraJeu == null)
            cameraJeu = Camera.main;

        AppliquerDimensionsCarte();
    }

    /// <summary>
    /// Calcule la taille du viewport en unités monde et applique
    /// la position (Z = 10, derrière tous les objets de jeu) et
    /// l'échelle correspondante au Transform du BackgroundPlane.
    /// </summary>
    private void AppliquerDimensionsCarte()
    {
        if (cameraJeu == null || !cameraJeu.orthographic)
        {
            Debug.LogWarning("[BackgroundPositioner] Aucune caméra orthographique trouvée. " +
                             "Assignez la caméra dans l'Inspector.");
            return;
        }

        // Calcul des dimensions en unités monde
        float hauteur = cameraJeu.orthographicSize * 2f;
        float largeur = hauteur * cameraJeu.aspect;

        // Application de la position et de l'échelle
        transform.position   = new Vector3(0f, 0f, 10f);
        transform.rotation   = Quaternion.identity;
        transform.localScale = new Vector3(largeur, hauteur, 1f);
    }
}
