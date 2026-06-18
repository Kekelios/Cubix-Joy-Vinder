using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float vitesseDeplacement = 5f;
    [SerializeField] private float cooldownTir = 0.4f;
    [SerializeField] private GameObject prefabProjectile;
    [SerializeField] private Transform pointDeTir;

    private float limiteX;
    private float tempsCooldown;
    private float positionY;

    private void Start()
    {
        limiteX = Camera.main != null
            ? Camera.main.orthographicSize * Camera.main.aspect - 0.5f
            : 8f;

        // Verrouille la position Y initiale du joueur pour tout le jeu
        positionY = transform.position.y;
    }

    private void Update()
    {
        var keyboard = Keyboard.current;
        if (keyboard == null) return;

        // Supporte AZERTY (Q/D) et QWERTY (A/D) + flèches directionnelles
        float h = 0f;
        if (keyboard.leftArrowKey.isPressed || keyboard.aKey.isPressed || keyboard.qKey.isPressed) h = -1f;
        if (keyboard.rightArrowKey.isPressed || keyboard.dKey.isPressed) h = 1f;

        Vector3 pos = transform.position;
        pos.x = Mathf.Clamp(pos.x + h * vitesseDeplacement * Time.deltaTime, -limiteX, limiteX);
        pos.y = positionY; // Y fixe — le joueur ne bouge que sur l'axe X
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
        if (autre.CompareTag("EnemyBullet"))
        {
            Destroy(autre.gameObject);
            LevelManager.Instance.OnJoueurTouche();
        }
        else if (autre.CompareTag("Enemy"))
        {
            LevelManager.Instance.OnJoueurTouche();
        }
    }
}
