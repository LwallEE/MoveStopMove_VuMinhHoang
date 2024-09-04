using System.Collections;
using System.Collections.Generic;
using ReuseSystem;
using UnityEngine;

public class GameAssets : Singleton<GameAssets>
{
    [field:SerializeField] public EquipmentManagerSO EquipmentManager { get; private set; }
}
