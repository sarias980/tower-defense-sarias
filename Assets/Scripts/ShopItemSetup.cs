using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName ="Shop", menuName = "Shop/Item")]
public class ShopItemSetup : ScriptableObject
{
    public string shopOwnerName;
    public string shopName;

    public List<Item> shopItem;

    [CustomEditor(typeof(Item))]
    public class ItemInInspector : Editor
    {
        string id = "pepe";
        float numero = 9.0f;
        int entero = 88;

        bool isShowing = true;
        //Si queremos hacerlo para cada uno de la lista del foreach necesitariamos una lista de bools
        
        public override void OnInspectorGUI()
        {
            //base.OnInspectorGUI();
            //DrawDefaultInspector();
            // Item myItem = target as Item;
            ShopItemSetup myShop = target as ShopItemSetup;
            isShowing = EditorGUILayout.Foldout(isShowing, "NOMBRES");
            if (isShowing)
            {
                myShop.shopName = EditorGUILayout.TextField("shopName", myShop.shopName);
                EditorGUILayout.TextField(myShop.shopOwnerName);
            }
            if (GUILayout.Button("Add"))
            {
                myShop.shopItem.Add(new Item());
            }
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.Space();

            int size = EditorGUILayout.IntField("Tamaño Lista", myShop.shopItem.Count);
            int i = 0;
            foreach(Item item in myShop.shopItem)
            {
                item.id = i.ToString("oooo");
                EditorGUILayout.LabelField(item.id);
                item.id = EditorGUILayout.TextField("id", item.id);
                item.name = EditorGUILayout.TextField("Name", item.name);

                EditorGUILayout.Space();

                i++;
            }

            //EditorGUILayout.FloatField("Numero: ", numero);
            // EditorGUILayout.IntField("Entero: ", entero);

        }
    }
}
