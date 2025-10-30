using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Challenge1 : MonoBehaviour
{
    [SerializeField]
    float speed = 1;
    [SerializeField]
    Transform[] waypoints;

    int waypointIndex = 1;



    void Update()
    {
        // your code
    }

    /*Challenge 1:
     *Modify the script to get the enemy to follow each 
     *point inside the waypoints array of Transform objects, 
     *once they reach the last point destroy the enemy. 
     *This is like the enemies in the tower defense game.
     */

    /*
     * Test 1: checks if the enemy is moving
     * Test 2: checks if the enemy is moving at the right speed
     * Test 3: checks if enemy facing in the direction he is moving
     * Test 4: checks if enemy facing in the direction he is moving
     * Test 5: checks if top enemy reached the end
     * Test 6: checks if bottom enemy reached the end
     * Test 7: checks if enemy are destroyed at the end
     */
}
