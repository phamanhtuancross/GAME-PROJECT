using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum BlockType
{
    EMPTY,
    BLOCK_BOARD,
    BLOCK_GREEN,
    BLOCK_ORANGE,
    BLOCK_PURPLE,
    BLOCK_RED,
    BLOCK_YELLOW,
    BLOCK_GRASS,
    BLOCK_ROCK,
    BLOCK_BLUE
}

/// <summary>
/// Block class
/// <para>+ Use to create a single block</para>
/// <para>+ Handle:</para>
/// <para>    -Change block type</para>
/// <para>    -Clear block</para>
/// </summary>
[System.Serializable]
public class Block : MonoBehaviour {
    public BlockType type;
    //[HideInInspector]
    public Vector2Int index;
    public float particleMaxDuration = 3f;

    private int iceBreak;
    private Image image;

    private bool isClearing;

    public bool IsClearing
    {
        get
        {
            return isClearing;
        }
        set
        {
            isClearing = value;
        }
    }

    public int IceBreak
    {
        get
        {
            return iceBreak;
        }
    }
    public void UpdateIceBreak()
    {
        iceBreak--;
        Color c = image.color;
        c.a /= 2;
        image.color = c;
    }

    private void Awake()
    {
        image = this.GetComponentInChildren<Image>();
        isClearing = false;
        iceBreak = 1;
        //ChangeType(BlockType.EMPTY);
    }
    /// <summary>
    /// Change the type of the block and its sprite
    /// </summary>
    /// <param name="type">The target type</param>
    public void ChangeType(BlockType type)
    {
        this.type = type;

        if (image == null)
            image = GetComponentInChildren<Image>();

        image.sprite = BlockFactory.Instance.GetBlockSprite(type);
        GameObject particlePrefab = BlockFactory.Instance.GetBlockParticlePrefab(type);
    }

    /// <summary>
    /// Clear the block and run its clear effect
    /// </summary>
    [ContextMenu("Clear")]
    public void ClearBlock()
    {
        if (type == BlockType.BLOCK_BOARD || type == BlockType.EMPTY)    //empty block cannot be clear
            return;
        if (type == BlockType.BLOCK_BLUE)
        {
            Color c = image.color;
            c.a = 1; // FIX
            image.color = c;
        }
        //start clear animation
        isClearing = true;
        GetComponentInChildren<BlockImageAnimEvents>().startClear();

        //emit particle
        GameObject particlePrefab = BlockFactory.Instance.GetBlockParticlePrefab(type);
        if (particlePrefab == null)
            return;
        GameObject particle = GameObject.Instantiate(particlePrefab, this.transform, true);
        particle.transform.localScale = Vector3.one;
        particle.GetComponent<ParticleSystem>().Play(true);
        float particleDuration = particleMaxDuration;
        GameObject.Destroy(particle, particleDuration);
        particle.transform.localPosition = Vector3.zero;

        type = BlockType.BLOCK_BOARD;
        
    }
}
