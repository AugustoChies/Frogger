using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turtle : MovingObjectBehavior
{
    [SerializeField]
    private int turtleStage = 0; //0 = surfaced | 1 = sinking | 2 - sunk | 3 - surfacing
    [SerializeField]
    private GameObject surfaced = null;
    [SerializeField]
    private GameObject unstable = null;
    [SerializeField]
    private Collider mycollider = null;
    [SerializeField]
    private ChangingObjectList objectList = null;

    [SerializeField]
    private bool unchanging = false;

    public PlayerMovement[] playersOnBack;
    private PlayerMovement[] emptyPlayerMovements = new PlayerMovement[0];

    protected override void Awake()
    {
        base.Awake();
        SetTurtleStage();
        objectList.TurtleList.Add(this);
    }

    public override void ChangeMode()
    {
        if (unchanging) return;
        
        turtleStage++;
        turtleStage %= 4;
        SetTurtleStage();
    }

    private void SetTurtleStage()
    {
        surfaced.SetActive(false);
        unstable.SetActive(false);
        switch (turtleStage)
        {
            case 0:
                surfaced.SetActive(true);
                mycollider.enabled = true;
                break;
            case 1:
            case 3:
                unstable.SetActive(true);
                mycollider.enabled = true;
                break;            
            case 2:
                playersOnBack = gameObject.GetComponentsInChildren<PlayerMovement>();
                for (int i = 0; i < playersOnBack.Length; i++)
                {
                    playersOnBack[i].KillPlayer();
                }
                playersOnBack = emptyPlayerMovements;
                mycollider.enabled = false;
                break;
            default:
                break;
        }
    }

    private void OnDisable()
    {
        objectList.TurtleList.Remove(this);
    }
}
