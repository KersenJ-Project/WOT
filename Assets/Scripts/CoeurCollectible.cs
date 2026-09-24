using UnityEngine;

public class CoeurCollectible : MonoBehaviour
{
    public int quantiteSoin = 1;
    public string tagJoueur = "Player";

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (!collider.CompareTag(tagJoueur)) return;

        MouvementTank joueur = collider.GetComponent<MouvementTank>();
        if (joueur != null)
        {
            joueur.Soigner(quantiteSoin);
            Destroy(gameObject);
        }
    }
}