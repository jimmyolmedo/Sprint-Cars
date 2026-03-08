using System.Collections;
using UnityEngine;

public class Shield : Item
{
    [SerializeField] float timeShield = 1f;

    public override void SetPlayerUser(PlayerController _playerUser)
    {
        base.SetPlayerUser(_playerUser);
        Use();
    }

    public override void Use()
    {
        base.Use();
        StartCoroutine(UserShield(playerUser));
    }

    IEnumerator UserShield(PlayerController player)
    {
        player.ActivateShield(true);
        Debug.Log("escudo activado");
        yield return new WaitForSeconds(timeShield);
        player.ActivateShield(false);
        Debug.Log("escudo desactivado");
        Destroy(gameObject);
    }
}
