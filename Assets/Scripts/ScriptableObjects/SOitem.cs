using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "Game/Item")]

public class SOitem : ScriptableObject
{
    //variables
    public string itemName;
    public Sprite icon;
    public Item Prefab;
}
