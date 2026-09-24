using UnityEngine;
using TMPro;

public class GestionnaireUI : MonoBehaviour
{
    public static GestionnaireUI instance;

    public TMP_Text texteMissiles;
    public TMP_Text texteEnnemis;
    public GameObject[] coeurs; // Glisse ici tes 5 icônes de cœur, dans l'ordre

    public GameObject panelGameOver;

    public GameObject panelVictoire;

    public GameObject panelUI;

    private int ennemisTues = 0;
    private int totalEnnemis = 0;

    [SerializeField] private GameObject porteSpawn;
    [SerializeField] private GameObject porteSortie;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        if (porteSortie != null)
        {
            porteSortie.SetActive(false);
        }

        if (panelGameOver != null)
        {
            panelGameOver.SetActive(false);
        }

        if (panelVictoire != null)
        {
            panelVictoire.SetActive(false);
        }
    }

    private void Start()
    {
        // Compte tous les tanks ennemis ET les boss présents dans la scène au lancement
        totalEnnemis = FindObjectsOfType<TankEnemy>().Length + FindObjectsOfType<TankBoss>().Length;
        MettreAJourEnnemis();
    }

    public void MettreAJourMissiles(int nombre)
    {
        if (texteMissiles != null)
        {
            texteMissiles.text = "Missiles : " + nombre;
        }
    }

    // Appelée par TankEnnemi quand un ennemi est détruit
    public void EnnemiDetruit()
    {
        ennemisTues++;
        MettreAJourEnnemis();
        FinDuNiveau();
    }

    private void MettreAJourEnnemis()
    {
        if (texteEnnemis != null)
        {
            texteEnnemis.text = "Ennemis tué " + ennemisTues + "/" + totalEnnemis;
        }
    }

    // Appelée par MouvementRobot à chaque changement de vie du joueur
    public void MettreAJourVie(int vieActuelle)
    {
        if (coeurs == null) return;

        for (int i = 0; i < coeurs.Length; i++)
        {
            if (coeurs[i] != null)
            {
                coeurs[i].SetActive(i < vieActuelle);
            }
        }
    }

    public void FinDuNiveau()
    {
        if (ennemisTues == totalEnnemis)
        {
            if (porteSpawn != null) porteSpawn.SetActive(false);
            if (porteSortie != null) porteSortie.SetActive(true);
        }
    }

    // Appelée par MouvementRobot au démarrage pour se positionner au bon endroit
    public Vector3 ObtenirPositionSpawn()
    {
        if (porteSpawn != null)
        {
            return porteSpawn.transform.position;
        }
        return Vector3.zero;
    }

    // Appelée par MouvementRobot quand le joueur perd toute sa vie
    public void AfficherGameOver()
    {
        if (panelGameOver != null)
        {
            panelGameOver.SetActive(true);
            panelUI.SetActive(false);
        }
        Time.timeScale = 0f; // Met le jeu en pause
    }

    // Appelée par PorteSortieFinale quand le joueur atteint la sortie du dernier niveau
    public void AfficherVictoire()
    {
        if (panelVictoire != null)
        {
            panelVictoire.SetActive(true);
            panelUI.SetActive(false);
        }
        Time.timeScale = 0f; // Met le jeu en pause
    }
}