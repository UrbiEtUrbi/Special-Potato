using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitTrigger : MonoBehaviour
{


    [SerializeField]
    public int Room;

    [SerializeField]
    public int Entrance;

    public void OnTriggerEnter2D()
    {
        ControllerGame.Rooms.LoadRoom(Room, Entrance);

    }
}
