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
    private Transform icon;
    private Transform backgroundSymbol;
    private Sprite NormalBackgroundSprite;

    internal void Setup(UnitModel model, GameObject view, GameController gameController)
    {
        unitView = view;
        unitModel = model;
        this.gameController = gameController;

        Sprite unitSprite = gameController.assets.Units[model.UnitType.ID];
        switch(model.UnitType.iconKind)
        {
            case UnitTypeModel.UnitKind.Normal:
                NormalBackgroundSprite = gameController.assets.UnitBackGrounds[Assets.UnitBackgrounds.Normal];
                break;
            case UnitTypeModel.UnitKind.Worker:
                NormalBackgroundSprite = gameController.assets.UnitBackGrounds[Assets.UnitBackgrounds.Worker];
                break;
        }
        icon = transform.GetChild(0);
        backgroundSymbol = transform.GetChild(1);

        icon.GetComponent<SpriteRenderer>().sprite = unitSprite;
        backgroundSymbol.GetComponent<SpriteRenderer>().sprite = NormalBackgroundSprite;
    }

    public void MovePosition(SpaceModel space)
    {
        unitView.GetComponent<Transform>().position = space.controller.GetPosition();
    }

    public void SetBackGroundShape(Assets.UnitBackgrounds background)
    {
        backgroundSymbol.GetComponent<SpriteRenderer>().sprite = gameController.assets.UnitBackGrounds[background];
    }

    public void RevertBackground()
    {
        backgroundSymbol.GetComponent<SpriteRenderer>().sprite = NormalBackgroundSprite;
    }
}