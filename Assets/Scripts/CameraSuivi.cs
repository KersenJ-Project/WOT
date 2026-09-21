using UnityEngine;

public class CameraSuivi : MonoBehaviour
{
    [SerializeField] private Transform cible;

    private void LateUpdate()
    {
        if (cible == null) return;

        transform.position = new Vector3(
            cible.position.x,
            cible.position.y,
            transform.position.z
        );
    }
}