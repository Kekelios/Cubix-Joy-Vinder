using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Gère le déplacement du joueur, le tir et la réception des dégâts.
/// Après un coup encaissé, le joueur est invulnérable pendant dureeInvulnerabilite
/// secondes et clignote visuellement pour signaler cet état.
/// </summary>
public class PlayerController : MonoBehaviour
{
    [SerializeField] private float vitesseDeplacement = 5f;
    [SerializeField] private float cooldownTir        = 0.4f;
    [SerializeField] private GameObject prefabProjectile;
    [SerializeField] private Transform pointDeTir;

    /// <summary>Durée d'invulnérabilité après un coup, en secondes.</summary>
    [SerializeField] private float dureeInvulnerabilite = 1.5f;

    /// <summary>Temps entre chaque toggle du renderer pendant le clignotement.</summary>
    [SerializeField] private float frequenceClignotement = 0.1f;

    private float limiteX;
    private float tempsCooldown;
    private float positionY;

    private bool estInvulnerable = false;
    private Renderer rendererJoueur;

    private void Start()
    {
        limiteX = Camera.main != null
            ? Camera.main.orthographicSize * Camera.main.aspect - 0.5f
            : 8f;

        positionY = transform.position.y;
        rendererJoueur = GetComponentInChildren<Renderer>();
    }

    private void Update()
    {
        var keyboard = Keyboard.current;
        if (keyboard == null) return;

        float h = 0f;
        if (keyboard.leftArrowKey.isPressed || keyboard.aKey.isPressed || keyboard.qKey.isPressed) h = -1f;
        if (keyboard.rightArrowKey.isPressed || keyboard.dKey.isPressed) h = 1f;

        Vector3 pos = transform.position;
        pos.x = Mathf.Clamp(pos.x + h * vitesseDeplacement * Time.deltaTime, -limiteX, limiteX);
        pos.y = positionY;
        transform.position = pos;

        if (tempsCooldown > 0f)
            tempsCooldown -= Time.deltaTime;

        if (keyboard.spaceKey.wasPressedThisFrame && tempsCooldown <= 0f)
        {
            Transform spawnPoint = pointDeTir != null ? pointDeTir : transform;
            BulletManager.Instance.TirerProjectileJoueur(spawnPoint.position);
            tempsCooldown = cooldownTir;
        }
    }

    private void OnTriggerEnter(Collider autre)
    {
        if (estInvulnerable) return;

        if (autre.CompareTag("EnemyBullet"))
        {
            Destroy(autre.gameObject);
            SubirUnCoup();
        }
        else if (autre.CompareTag("Enemy"))
        {
            SubirUnCoup();
        }
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
    }

    /// <summary>
    /// Encaisse un coup : notifie LevelManager, puis déclenche
    /// l'invulnérabilité avec clignotement visuel.
    /// </summary>
    private void SubirUnCoup()
    {
        LevelManager.Instance.OnJoueurTouche();
        StartCoroutine(ClignoterInvulnerabilite());
    }

    /// <summary>
    /// Rend le joueur invulnérable pendant dureeInvulnerabilite secondes.
    /// Le renderer clignote à fréquenceClignotement pour signaler l'état au joueur.
    /// Le renderer est garanti visible à la fin de la coroutine.
    /// </summary>
    private IEnumerator ClignoterInvulnerabilite()
    {
        estInvulnerable = true;
        float tempsRestant = dureeInvulnerabilite;

        while (tempsRestant > 0f)
        {
            if (rendererJoueur != null)
                rendererJoueur.enabled = !rendererJoueur.enabled;

            yield return new WaitForSeconds(frequenceClignotement);
            tempsRestant -= frequenceClignotement;
        }

        if (rendererJoueur != null)
            rendererJoueur.enabled = true;

        estInvulnerable = false;
    }
}
