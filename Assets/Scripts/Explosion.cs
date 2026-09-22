using UnityEngine;

public class Explosion : MonoBehaviour
{
    public float duree = 1.5f;

    void Start()
    {
        Destroy(gameObject, duree);
    }
}