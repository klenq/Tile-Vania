using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinPickup : MonoBehaviour
{
    [SerializeField] int coinPoints = 5;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            //AudioSource.PlayClipAtPoint(coinSound, Camera.main.transform.position);
            FindObjectOfType<GameSession>().GainScore(coinPoints);
            Destroy(gameObject);
        }
    }
}
