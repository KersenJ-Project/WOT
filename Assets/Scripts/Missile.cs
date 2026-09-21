using UnityEngine;
using UnityEngine.Tilemaps;

public class Missile : MonoBehaviour
{
    public float vitesse = 10f;
    public float tempsDeVie = 3f;

    void Start()
    {
        Destroy(gameObject, tempsDeVie);
    }

    void Update()
    {
        transform.Translate(Vector3.up * vitesse * Time.deltaTime, Space.Self);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        GérerImpact(collision.gameObject, collision.GetContact(0).point);
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        GérerImpact(collider.gameObject, transform.position);
    }

    private void GérerImpact(GameObject cible, Vector3 pointImpact)
    {
        Tilemap tilemap = cible.GetComponent<Tilemap>();
        if (tilemap != null && cible.CompareTag("Obstacle"))
        {
            Vector3Int cellPosition = tilemap.WorldToCell(pointImpact);
            tilemap.SetTile(cellPosition, null);
            Destroy(gameObject);
            return;
        }

        if (cible.CompareTag("Enemy"))
        {
            Destroy(cible);
            Destroy(gameObject);
        }
    }
}