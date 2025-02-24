using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Card/Card")]
public class CardData : ScriptableObject
{
    public int ID;
    public int itemID;
    public string cardName;
    public Sprite cardArt;
}
