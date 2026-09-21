using UnityEngine;
using TMPro;

public class GestionnaireUI : MonoBehaviour
{
    // Accès global simple depuis n'importe quel autre script (GestionnaireUI.instance)
    public static GestionnaireUI instance;

    [Header("Références UI")]
    public TMP_Text texteMissiles;
    public TMP_Text texteEnnemis;

    private int ennemisTues = 0;
    private int totalEnnemis = 0;

    private void Awake()
    {
        // S'assure qu'il n'existe qu'un seul GestionnaireUI dans la scène
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        // Compte automatiquement tous les tanks ennemis présents dans la scène au lancement
        totalEnnemis = FindObjectsOfType<TankEnemy>().Length;
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
    }

    private void MettreAJourEnnemis()
    {
        if (texteEnnemis != null)
        {
            texteEnnemis.text = "Ennemis tué " + ennemisTues + "/" + totalEnnemis;
        }
    }
}