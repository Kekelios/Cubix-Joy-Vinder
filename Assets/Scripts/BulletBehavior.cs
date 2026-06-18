using UnityEngine;

public class BulletBehavior : MonoBehaviour
{
    [SerializeField] private float vitesse = 12f;
    [SerializeField] private bool estProjectileEnnemi = false;

    private bool aDejaFrappe = false;

    private void Update()
    {
        float sens = estProjectileEnnemi ? -1f : 1f;
        transform.Translate(Vector3.up * vitesse * sens * Time.deltaTime);

        if (Camera.main != null)
        {
            float limiteY = Camera.main.orthographicSize + 2f;
            if (Mathf.Abs(transform.position.y) > limiteY)
                Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider autre)
    {
        if (aDejaFrappe) return;

        if (!estProjectileEnnemi && autre.CompareTag("Enemy"))
        {
            aDejaFrappe = true;
            LevelManager.Instance.OnEnnemieDetruit(autre.gameObject);
            Destroy(gameObject);
        }
        else if (estProjectileEnnemi && autre.CompareTag("Player"))
        {
            aDejaFrappe = true;
            Destroy(gameObject);
        }
    }
}
