using UnityEngine;
using System.Collections;

public class NetGolfBallController : MonoBehaviour
{
    [Header("Ball Information")]
    public Rigidbody2D _rb;
    public NetManager _nm;
    public NeuralNetwork _net;

    public int maxHits;

    public float angle; // The angle of the ball
    float maxAngle = 180.0f;

    public float power; // The power of the ball
    float maxPower = 1000.0f;

    public int timesHit = 0; // The number of times the ball has been hit
    public bool initialized = false;
    public bool finished = false;

    void FixedUpdate()
    {
        if (initialized)
        {
            if (_rb.IsSleeping() && timesHit < maxHits)
            {
                Put();
            }

            if(_rb.IsSleeping() && timesHit >= maxHits && !finished)
            {
                _net.SetFitness(Mathf.Sqrt(Mathf.Pow((_nm.goalPosX - transform.position.x),2) + Mathf.Pow((_nm.goalPosY - transform.position.y),2)));
                Debug.Log(_net.GetFitness());

                finished = true;
                _nm.finishedNets += 1;
            }
        }
    }

    void Put()
    {
        float[] inputs = new float[] {transform.position.x, transform.position.y};
        float[] outputs = _net.FeedForward(inputs);

        float radianAngle = Mathf.Deg2Rad * (maxAngle/2 + (maxAngle/2 * outputs[0]));
        float power = (maxPower / 2 + (maxPower / 2 * outputs[1]));

        _rb.AddForce(new Vector2(Mathf.Cos(radianAngle), Mathf.Sin(radianAngle)) * power, ForceMode2D.Impulse);
        timesHit += 1;
    }

    public void Init(NeuralNetwork net, NetManager nm)
    {
        this._net = net;
        this._nm = nm;
        this.initialized = true;
        maxHits = _nm.maxHits;
        maxPower = _nm.maxPower;   
        maxAngle = _nm.maxAngle;
    }
}
