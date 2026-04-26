using System;
using Unity.Cinemachine;
using Unity.VisualScripting;
using UnityEngine;

public class map_Transition : MonoBehaviour
{
    [SerializeField] private PolygonCollider2D mapBoundry;
    CinemachineConfiner2D confiner;
    [SerializeField] private Direction direction;
    [SerializeField] private Transform teleportTargetPosition; // Optional: A specific position to teleport the player to

    [SerializeField] private float transitionDistance = 2f; // Distance to move the player during transition

    enum Direction { Up, Down, Left, Right, Teleport }

    private void Awake()
    {
        confiner = FindObjectOfType<CinemachineConfiner2D>();
    } 

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Add this line to force a message in the console!
        Debug.Log("Something touched the waypoint! It was: " + collision.gameObject.name);

        if (collision.CompareTag("Player"))
        {
            confiner.BoundingShape2D = mapBoundry;
            UpdatePlayerPosition(collision.gameObject);
        }
    }

    private void UpdatePlayerPosition(GameObject player)
    {
        if (direction == Direction.Teleport)
        {
            // Safety check!
            if (teleportTargetPosition != null)
            {
                player.transform.position = teleportTargetPosition.position;
            }
            else
            {
                Debug.LogError("Teleport failed! You forgot to drag a target into the Inspector on " + gameObject.name);
            }
            return;
        }
        Vector3 newPos = player.transform.position;

        switch (direction)
        {
            case Direction.Up:
                newPos.y += transitionDistance; // Adjust as needed
                break;
            case Direction.Down:
                newPos.y -= transitionDistance; // Adjust as needed
                break;
            case Direction.Left:
                newPos.x -= transitionDistance; // Adjust as needed
                break;
            case Direction.Right:
                newPos.x += transitionDistance; // Adjust as needed
                break;
        }

        player.transform.position = newPos;
    }
}
