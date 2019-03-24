using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Recipe
{
    public List<Item> resources = new List<Item>();

    public string Name;
    public int Level;

    public Recipe(string Name, List<Item> resources, int level)
    {
        this.Name = Name;
        this.resources = resources;
        this.Level = level;
    }
}
