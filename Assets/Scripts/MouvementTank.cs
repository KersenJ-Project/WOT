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

    public bool tirDouble = false;
    public float ecartTirDouble = 0.3f;

    public int vieMax = 5;
    private int vieActuelle;

    public GameObject prefabExplosion;

    public string tagEnnemi = "Enemy";
    public float forceRebond = 1f;

    void Start()
    {
        // Place le tank à la position de la porte de spawn du niveau
        if (GestionnaireUI.instance != null)
        {
            transform.position = GestionnaireUI.instance.ObtenirPositionSpawn();
        }

        // Si on arrive d'un niveau précédent (GestionnaireJeu a des valeurs sauvegardées),
        // on reprend la vie et les missiles de fin de niveau précédent (+3 missiles déjà inclus)
        if (GestionnaireJeu.instance != null && GestionnaireJeu.instance.vieSauvegardee >= 0)
        {
            vieActuelle = GestionnaireJeu.instance.vieSauvegardee;
            nombreMissiles = GestionnaireJeu.instance.missilesSauvegardes;
        }
        else
        {
            // Premier niveau : valeurs par défaut définies dans l'Inspector
            vieActuelle = vieMax;
        }

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

        if (tirDouble)
        {
            // Deux missiles décalés de part et d'autre du point de tir
            InstancierMissile(pointTir.position + transform.right * ecartTirDouble);
            InstancierMissile(pointTir.position - transform.right * ecartTirDouble);
        }
        else
        {
            InstancierMissile(pointTir.position);
        }

        nombreMissiles--;

        if (GestionnaireUI.instance != null)
        {
            GestionnaireUI.instance.MettreAJourMissiles(nombreMissiles);
        }
    }

    private void InstancierMissile(Vector3 position)
    {
        GameObject missileObj = Instantiate(
            missilePrefab,
            position,
            pointTir.rotation
        );

        // Empêche le missile de percuter son propre tireur dès son apparition
        Collider2D monCollider = GetComponent<Collider2D>();
        Collider2D colliderMissile = missileObj.GetComponent<Collider2D>();
        if (monCollider != null && colliderMissile != null)
        {
            Physics2D.IgnoreCollision(colliderMissile, monCollider);
        }
    }

    // Appelée par TirDoubleCollectible quand le joueur ramasse ce pickup
    public void ActiverTirDouble()
    {
        tirDouble = true;
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

    // Appelée par CoeurCollectible quand le joueur ramasse un cœur
    public void Soigner(int quantite)
    {
        vieActuelle += quantite;
        if (vieActuelle > vieMax) vieActuelle = vieMax;

        if (GestionnaireUI.instance != null)
        {
            GestionnaireUI.instance.MettreAJourVie(vieActuelle);
        }
    }

    private void Mourir()
    {
        Debug.Log("Le joueur est mort !");

        if (prefabExplosion != null)
        {
            Instantiate(prefabExplosion, transform.position, Quaternion.identity);
        }

        if (GestionnaireUI.instance != null)
        {
            GestionnaireUI.instance.AfficherGameOver();
        }

        // Détruit complètement le tank joueur
        Destroy(gameObject);
    }

    // Utilisées par PorteSortie pour sauvegarder les stats avant de changer de niveau
    public int ObtenirVie()
    {
        return vieActuelle;
    }

    public int ObtenirMissiles()
    {
        return nombreMissiles;
    }

    // Rebond instantané au premier contact avec un ennemi (collider en Is Trigger)
    private void OnTriggerEnter2D(Collider2D autre)
    {
        if (autre.CompareTag(tagEnnemi))
        {
            Vector2 directionRebond = (transform.position - autre.transform.position).normalized;
            transform.position += (Vector3)(directionRebond * forceRebond);
        }
    }

    // Repousse continuellement le joueur tant qu'il essaie de pousser dans l'ennemi,
    // pour empêcher de passer à travers en maintenant une touche de direction
    private void OnTriggerStay2D(Collider2D autre)
    {
        if (autre.CompareTag(tagEnnemi))
        {
            Vector2 directionRebond = (transform.position - autre.transform.position).normalized;
            transform.position += (Vector3)(directionRebond * vitesse * Time.deltaTime);
        }
    }
}