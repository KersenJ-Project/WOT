using UnityEngine;

public class TirDoubleCollectible : MonoBehaviour
{
    public string tagJoueur = "Player";

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (!collider.CompareTag(tagJoueur)) return;

        MouvementTank joueur = collider.GetComponent<MouvementTank>();
        if (joueur != null)
        {
            joueur.ActiverTirDouble();
            Destroy(gameObject);
        }
    }
}