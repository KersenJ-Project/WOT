using UnityEngine;

public class TankBoss : MonoBehaviour
{
    public Transform[] pointsPatrouille;
    public float vitessePatrouille = 2f;
    public float distanceArret = 0.2f;

    public float rayonDetection = 14f;
    public string tagJoueur = "Player";
    private Transform cibleJoueur;
    private bool joueurDetecte = false;

    public float vitesseRotation = 120f;

    public GameObject missilePrefab;
    public Transform pointTir;
    public float delaiEntreTirs = 2f;
    private float chronoTir = 0f;

    public GameObject prefabExplosion;

    public int vieMax = 5;
    private int vieActuelle;

    private int indexPointActuel = 0;

    void Start()
    {
        vieActuelle = vieMax;
    }

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

    // Cherche le joueur dans un rayon autour du boss
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

    // Déplace le boss d'un point de patrouille au suivant
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

    // Tourne progressivement le boss vers une position cible
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

    // Appelée par Missile.cs à chaque impact. Le boss ne meurt qu'après 5 coups.
    public void SubirDegats(int degats)
    {
        vieActuelle -= degats;
        Debug.Log(vieActuelle);
        if (vieActuelle <= 0)
        {
            Destroy(gameObject);
        }
    }

    // Affiche le rayon de détection dans l'éditeur (visible seulement quand l'objet est sélectionné)
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, rayonDetection);
    }

    // Prévient le gestionnaire UI et déclenche l'explosion quand le boss est détruit (vie à 0)
    private void OnDestroy()
    {
        if (prefabExplosion != null)
        {
            Instantiate(prefabExplosion, transform.position, Quaternion.identity);
        }

        if (GestionnaireUI.instance != null)
        {
            GestionnaireUI.instance.EnnemiDetruit();
        }
    }
}