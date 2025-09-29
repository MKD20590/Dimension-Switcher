using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Daisy : MonoBehaviour
{
    private Player player;
    private void Start()
    {
        player = FindObjectOfType<Player>();
    }
    public void Walk()
    {
        player.WalkingSfx();
    }
    public void Land()
    {
        player.LandSfx();
    }
}
