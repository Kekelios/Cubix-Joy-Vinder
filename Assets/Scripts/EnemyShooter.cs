using System.Collections;
using UnityEngine;

/// <summary>
/// Fait tirer un projectile vers le bas à intervalles réguliers.
/// Chaque tireur démarre avec un décalage initial aléatoire :
/// ils ne tirent donc jamais tous en même temps.
/// Désactivé par défaut ; EnemyGrid l'active pour les ennemis sélectionnés.
/// </summary>
public class EnemyShooter : MonoBehaviour
{
    /// <summary>Durée centrale entre deux tirs, en secondes.</summary>
    [SerializeField] public float intervalTir = 2f;

    /// <summary>
    /// Variation aléatoire appliquée à chaque intervalle (±variationInterval).
    /// Crée un rythme irrégulier entre les différents tireurs.
    /// </summary>
    [SerializeField] public float variationInterval = 0.5f;

    private void Awake()
    {
        // Désactivé par défaut : seul EnemyGrid l'active sur les ennemis tireurs
        enabled = false;
    }

    private void OnEnable()
    {
        StartCoroutine(BoucleDetir());
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }

    /// <summary>
    /// Attend un décalage initial aléatoire (entre 0 et intervalTir),
    /// puis tire indéfiniment avec un intervalle de (intervalTir ± variationInterval).
    /// Ce décalage initial empêche tous les tireurs de tirer au même moment.
    /// </summary>
    private IEnumerator BoucleDetir()
    {
        // Décalage initial unique par ennemi — désynchronise la grille entière
        yield return new WaitForSeconds(Random.Range(0f, intervalTir));

        while (true)
        {
            float attente = intervalTir + Random.Range(-variationInterval, variationInterval);
            yield return new WaitForSeconds(Mathf.Max(0.3f, attente));

            if (BulletManager.Instance != null)
                BulletManager.Instance.TirerProjectileEnnemi(transform.position);
        }
    }
}
