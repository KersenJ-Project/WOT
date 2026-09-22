using UnityEngine;
using TMPro;

public class GestionnaireUI : MonoBehaviour
{
    public static GestionnaireUI instance;

    public TMP_Text texteMissiles;
    public TMP_Text texteEnnemis;
    public GameObject[] coeurs;

    private int ennemisTues = 0;
    private int totalEnnemis = 0;

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
    }

    private void Start()
    {
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
}