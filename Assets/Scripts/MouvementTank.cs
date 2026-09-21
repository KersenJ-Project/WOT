using UnityEngine;

public class MouvementTank : MonoBehaviour
{
    [Header("Déplacement")]
    public float vitesse = 5f;
    public float vitesseRotation = 120f;

    [Header("Missile")]
    public GameObject missilePrefab;
    public Transform pointTir;
    public float delaiEntreTirs = 0.7f;
    private float chronoTir = 0f;

    [Header("Munitions")]
    public int nombreMissiles = 3;

    void Update()
    {
        // Avancer / reculer
        float mouvement = Input.GetAxis("Vertical");

        transform.Translate(
            Vector3.up * mouvement * vitesse * Time.deltaTime,
            Space.Self
        );

        // Tourner à gauche / droite
        float rotation = Input.GetAxis("Horizontal");

        transform.Rotate(
            Vector3.forward * -rotation * vitesseRotation * Time.deltaTime
        );

        // Cooldown du tir
        chronoTir += Time.deltaTime;

        // Tirer avec Espace
        if (Input.GetKeyDown(KeyCode.Space) && chronoTir >= delaiEntreTirs && nombreMissiles > 0)
        {
            Tirer();
            chronoTir = 0f;
        }
    }

    void Tirer()
    {
        if (missilePrefab == null || pointTir == null)
        {
            Debug.LogWarning("Missile ou point de tir non configuré !");
            return;
        }

        GameObject missileObj = Instantiate(
            missilePrefab,
            pointTir.position,
            pointTir.rotation
        );

        // Empêche le missile de percuter son propre tireur dès son apparition
        Collider2D monCollider = GetComponent<Collider2D>();
        Collider2D colliderMissile = missileObj.GetComponent<Collider2D>();
        if (monCollider != null && colliderMissile != null)
        {
            Physics2D.IgnoreCollision(colliderMissile, monCollider);
        }

        nombreMissiles--;
    }

    // Appelée par MissileCollectible quand le joueur ramasse un pickup
    public void AjouterMissiles(int quantite)
    {
        nombreMissiles += quantite;
    }
}