using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Item Data", menuName = "Data/ItemData")]
public class ItemData : ScriptableObject
{
    public Sprite Icon { get { return icon; } }
    [SerializeField] private Sprite icon;

    public string ItemName { get { return itemName; } }
    [SerializeField] private string itemName;

    public ItemType Type { get { return type; } }
    [SerializeField] private ItemType type;

    public string Title { get { return title; } }
    [SerializeField] private string title;

    public string Desc { get { return desc; } }
    [SerializeField] private string desc;


}
