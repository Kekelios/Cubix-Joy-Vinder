using UnityEngine;

public class BottomWall : MonoBehaviour
{
    private void OnTriggerEnter(Collider autre)
    {
        if (autre.CompareTag("Enemy"))
            LevelManager.Instance.OnEnnemisAtteintBas(autre.gameObject);
    }
}
