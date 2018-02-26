using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Quest", menuName = "Quest")]
public class Quest : ScriptableObject {
    public int STT;
    public Sprite QuestSpriteIcon;
    public string Description;
    public int amountKey;
    public int[] nextQuest;
    public List<Sprite> listType = new List<Sprite>();
    public bool endDay;
}
