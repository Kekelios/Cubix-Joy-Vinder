using UnityEngine;

/// <summary>
/// Coeur laissé par un ennemi vaincu. Tombe vers le bas à vitesse constante.
/// Si le joueur le touche, il regagne un cœur (plafonné au maximum).
/// Se détruit automatiquement en sortant de l'écran par le bas.
/// </summary>
[RequireComponent(typeof(Collider))]
public class HeartPickup : MonoBehaviour
{
    /// <summary>Vitesse de chute vers le bas, en unités/seconde.</summary>
    [SerializeField] private float vitesseChute = 3f;

    private void Update()
    {
        transform.Translate(Vector3.down * vitesseChute * Time.deltaTime);

        // Auto-destruction quand le pickup sort de l'écran par le bas
        if (Camera.main != null)
        {
            float limiteY = Camera.main.orthographicSize + 2f;
            if (transform.position.y < -limiteY)
                Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider autre)
    {
        if (autre.CompareTag("Player"))
        {
            LevelManager.Instance.OnRamassageCoeur();
            Destroy(gameObject);
        }
    }
}
