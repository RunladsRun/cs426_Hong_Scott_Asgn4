using UnityEngine;

public class FinishZone : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("ball")) // Ensure players have the "Player" tag
        {
            GameObject player = GameObject.FindWithTag("Player");
            PlayerTimer playerTimer = player.GetComponent<PlayerTimer>();
            if (playerTimer != null)
            {
                playerTimer.StopTimer(); // Stop the player's timer
                // Debug.Log("You Finished!");
            }
        }
    }
}
