using System.Collections;
using System.Collections.Generic;
using UnityEngine;


class Player
{
    private List<string> Inventory;

    public Player(List<string> items)
    {
        Inventory = items;
    }

    public List<string> GetItems()
    {
        return Inventory;
    }

}

public class mathTesting : MonoBehaviour
{
    public List<string> test;

    private void Start()
    {
        var jacksItems = new List<string>();
        jacksItems.Add("potion");
        jacksItems.Add("yeet");
        var jack = new Player(jacksItems);

        var jillsItems = new List<string>();
        jillsItems = jack.GetItems();
        jillsItems.Add("helmet");
        jillsItems.Add("yes");
        var jill = new Player(jillsItems);

        test = jacksItems;

        foreach (string item in jack.GetItems()) { print(item); }



    }




}
