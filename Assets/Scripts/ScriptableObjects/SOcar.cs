using UnityEngine;

[CreateAssetMenu(fileName = "car", menuName = "Game/car")]
public class SOcars : ScriptableObject
{
    //variables
    public string carName;
    public Sprite icon;
    public RuntimeAnimatorController animator;

    [Header("defaultValues")]
    public int defaultSpeed = 7;
    public int aceleration = 3;
    public int rotationSpeed = 100;
    public int defaultHealth = 100;
    public int attack = 10;
}
