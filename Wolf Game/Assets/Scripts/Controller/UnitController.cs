using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using Model;

public class UnitController : MonoBehaviour
{
    private GameObject unitView;
    private UnitModel unitModel;
    private GameController gameController;

    internal void Setup(UnitModel model, GameObject view, GameController gameController)
    {
        unitView = view;
        unitModel = model;
        this.gameController = gameController;

        Sprite unitSprite = gameController.assets.Units[model.UnitType.ID];
        Sprite unitBackground = null;
        switch(model.UnitType.iconKind)
        {
            case UnitTypeModel.UnitKind.Normal:
                unitBackground = gameController.assets.UnitBackGrounds[Assets.UnitBackgrounds.Normal];
                break;
            case UnitTypeModel.UnitKind.Worker:
                unitBackground = gameController.assets.UnitBackGrounds[Assets.UnitBackgrounds.Worker];
                break;
        }

        var icon = transform.GetChild(0);
        var backgroundSymbol = transform.GetChild(1);

        icon.GetComponent<SpriteRenderer>().sprite = unitSprite;
        backgroundSymbol.GetComponent<SpriteRenderer>().sprite = unitBackground;
    }

    public void MovePosition(SpaceModel space)
    {
        unitView.GetComponent<Transform>().position = space.controller.GetPosition();
    }
}