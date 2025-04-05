using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SaveData
{
    public int health;
    public int money;
    public int attackPower;
    // 存储库存道具的标识，这里简单使用物品名称列表，实际项目中可能需要更复杂的结构
    public List<string> inventoryItemNames;
    public List<int> inventoryItemCounts;
    // 例如，还可以存储复活点等其他信息
    public int savedPortalEnum;
    public string savedScene;
}

