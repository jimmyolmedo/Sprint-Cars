using UnityEngine;

public class Goal : MonoBehaviour, Interactable
{
    [SerializeField] RacingManager RacingManager;
    [SerializeField] ParticleSystem goalParticle;
   
    public void Interact(GameObject obj)
    {
        if (obj.TryGetComponent(out PlayerController player))
        {
            RacingManager.CheckGoal(player);
        }
    }

    public void ActivateParticles(PlayerController player)
    {
        goalParticle.transform.position = player.transform.position;
        var main = goalParticle.main;
        main.startColor = player.PlayerColor;
        goalParticle.Play();
    }
}
