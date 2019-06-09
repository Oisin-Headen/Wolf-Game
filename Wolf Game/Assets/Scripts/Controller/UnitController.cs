using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

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

    public void MovePosition(SpaceModel space)
    {
        unitView.GetComponent<Transform>().position = space.controller.GetPosition();
    }
}
