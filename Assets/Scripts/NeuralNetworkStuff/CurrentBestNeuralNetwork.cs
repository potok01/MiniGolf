using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CurrentBestNeuralNetwork
{
    public static bool auto { get; set; } = false;
    public static NeuralNetwork bestNet { get; set; }
}
