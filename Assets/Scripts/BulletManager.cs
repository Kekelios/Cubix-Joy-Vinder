using UnityEngine;

public class BulletManager : MonoBehaviour
{
    public static BulletManager Instance { get; private set; }

    [SerializeField] private GameObject prefabProjectileJoueur;
    [SerializeField] private GameObject prefabProjectileEnnemi;
    [SerializeField] private Transform conteneurProjectiles;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    /// <summary>Instancie un projectile joueur à la position donnée.</summary>
    public void TirerProjectileJoueur(Vector3 position)
    {
        if (prefabProjectileJoueur != null)
            Instantiate(prefabProjectileJoueur, position, Quaternion.identity, conteneurProjectiles);
    }

    /// <summary>Instancie un projectile ennemi à la position donnée.</summary>
    public void TirerProjectileEnnemi(Vector3 position)
    {
        if (prefabProjectileEnnemi != null)
            Instantiate(prefabProjectileEnnemi, position, Quaternion.identity, conteneurProjectiles);
    }
}
