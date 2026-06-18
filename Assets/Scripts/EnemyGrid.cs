using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyGrid : MonoBehaviour
{
    public static EnemyGrid Instance { get; private set; }

    [SerializeField] private LevelConfig config;
    [SerializeField] private GameObject prefabEnnemi;
    [SerializeField] private Transform conteneurEnnemis;

    private readonly List<GameObject> ennemis = new List<GameObject>();
    private float direction = 1f;
    private bool spawnTermine = false;

    private const float MargeX = 0.5f;
    private const float DelaiEntreEnnemis = 0.05f;
    private const float DelaiEntreLignes = 5f;

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
        StartCoroutine(SpawnEnnemisCoroutine());
    }

    private IEnumerator SpawnEnnemisCoroutine()
    {
        if (prefabEnnemi == null || config == null) yield break;

        float totalWidth = (config.colonnesEnnemis - 1) * config.espacementEnnemis;
        float startX = -totalWidth / 2f;

        // Démarre en haut de l'écran, calculé depuis la caméra orthographique
        float startY = Camera.main != null
            ? Camera.main.orthographicSize - 1f
            : 4f;

        Transform parent = conteneurEnnemis != null ? conteneurEnnemis : transform;

        for (int row = 0; row < config.lignesEnnemis; row++)
        {
            for (int col = 0; col < config.colonnesEnnemis; col++)
            {
                Vector3 pos = new Vector3(
                    startX + col * config.espacementEnnemis,
                    startY - row * config.espacementEnnemis,
                    0f
                );
                GameObject ennemi = Instantiate(prefabEnnemi, pos, Quaternion.identity, parent);
                ennemis.Add(ennemi);
                yield return new WaitForSeconds(DelaiEntreEnnemis);
            }
            yield return new WaitForSeconds(DelaiEntreLignes);
        }

        if (config.ennemisCanShoot)
            ActiverTireurs();

        // Les ennemis ne commencent à se déplacer qu'une fois tous apparus
        spawnTermine = true;
    }

    private void ActiverTireurs()
    {
        int nombreTireurs = Mathf.Min(config.nombreTireursParColonne, config.lignesEnnemis);

        for (int col = 0; col < config.colonnesEnnemis; col++)
        {
            for (int t = 0; t < nombreTireurs; t++)
            {
                // Les ennemis les plus bas de chaque colonne
                int ligneIndex = config.lignesEnnemis - 1 - t;
                int index = ligneIndex * config.colonnesEnnemis + col;

                if (index >= 0 && index < ennemis.Count && ennemis[index] != null)
                {
                    EnemyShooter shooter = ennemis[index].GetComponent<EnemyShooter>();
                    if (shooter != null)
                    {
                        shooter.intervalTir = config.intervalTir;
                        shooter.probabiliteTir = config.probabiliteTir;
                        shooter.enabled = true;
                    }
                }
            }
        }
    }

    private void Update()
    {
        if (!spawnTermine || conteneurEnnemis == null || config == null) return;

        conteneurEnnemis.Translate(Vector3.right * direction * config.vitesseDeplacement * Time.deltaTime);

        if (Camera.main != null)
        {
            float limiteX = Camera.main.orthographicSize * Camera.main.aspect - MargeX;

            // Vérifie uniquement le bord dans la direction de déplacement actuelle
            bool bordAtteint = direction > 0f
                ? ennemis.Any(e => e != null && e.transform.position.x > limiteX)
                : ennemis.Any(e => e != null && e.transform.position.x < -limiteX);

            if (bordAtteint)
            {
                direction *= -1f;
                conteneurEnnemis.Translate(Vector3.down * config.distanceDescente);
            }
        }
    }

    /// <summary>Retire un ennemi de la grille et le détruit.</summary>
    public void SupprimerEnnemi(GameObject ennemi)
    {
        ennemis.Remove(ennemi);
        if (ennemi != null)
            Destroy(ennemi);
    }

    /// <summary>Retourne le nombre d'ennemis restants dans la grille.</summary>
    public int NombreEnnemisRestants()
    {
        return ennemis.Count(e => e != null);
    }
}
