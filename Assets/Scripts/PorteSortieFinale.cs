using UnityEngine;

public class PorteSortieFinale : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D autre)
    {
        if (autre.CompareTag("Player"))
        {
            Debug.Log("BRAVO ! JEU TERMINÉ !");

            if (GestionnaireUI.instance != null)
            {
                GestionnaireUI.instance.AfficherVictoire();
            }
        }
    }
}