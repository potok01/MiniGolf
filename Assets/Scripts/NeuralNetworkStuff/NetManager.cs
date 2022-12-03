using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public class NetManager : MonoBehaviour
{
    public int finishedNets = 0;
    public int generation = 0;

    public float bestFitness = 0;
    public float worstFitness = 0;
    public int bestFitnessTimesHit = 0;

    private int population = 8;
    public int maxHitAttempts = 1;
    public float maxPower = 1000f;
    public float maxAngle = 180.0f;

    private int[] layers = {484, 484, 3};

    public GameObject netBall;

    public List<NeuralNetwork> nets = new List<NeuralNetwork>();
    public List<GameObject> netBalls = new List<GameObject>();

    private Tilemap _tilemap;

    public int[] worldState = new int[482];
    public float goalPosX;
    public float goalPosY;

    private void Awake()
    {
        _tilemap = GameObject.Find("Tilemap").GetComponent<Tilemap>();
    }
    // Start is called before the first frame update
    void Start()
    {
        if(population%4 != 0)
        {
            population = 20;
            Debug.Log("Population must be divisible by 4, it was set to 20 by default");
        }

        parseWorld();
        for(int i = 0; i < population; i++)
        {
            NeuralNetwork net = new NeuralNetwork(layers, worldState);
            net.Mutate();
            nets.Add(net);

            GameObject newNetBall = Instantiate(netBall, new Vector3(-14.75f, -7.75f, 0), new Quaternion(0, 0, 0, 1));
            newNetBall.GetComponent<NetGolfBallController>().Init(net, this);
            netBalls.Add(newNetBall);
        }
    }

    // Update is called once per frame
    void Update()
    {
        // These buttons control timescale of the evolution since the balls can take a long time to stop sometimes
        // Subject to change but shorthand is the following
        // MegaSpeed = m
        // SuperSpeed = d
        // SpeedUp = s
        // NormalSpeed = a
        // SlowDown = f
        if (Input.GetButtonDown("MegaSpeed"))
        {
            Time.timeScale = 100;
        }
        if (Input.GetButtonDown("SuperSpeed"))
        {
            Time.timeScale = 20;
        }
        if (Input.GetButtonDown("SpeedUp"))
        {
            Time.timeScale = 5;
        }
        if (Input.GetButtonDown("NormalSpeed"))
        {
            Time.timeScale = 0.5f;
        }
        if (Input.GetButtonDown("SlowDown"))
        {
            Time.timeScale = 0.5f;
        }

        if (finishedNets >= population)
        {
            // Finished nets is set to 0 so that we know none of the next generation have finished
            finishedNets = 0;

            int maxFitnessIndex = 0;
            float maxFitness = Mathf.NegativeInfinity;
            for(int i = 0; i < nets.Count; i++)
            {
                if (nets[i].GetFitness() >= maxFitness)
                {
                    maxFitnessIndex = i;
                    maxFitness = nets[i].GetFitness();
                }
            }

            bestFitnessTimesHit = netBalls[maxFitnessIndex].GetComponent<NetGolfBallController>().timesHit;

            // Sort nets in ascending order so that nets[0] is worst fitness and nets[population - 1] is best
            nets.Sort();

            // Get fitnesses and times hit for display
            bestFitness = nets[population - 1].GetFitness();
            worstFitness = nets[0].GetFitness();
            

            // Generate next generation
            for (int i = 0; i < population / 4; i++)
            {
                // Randomize first two quarters of next generation
                nets[i] = new NeuralNetwork(layers, worldState);
                nets[i + ((1*population) / 4)] = new NeuralNetwork(layers, worldState);

                // Mutate the best quarter from the last generation as the third quarter of the next generation
                nets[i + ((2*population) / 4)] = new NeuralNetwork(nets[i + ((3*population) / 4)], worldState);
                nets[i + ((2*population) / 4)].Mutate();

                // Copy the best quarter from the last generation as the final quarter of the next generation
                nets[i + ((3*population) / 4)] = new NeuralNetwork(nets[i + ((3*population) / 4)], worldState);
            }

            // Set all fitnesses to 0 since we copied some networks
            for (int i = 0; i < population; i++)
            {
                nets[i].SetFitness(0f);
            }

            // Destroy golf balls from old generation and create new generation of balls
            CreateNetBalls();

            generation += 1;
        }
    }

    // This function destroys old balls and creates new balls
    private void CreateNetBalls()
    {
        if (netBalls != null)
        {
            for (int i = 0; i < netBalls.Count; i++)
            {
                Destroy(netBalls[i].gameObject);
            }

        }

        netBalls = new List<GameObject>();

        for (int i = 0; i < population; i++)
        {
            GameObject newNetBall = Instantiate(netBall, new Vector3(-14.75f, -7.75f, 0), new Quaternion(0, 0, 0, 1));
            newNetBall.GetComponent<NetGolfBallController>().Init(nets[i], this);
            netBalls.Add(newNetBall);
        }

        netBalls[population-1].GetComponent<SpriteRenderer>().color = Color.red;
        netBalls[population - 1].GetComponent<SpriteRenderer>().sortingOrder = 3;
        netBalls[population - 1].AddComponent<LineRenderer>();
        LineRenderer _lr = netBalls[population - 1].GetComponent<LineRenderer>();
        netBalls[population - 1].AddComponent<TracePath>();

    }

    // This function looks at the tilemap and parses the tile information into a one dimensional array to input into the neural network. It gets information
    // about where the blocks are as well as the goal since that information is static for each level.
    public void parseWorld()
    {
        BoundsInt bounds = _tilemap.cellBounds;
        TileBase[] allTiles = _tilemap.GetTilesBlock(bounds);

        for (int x = 0; x < 30; x++)
        {
            for (int y = 0; y < bounds.size.y; y++)
            {
                TileBase tile = allTiles[x + y * bounds.size.x];
                if (tile != null && tile.name == "BlackSquare")
                {
                    worldState[(x * 16) + y + 2] = 1;
                }
                else if (tile != null && tile.name == "Goal")
                {
                    worldState[0] = x;
                    worldState[1] = y;

                    worldState[(x * 16) + y + 2] = 0;
                }
                else
                {
                    worldState[(x * 16) + y + 2] = 0;
                }
            }
        }

        Vector3 goalInLocal = _tilemap.GetCellCenterWorld(new Vector3Int(worldState[0], worldState[1], 0));
        Vector3 goalInWorld = goalInLocal + _tilemap.origin;

        goalPosX = goalInWorld.x;
        goalPosY = goalInWorld.y;
    }

}
