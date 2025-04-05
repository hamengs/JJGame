using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SaveData
{
    public int health;
    public int money;
    public int attackPower;
    // �洢�����ߵı�ʶ�������ʹ����Ʒ�����б�ʵ����Ŀ�п�����Ҫ�����ӵĽṹ
    public List<string> inventoryItemNames;
    public List<int> inventoryItemCounts;
    // ���磬�����Դ洢������������Ϣ
    public int savedPortalEnum;
    public string savedScene;
}

