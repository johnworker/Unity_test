using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class Spawner
{
    public Transform[] objects;
    public Transform[] positions;

    public void Spawn()
    {
        // Get available indexes in the array of positions
        List<int> available = Enumerable.Range(0, positions.Length).ToList();

        int rnd;
        int index;

        foreach(Transform o in objects)
        {
            // Get random available index
            rnd = UnityEngine.Random.Range(0, available.Count);
            index = available[rnd];

            // Remove index availability
            available.RemoveAt(rnd);
            // Position object to the indexed position
            //o.position = positions[index].position;
            o.localPosition = new Vector3();
            o.localRotation = new Quaternion();
            o.SetParent(positions[index], false);
        }
    }
}
