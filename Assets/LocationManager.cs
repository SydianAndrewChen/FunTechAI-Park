using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocationManager : MonoBehaviour
{
    public List<Transform> locations;

    // Update is called once per frame
    public Transform GetLocation(string locationName)
    {
        switch (locationName)
        {
            case "common room":
                return locations[0];
            case "wish garden":
                return locations[1];
            case "library":
                return locations[2];
            case "cafe":
                return locations[3];
            case "grassland":
                return locations[4];
            case "lake":
                return locations[5];
            case "farmland":
                return locations[6];
            case "cliff":
                return locations[7];
            case "garden":
                return locations[8];
            case "restaurant":
                return locations[9];
            case "cliff b":
                return locations[10];
            default:
                return null;
        }
    }
}


