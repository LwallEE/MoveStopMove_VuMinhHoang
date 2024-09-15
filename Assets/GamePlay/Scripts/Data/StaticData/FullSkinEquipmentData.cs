using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct FullSkinPart
{
    public SkinPartEnum partPosition;
    public int partIndex;
    public Color color;
    public bool isNotApplyColor;
}

public enum SkinPartEnum
{
    Head,
    Tail,
    LeftArm,
    Wing
}
[CreateAssetMenu(menuName = "MyAssets/EquipmentData/FullSkinEquipmentData")]
public class FullSkinEquipmentData : EquipmentData
{
    public Color bodyColor;
    public Texture bodyTexture;

    public List<FullSkinPart> skinParts;
}
