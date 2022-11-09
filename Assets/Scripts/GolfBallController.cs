using UnityEngine;

public class GolfBallController : MonoBehaviour
{
    public float maxSpeed = 10.0f;
    public float angle = 0.0f;
    public float velocity = 0.0f;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            // change the projectory of the ball to the left
        }

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            // change the projectory of the ball to the right            
        }

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            // increase the power to be applied to the ball          
        }

        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            // decrease the power to be applied to the ball            
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            angle = Random.Range(0.0f, 360.0f);
            velocity = Random.Range(0.0f, maxSpeed);
            Vector3 force = new Vector3(Mathf.Cos(angle), 0.0f, Mathf.Sin(angle)) * velocity;
            GetComponent<Rigidbody2D>().AddForce(force);
        }
    }
}