using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitController : MonoBehaviour
{
    private GameObject unitView;
    private UnitModel unitModel;
    private GameController gameController;

    public void Setup(UnitModel model, GameObject view, GameController gameController)
    {
        unitView = view;
        unitModel = model;
        this.gameController = gameController;
    }

    public void MovePosition(PathfindingNode node)
    {
        unitView.GetComponent<Transform>().position = node.GetSpace().controller.GetPosition();
    }
}
