using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

[System.Serializable]
public struct BlockTypeSpritePair
{
    public BlockType type;
    public Sprite sprite;
}

[System.Serializable]
public struct BlockTypeParticlePair
{
    public BlockType type;
    public GameObject particlePrefab;
}

[System.Serializable]
public class BlockFactory : Singleton<BlockFactory>
{
    public BlockTypeSpritePair[] blockSprites;
    public BlockTypeParticlePair[] blockParticles;

    public Sprite GetBlockSprite(BlockType type)
    {
        foreach(BlockTypeSpritePair block in blockSprites)
        {
            if (block.type == type)
                return block.sprite;
        }
        DebugUtil.debugError("Given type is not existed in BlockFactory sprites!");
        return null;
    }

    public GameObject GetBlockParticlePrefab(BlockType type)
    {
        foreach (BlockTypeParticlePair prefab in blockParticles)
        {
            if (prefab.type == type)
                return prefab.particlePrefab;
        }
        //DebugUtil.debugError("Given type is not existed in BlockFactory Particles (Or particle is not create)! TYPE: " + type.ToString());
        return null;
    }

    public T GetRandomEnum<T>()
    {
        Array values = System.Enum.GetValues(typeof(T));
        int randIndex = UnityEngine.Random.Range(0, values.Length - 1);
        return (T)values.GetValue(randIndex);
    }
}
