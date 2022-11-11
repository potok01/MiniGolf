using UnityEngine;

public class RandomGolfBall : MonoBehaviour
{
    public Camera Camera;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            // apply a random upwards force to the ball
            Vector2 direction = new Vector2(Random.Range(-1.0f, 1.0f), Random.Range(0.0f, 1.0f));
            GetComponent<Rigidbody2D>().AddForce(direction * 5000);
        }  
    }
}