using UnityEngine;

public class TankEnemy : MonoBehaviour
{
    public Transform[] pointsPatrouille;
    public float vitessePatrouille = 2f;
    public float distanceArret = 0.2f;

    public float rayonDetection = 8f;
    public string tagJoueur = "Player";
    private Transform cibleJoueur;
    private bool joueurDetecte = false;

    public float vitesseRotation = 120f;

    public GameObject missilePrefab;
    public Transform pointTir;
    public float delaiEntreTirs = 2f;
    private float chronoTir = 0f;

    private int indexPointActuel = 0;

    void Update()
    {
        DetecterJoueur();

        if (joueurDetecte && cibleJoueur != null)
        {
            ViserEtTirer();
        }
        else
        {
            Patrouiller();
        }
    }

    // Cherche le joueur dans un rayon autour du tank ennemi
    void DetecterJoueur()
    {
        Collider2D[] resultats = Physics2D.OverlapCircleAll(transform.position, rayonDetection);
        joueurDetecte = false;

        foreach (Collider2D col in resultats)
        {
            if (col.CompareTag(tagJoueur))
            {
                cibleJoueur = col.transform;
                joueurDetecte = true;
                break;
            }
        }
    }

    // Déplace le tank d'un point de patrouille au suivant
    void Patrouiller()
    {
        if (pointsPatrouille == null || pointsPatrouille.Length == 0) return;

        Transform pointActuel = pointsPatrouille[indexPointActuel];

        RotationVers(pointActuel.position);
        transform.position = Vector3.MoveTowards(
            transform.position,
            pointActuel.position,
            vitessePatrouille * Time.deltaTime
        );

        if (Vector3.Distance(transform.position, pointActuel.position) < distanceArret)
        {
            indexPointActuel = (indexPointActuel + 1) % pointsPatrouille.Length;
        }
    }

    // Vise le joueur et tire à intervalle régulier
    void ViserEtTirer()
    {
        RotationVers(cibleJoueur.position);

        chronoTir += Time.deltaTime;
        if (chronoTir >= delaiEntreTirs)
        {
            Tirer();
            chronoTir = 0f;
        }
    }

    // Tourne progressivement le tank vers une position cible
    void RotationVers(Vector3 positionCible)
    {
        Vector3 direction = positionCible - transform.position;
        if (direction.sqrMagnitude < 0.0001f) return;

        float angleCible = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
        Quaternion rotationCible = Quaternion.Euler(0, 0, angleCible);

        transform.rotation = Quaternion.RotateTowards(
            transform.rotation,
            rotationCible,
            vitesseRotation * Time.deltaTime
        );
    }

    void Tirer()
    {
        if (missilePrefab == null || pointTir == null) return;

        GameObject missileObj = Instantiate(missilePrefab, pointTir.position, pointTir.rotation);

        // Empêche le missile de percuter son propre tireur dès son apparition
        Collider2D monCollider = GetComponent<Collider2D>();
        Collider2D colliderMissile = missileObj.GetComponent<Collider2D>();
        if (monCollider != null && colliderMissile != null)
        {
            Physics2D.IgnoreCollision(colliderMissile, monCollider);
        }
    }

    // Affiche le rayon de détection dans l'éditeur (visible seulement quand l'objet est sélectionné)
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, rayonDetection);
    }

    // Prévient le gestionnaire UI quand ce tank ennemi est détruit (par un missile, etc.)
    private void OnDestroy()
    {
        if (GestionnaireUI.instance != null)
        {
            GestionnaireUI.instance.EnnemiDetruit();
        }
    }
}