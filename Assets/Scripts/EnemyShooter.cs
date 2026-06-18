using System.Collections;
using UnityEngine;

public class EnemyShooter : MonoBehaviour
{
    [SerializeField] public float intervalTir = 3f;
    [SerializeField] public float probabiliteTir = 0.3f;

    private void Awake()
    {
        // Disabled by default; EnemyGrid enables it at runtime for shooter rows
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

    /// <summary>Boucle de tir : attend intervalTir secondes, puis tire selon la probabilité.</summary>
    private IEnumerator BoucleDetir()
    {
        while (true)
        {
            yield return new WaitForSeconds(intervalTir);
            if (Random.value < probabiliteTir)
                BulletManager.Instance.TirerProjectileEnnemi(transform.position);
        }
    }
}
