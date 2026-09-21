using UnityEngine;

public class MissileCollectible : MonoBehaviour
{
    public int quantiteAjoutee = 1;
    public string tagJoueur = "Player";

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (!collider.CompareTag(tagJoueur)) return;

        MouvementTank joueur = collider.GetComponent<MouvementTank>();
        if (joueur != null)
        {
            joueur.AjouterMissiles(quantiteAjoutee);
            Destroy(gameObject);
        }
    }
}