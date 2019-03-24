using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecipeDatabase : MonoBehaviour
{
    public List<Recipe> database = new List<Recipe>();

	void Awake ()
    {
        BuildRecipeDatabase();
	}
	
    private void BuildRecipeDatabase()
    {
        database = new List<Recipe>()
        {
            new Recipe("Gas absorbent tool",
            new List<Item>()
            {
                new Item("Alloy", 3),
                new Item("Gold", 1)
            }, 1),
            new Recipe("Upgrade fuel storage",
            new List<Item>()
            {
                new Item("Ship part", 1),
                new Item("Helium", 3)

            }, 2),
            new Recipe("Upgrade engine",
            new List<Item>()
            {
                new Item("Gold", 2),
                new Item("Helium", 3),
                new Item("Water", 3)
            }, 3),
            new Recipe("Farm",
            new List<Item>()
            {
                new Item("Ship part", 1),
                new Item("Water", 3)
            }, 1),
            new Recipe("Upgrade food storage",
            new List<Item>()
            {
                new Item("Alloy", 3),
                new Item("Ship part", 1),
            }, 2),
            new Recipe("Canned food",
            new List<Item>()
            {
                new Item("Alloy", 1)
            }, 3),
            new Recipe("Patch I",
            new List<Item>()
            {
                new Item("Alloy", 3)
            }, 1),
            new Recipe("Patch II",
            new List<Item>()
            {
                new Item("Gold", 1),
                new Item("Alloy", 3)
            }, 2),
            new Recipe("Asteroid Shield",
            new List<Item>()
            {
                new Item("Ship part", 1),
                new Item("Gold", 2),
                new Item("Uranium", 2),
            }, 3),
            new Recipe("Workbench I",
            new List<Item>()
            {
                new Item("Gold", 1),
                new Item("Alloy", 2),
                new Item("Electronics", 1)
            }, 1),
            new Recipe("Workbench II",
            new List<Item>()
            {
                new Item("Gold", 2),
                new Item("Alloy", 3),
                new Item("Electronics", 2)
            }, 2),
            new Recipe("Navigation upgrade",
            new List<Item>()
            {
                new Item("Uranium", 2),
                new Item("Alloy", 5),
                new Item("Helium", 3)
            }, 3),
            new Recipe("Mining drill",
            new List<Item>()
            {
                new Item("Alloy", 3)
            }, 1),
            new Recipe("Scanning station",
            new List<Item>()
            {
                new Item("Mining drill", 1),
                new Item("Electronics", 1),
                new Item("Gold", 1)
            }, 2),
            new Recipe("Mining station",
            new List<Item>()
            {
                new Item("Scanning station", 1),
                new Item("Helium", 3),
                new Item("Uranium", 1)
            }, 3),
            new Recipe("Electronics",
            new List<Item>()
            {
                new Item("Gold", 1),
                new Item("Silicium", 1),
            }, 1),
            new Recipe("Hyperspace drive",
            new List<Item>()
            {
                new Item("Alloy", 5),
                new Item("Electronics", 2),
                new Item("Uranium", 2),
                new Item("Nuclear Fuel", 1)
            }, 3),
        };
        
    }
}
