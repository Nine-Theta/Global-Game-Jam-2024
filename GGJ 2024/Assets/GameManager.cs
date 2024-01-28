using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    public Transform WeaponSpawn;

    public List<Weapon> Weapons;

    public PlayerInputManager PlayerManager;

    public float Cooldown = 20;
    private float _counter = 30;

    private void Start()
    {
        Instantiate(Weapons[Random.Range(0, Weapons.Count - 1)],WeaponSpawn.position,WeaponSpawn.rotation);
        _counter = Cooldown;
    }

    private void Update()
    {
        if(_counter <= 0)
        {
            Instantiate(Weapons[Random.Range(0, Weapons.Count - 1)], WeaponSpawn.position, WeaponSpawn.rotation);
            _counter = Cooldown;
        }

        _counter -= Time.deltaTime;
    }

    public void OnPlayerLeave(PlayerInput pInput)
    {
        Instantiate(Weapons[Random.Range(0, Weapons.Count - 1)], WeaponSpawn.position, WeaponSpawn.rotation);
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("player entered");

        if (other.CompareTag("Player"))
        {
            PlayerInputManager.instance.playerLeftEvent.Invoke(other.GetComponent<PlayerInput>());
            other.GetComponent<PlayerInput>().user.UnpairDevicesAndRemoveUser();
        }
    }
}
