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

    private Queue<Vector2> spacesToMove;
    private Vector2 startPos;
    private float startTime;
    private float distance;

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

        spacesToMove = new Queue<Vector2>();
    }

    public void MovePosition(SpaceModel space)
    {
        spacesToMove.Enqueue(space.controller.GetPosition());
        if(spacesToMove.Count == 1)
        {
            startPos = transform.position;
            startTime = Time.time;
            distance = Vector2.Distance(startPos, spacesToMove.Peek());
        }
    }

    public void Update()
    {
        if(spacesToMove.Count > 0)
        {
            float current = (Time.time - startTime) * Utilities.UNIT_SPEED;
            float fraction = current / distance;
            fraction = Mathf.Max(0f, fraction);
            fraction = Mathf.Min(1f, fraction);
            

            unitView.GetComponent<Transform>().position = Vector2.Lerp(startPos, spacesToMove.Peek(), fraction);

            if(fraction >= 1f)
            {
                spacesToMove.Dequeue();

                if (spacesToMove.Count > 0)
                {
                    startPos = transform.position;
                    startTime = Time.time;
                    distance = Vector2.Distance(startPos, spacesToMove.Peek());
                }
            }
        }
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
