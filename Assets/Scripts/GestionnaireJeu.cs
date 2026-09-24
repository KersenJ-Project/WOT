using UnityEngine;
using UnityEngine.SceneManagement;

public class GestionnaireJeu : MonoBehaviour
{
    public static GestionnaireJeu instance;


    [HideInInspector] public int vieSauvegardee = -1;
    [HideInInspector] public int missilesSauvegardes = -1;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Survit au chargement de la scène suivante
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Appelée par PorteSortie juste avant de charger le niveau suivant
    public void SauvegarderStats(int vieActuelle, int missilesActuels)
    {
        vieSauvegardee = vieActuelle + 2;
        missilesSauvegardes = missilesActuels + 4; // Bonus de +3 missiles pour le niveau suivant
    }

    public void ChargerNiveauSuivant(string nomScene)
    {
        Time.timeScale = 1f; // Au cas où le jeu était en pause
        SceneManager.LoadScene(nomScene);
    }

    // Appelée par le bouton "Rejouer" du panel Game Over
    public void RejouerPartie()
    {
        vieSauvegardee = -1;
        missilesSauvegardes = -1;
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    // Appelée par les boutons "Niveau 1" / "Niveau 2" du panel de victoire
    // Réinitialise les stats (vie/missiles par défaut) et charge le niveau choisi
    public void RecommencerAuNiveau(string nomScene)
    {
        vieSauvegardee = -1;
        missilesSauvegardes = -1;
        Time.timeScale = 1f;
        SceneManager.LoadScene(nomScene);
    }
}