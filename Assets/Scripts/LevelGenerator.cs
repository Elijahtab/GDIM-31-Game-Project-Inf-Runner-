using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// By: Nicolas Assakura Miyazaki
public class LevelGenerator : MonoBehaviour
{
    [SerializeField]
    private GameObject[] levelConfigs;
    [SerializeField]
    private float spawnInterval;

    private int numConfigs;
    private float timer;

    void Start()
    {
        timer = 0;
        numConfigs = levelConfigs.Length;
    }

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= spawnInterval)
        {
            timer = 0;
            int nextConfig = Random.Range(0, numConfigs);
            GameObject newConfig = Instantiate(levelConfigs[nextConfig],
                                                transform.position, transform.rotation);
            WorldMover.addMovable(newConfig);
        }
    }
}
