using UnityEngine;
using System.Collections;

public class NetGolfBallController : MonoBehaviour
{
    [Header("Ball Information")]
    public Rigidbody2D _rb;
    private NeuralNetwork _net;

    public float angle; // The angle of the ball
    float maxAngle = 180.0f;

    public float power; // The power of the ball
    float maxPower = 1000.0f;

    public int timesHit = 0; // The number of times the ball has been hit
    public bool initialized = false;

    private void Start()
    {

    }
    void FixedUpdate()
    {
        if (initialized)
        {
            if (_rb.IsSleeping() && timesHit < 100)
            {
                Put();
            }
        }
    }

    void Put()
    {
        float[] inputs = new float[] {transform.position.x, transform.position.y};
        float[] outputs = _net.FeedForward(inputs);

        Debug.Log(outputs[0]);
        Debug.Log(outputs[1]);

        float radianAngle = Mathf.Deg2Rad * (maxAngle/2 + (maxAngle/2 * outputs[0]));
        float power = (maxPower / 2 + (maxPower / 2 * outputs[1]));

        _rb.AddForce(new Vector2(Mathf.Cos(radianAngle), Mathf.Sin(radianAngle)) * power, ForceMode2D.Impulse);
        timesHit += 1;

        Debug.Log(timesHit);
    }

    public void Init(NeuralNetwork net)
    {
        this._net = net;
        this.initialized = true;
    }
}