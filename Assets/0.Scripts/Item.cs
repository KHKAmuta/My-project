using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item
{
    public ItemType Type { get; private set; }
    public int Level { get; private set; }

    public Item(ItemType type, int level = 1)
    {
        Type = type;
        Level = level;
    }

    public void LevelUp()
    {
        Level++;
    }
}