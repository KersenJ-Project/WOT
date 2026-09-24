using UnityEngine;

public class PorteSortie : MonoBehaviour
{
    public string nomSceneSuivante;

    private void OnTriggerEnter2D(Collider2D autre)
    {
        if (autre.CompareTag("Player"))
        {
            Debug.Log("MISSION RÉUSSIE !");

            MouvementTank joueur = autre.GetComponent<MouvementTank>();
            if (joueur != null && GestionnaireJeu.instance != null)
            {
                GestionnaireJeu.instance.SauvegarderStats(joueur.ObtenirVie(), joueur.ObtenirMissiles());
            }

            if (GestionnaireJeu.instance != null && !string.IsNullOrEmpty(nomSceneSuivante))
            {
                GestionnaireJeu.instance.ChargerNiveauSuivant(nomSceneSuivante);
            }
        }
    }
}