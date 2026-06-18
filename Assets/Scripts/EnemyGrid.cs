using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// Gère le spawn de la grille d'ennemis, leur déplacement latéral et leur destruction.
/// En Level2, le spawn est découpé en deux vagues séparées par delaiEntreVagues :
/// la première moitié apparaît, le joueur peut commencer à jouer, puis la seconde
/// moitié spawn depuis le haut de l'écran.
/// </summary>
public class EnemyGrid : MonoBehaviour
{
    public static EnemyGrid Instance { get; private set; }

    [SerializeField] private LevelConfig config;

    /// <summary>Prefab de l'ennemi de base (Sphère rose).</summary>
    [SerializeField] private GameObject prefabEnnemi;

    /// <summary>Prefab de l'ennemi tireur (Cube rouge foncé), utilisé en Level2.</summary>
    [SerializeField] private GameObject prefabEnnemiTireur;

    /// <summary>
    /// Active le mode Level2 : spawn en deux vagues + quart des ennemis tireurs.
    /// À cocher uniquement dans l'Inspector de la Level2Scene.
    /// </summary>
    [SerializeField] private bool estNiveau2 = false;

    /// <summary>Proportion d'ennemis tireurs en Level2 (0 = aucun, 1 = tous). Défaut : 0.25.</summary>
    [SerializeField] [Range(0f, 1f)] private float proportionTireurs = 0.25f;

    /// <summary>
    /// Délai en secondes entre la vague 1 et la vague 2 en Level2.
    /// Donne au joueur le temps de commencer à se battre avant le second arrivage.
    /// </summary>
    [SerializeField] private float delaiEntreVagues = 12f;

    [SerializeField] private Transform conteneurEnnemis;

    private readonly List<GameObject> ennemis = new List<GameObject>();
    private float direction = 1f;
    private bool spawnTermine = false;

    private const float MargeX = 0.5f;
    private const float DelaiEntreEnnemis = 0.05f;
    private const float DelaiEntreLignes = 0.3f;

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
        float startX    = -totalWidth / 2f;
        Transform parent = conteneurEnnemis != null ? conteneurEnnemis : transform;

        // ── Pré-sélection aléatoire des indices tireurs (Level2 uniquement) ────────
        var indicesTireurs = new HashSet<int>();
        if (estNiveau2 && prefabEnnemiTireur != null)
        {
            int total        = config.lignesEnnemis * config.colonnesEnnemis;
            int nbrTireurs   = Mathf.Max(1, Mathf.RoundToInt(total * proportionTireurs));
            var selection    = Enumerable.Range(0, total)
                                         .OrderBy(_ => Random.value)
                                         .Take(nbrTireurs);
            foreach (int idx in selection)
                indicesTireurs.Add(idx);
        }

        // ── Calcul du découpage en vagues ─────────────────────────────────────────
        // En Level2 : moitié des lignes par vague. En Level1 : une seule passe.
        int lignesVague1 = estNiveau2
            ? Mathf.Max(1, config.lignesEnnemis / 2)
            : config.lignesEnnemis;

        int spawnIndex = 0;

        // ── VAGUE 1 ───────────────────────────────────────────────────────────────
        float startY = Camera.main != null ? Camera.main.orthographicSize - 1f : 4f;

        for (int row = 0; row < lignesVague1; row++)
        {
            for (int col = 0; col < config.colonnesEnnemis; col++)
            {
                Vector3 pos = new Vector3(
                    startX + col * config.espacementEnnemis,
                    startY - row * config.espacementEnnemis,
                    0f
                );
                SpawnEnnemi(pos, parent, indicesTireurs.Contains(spawnIndex));
                spawnIndex++;
                yield return new WaitForSeconds(DelaiEntreEnnemis);
            }
            yield return new WaitForSeconds(DelaiEntreLignes);
        }

        // La vague 1 est en place : les ennemis commencent à se déplacer
        spawnTermine = true;

        // ── PAUSE + VAGUE 2 (Level2 uniquement) ───────────────────────────────────
        if (estNiveau2 && lignesVague1 < config.lignesEnnemis)
        {
            yield return new WaitForSeconds(delaiEntreVagues);

            // La vague 2 spawn depuis le haut de l'écran (même startY),
            // indépendamment de la position actuelle du conteneur.
            float startYVague2 = Camera.main != null ? Camera.main.orthographicSize - 1f : 4f;
            int lignesVague2   = config.lignesEnnemis - lignesVague1;

            for (int row = 0; row < lignesVague2; row++)
            {
                for (int col = 0; col < config.colonnesEnnemis; col++)
                {
                    Vector3 pos = new Vector3(
                        startX + col * config.espacementEnnemis,
                        startYVague2 - row * config.espacementEnnemis,
                        0f
                    );
                    SpawnEnnemi(pos, parent, indicesTireurs.Contains(spawnIndex));
                    spawnIndex++;
                    yield return new WaitForSeconds(DelaiEntreEnnemis);
                }
                yield return new WaitForSeconds(DelaiEntreLignes);
            }
        }

        // Level1 : activation des tireurs via la config (comportement original)
        if (config.ennemisCanShoot && !estNiveau2)
            ActiverTireursParConfig();
    }

    /// <summary>
    /// Instancie un ennemi à la position donnée et active son script de tir
    /// immédiatement s'il est désigné comme tireur.
    /// </summary>
    private void SpawnEnnemi(Vector3 positionMonde, Transform parent, bool estTireur)
    {
        GameObject prefabChoisi = estTireur ? prefabEnnemiTireur : prefabEnnemi;
        GameObject ennemi       = Instantiate(prefabChoisi, positionMonde, Quaternion.identity, parent);
        ennemis.Add(ennemi);

        if (estTireur)
        {
            EnemyShooter shooter = ennemi.GetComponent<EnemyShooter>();
            if (shooter != null)
                shooter.enabled = true;
        }
    }

    /// <summary>
    /// Activation des tireurs pour Level1 via la config (comportement original).
    /// Active les ennemis les plus bas de chaque colonne selon nombreTireursParColonne.
    /// </summary>
    private void ActiverTireursParConfig()
    {
        int nombreTireurs = Mathf.Min(config.nombreTireursParColonne, config.lignesEnnemis);

        for (int col = 0; col < config.colonnesEnnemis; col++)
        {
            for (int t = 0; t < nombreTireurs; t++)
            {
                int ligneIndex = config.lignesEnnemis - 1 - t;
                int index      = ligneIndex * config.colonnesEnnemis + col;

                if (index >= 0 && index < ennemis.Count && ennemis[index] != null)
                {
                    EnemyShooter shooter = ennemis[index].GetComponent<EnemyShooter>();
                    if (shooter != null)
                    {
                        shooter.intervalTir       = config.intervalTir;
                        shooter.variationInterval = 0.5f;
                        shooter.enabled           = true;
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
