using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEditor;

[Serializable]
public class Item : MonoBehaviour {

    public enum ItemType { WEAPON, POTION, CLOTH};

    public string id;
    public string name;

    public ItemType type;
    public int resistance;
    public int health;

    public float brokeProbability;

    public Sprite shopIcon;

    public bool canWearMage;
    public bool canWearWarrior;
    public bool canWearElf;

    public float price;

}
[CustomEditor(typeof(Item))]
public class ItemInInspector : Editor
{
    string id = "pepe";
    float numero = 9.0f;
    int entero = 88;


    public override void OnInspectorGUI()
    {
        //base.OnInspectorGUI();
        //DrawDefaultInspector();
        Item myItem = target as Item;
       // ShopItemSetup myShop = target as ShopItemSetup;

        EditorGUILayout.TextField(myItem.id);
        EditorGUILayout.TextField(myItem.name);


        EditorGUILayout.FloatField("Numero: ", numero);
        EditorGUILayout.IntField("Entero: ", entero);

    }
}
