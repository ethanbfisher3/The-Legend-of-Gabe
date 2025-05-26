using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFollower : MonoBehaviour
{
    new public Camera camera;
    public Player player;
    public Vector2 distanceBeforeMoving = new Vector2(2.5f, 1.25f);

    void Start()
    {
        camera = Camera.main;
    }

    void Update()
    {
        if (camera != null)
        {
            Vector3 cameraPosition = camera.transform.position;
            Vector3 playerPosition = player.transform.position;
            var diff = playerPosition - cameraPosition;
            var newPosition = cameraPosition;

            if (Math.Abs(diff.x) >= distanceBeforeMoving.x)
            {
                // If the player has moved more than 1 unit in the x direction, update the camera position
                newPosition.x = cameraPosition.x + playerPosition.x - distanceBeforeMoving.x;
            }

            if (Math.Abs(diff.y) >= distanceBeforeMoving.y)
            {
                // If the player has moved more than 1 unit in the y direction, update the camera position
                newPosition.y = cameraPosition.y + playerPosition.y - distanceBeforeMoving.y;
            }

            // Keep the camera at the player's position, but offset by a fixed distance
            camera.transform.position = new Vector3(newPosition.x, playerPosition.y, cameraPosition.z);
        }   
    }
}
