using UnityEngine;

public class RandomizeStartAnimation : MonoBehaviour
{
    [SerializeField] string animationName;
    [SerializeField] Animator[] animators;

    private void Start()
    {
        RandomizeAnimations();
    }

    void RandomizeAnimations()
    {
        foreach (Animator animator in animators)
        {
            float randomValue = Random.Range(0f, 1f);
            animator.Play(animationName, 0, randomValue);
        }
    }
}
