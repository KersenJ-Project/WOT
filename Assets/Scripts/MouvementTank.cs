using UnityEngine;

public class MouvementTank : MonoBehaviour
{
    public float vitesse = 5f;
    public float vitesseRotation = 120f;

    public GameObject missilePrefab;
    public Transform pointTir;
    public float delaiEntreTirs = 0.7f;
    private float chronoTir = 0f;

    public int nombreMissiles = 3;

    public int vieMax = 5;
    private int vieActuelle;

    public GameObject prefabExplosion;

    void Start()
    {
        vieActuelle = vieMax;

        // Affiche les valeurs de départ dès le lancement de la partie
        if (GestionnaireUI.instance != null)
        {
            GestionnaireUI.instance.MettreAJourMissiles(nombreMissiles);
            GestionnaireUI.instance.MettreAJourVie(vieActuelle);
        }
    }

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

        if (GestionnaireUI.instance != null)
        {
            GestionnaireUI.instance.MettreAJourMissiles(nombreMissiles);
        }
    }

    // Appelée par MissileCollectible quand le joueur ramasse un pickup
    public void AjouterMissiles(int quantite)
    {
        nombreMissiles += quantite;

        if (GestionnaireUI.instance != null)
        {
            GestionnaireUI.instance.MettreAJourMissiles(nombreMissiles);
        }
    }

    // Appelée par Missile.cs quand un missile ennemi touche le joueur
    public void SubirDegats(int degats)
    {
        vieActuelle -= degats;
        if (vieActuelle < 0) vieActuelle = 0;

        if (GestionnaireUI.instance != null)
        {
            GestionnaireUI.instance.MettreAJourVie(vieActuelle);
        }

        if (vieActuelle <= 0)
        {
            Mourir();
        }
    }

    private void Mourir()
    {
        Debug.Log("Le joueur est mort !");

        if (prefabExplosion != null)
        {
            Instantiate(prefabExplosion, transform.position, Quaternion.identity);
        }

        // Détruit complètement le tank joueur. Remplaçable plus tard par un écran Game Over.
        Destroy(gameObject);
    }
}