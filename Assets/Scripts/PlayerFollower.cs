using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFollower : MonoBehaviour
{
    new public Camera camera;
    public Player player;

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

            // Keep the camera at the player's position, but offset by a fixed distance
            camera.transform.position = new Vector3(playerPosition.x, playerPosition.y, cameraPosition.z);
        }   
    }
}
