using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockImageAnimEvents : MonoBehaviour
{
    public GameObject block;

    public void onClearAnimComplete()
    {
        block.GetComponent<Block>().ChangeType(BlockType.BLOCK_BOARD);
        block.GetComponent<Block>().IsClearing = false;
        this.GetComponent<Animator>().SetTrigger("SpawnBlock");
    }

    public void startClear()
    {
        GetComponent<Animator>().SetTrigger("ClearBlock");
    }
}