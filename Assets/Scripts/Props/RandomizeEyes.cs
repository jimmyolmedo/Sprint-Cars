using System.Collections;
using UnityEngine;

public class RandomizeEyes : MonoBehaviour
{
    [SerializeField] Animator[] animators;
    [SerializeField] int minTime = 1;
    [SerializeField] int maxTime = 4;

    private void Start()
    {
        StartCoroutine(randomize());
    }

    IEnumerator randomize()
    {
        while (true)
        {
            int index = Random.Range(0, animators.Length);
            animators[index].Play("Eyes animation");

            yield return new WaitForSeconds(Random.Range(minTime, maxTime));
        }
    }
}
