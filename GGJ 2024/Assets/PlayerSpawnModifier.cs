using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerSpawnModifier : MonoBehaviour
{
    public List<Transform> SpawnLocations;

    public List<PlayerInput> Players;

    public List<GameObject> PlayerModels;

    public void OnPlayerJoin(PlayerInput pInput)
    {
        Players.Add(pInput);
        int p = Players.IndexOf(pInput);

        pInput.transform.position = SpawnLocations[p].position;

        Instantiate(PlayerModels[p],pInput.transform);
    }

    public void OnPlayerLeave(PlayerInput pInput)
    {
        Players.Remove(pInput);
    }
}
