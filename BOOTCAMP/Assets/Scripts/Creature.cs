using UnityEngine;

public class Creature : MonoBehaviour
{
    public enum Team
    {
        Player, Enemy
    }

    public Team team;
    public Transform head;
}
