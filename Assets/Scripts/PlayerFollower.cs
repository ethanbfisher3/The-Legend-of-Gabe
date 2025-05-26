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
                if (playerPosition.x > cameraPosition.x)
                    newPosition.x = playerPosition.x - distanceBeforeMoving.x;
                else
                    newPosition.x = playerPosition.x + distanceBeforeMoving.x;
            }

            if (Math.Abs(diff.y) >= distanceBeforeMoving.y)
            {
                if (playerPosition.y > cameraPosition.y)
                    newPosition.y = playerPosition.y - distanceBeforeMoving.y;
                else
                    newPosition.y = playerPosition.y + distanceBeforeMoving.y;
            }

            // Keep the camera at the player's position, but offset by a fixed distance
            camera.transform.position = new Vector3(newPosition.x, newPosition.y, cameraPosition.z);
        }   
    }
}
