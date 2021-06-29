using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ObjectsList")]
public class ChangingObjectList : ScriptableObject
{
    public List<Turtle> TurtleList = new List<Turtle>();

    public void UpdateTurtles()
    {
        foreach (Turtle t in TurtleList)
        {
            t.ChangeMode();
        }
    }
}
