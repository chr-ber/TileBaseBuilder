﻿using UnityEngine;

public class BuildModeController : MonoBehaviour
{
    bool _buildModeIsObjects = false;
    string _buildModObjectType;
    TileType _buildModeTile = TileType.Floor;

    public void SetModeBuildFloor()
    {
        _buildModeIsObjects = false;
        _buildModeTile = TileType.Floor;
    }

    public void SetModeBuildWalls()
    {
        _buildModeIsObjects = true;
        _buildModObjectType = "greyWall";
    }

    public void SetModeBuildDarkWalls()
    {
        _buildModeIsObjects = true;
        _buildModObjectType = "darkgreyWall";
    }

    public void SetModeBulldoze()
    {
        _buildModeIsObjects = false;
        _buildModeTile = TileType.Ground;
    }

    public void DoBuild(Tile tile)
    {
        if (_buildModeIsObjects)
        {
            // If we can not place the furniture in the selected tile abort
            if (WorldController.Instance.World.IsFurniturePlacementValid(_buildModObjectType, tile) == false || tile.PendingFurnitureJob != null)
                return;

            // We are saving the furniture type in a local variable as at the time the job is executed
            // the _buildModObjectType variable most likely has changed
            string furnitureType = _buildModObjectType;

            Job job = new Job(tile, furnitureType, (sender,args) =>
            {
                WorldController.Instance.World.PlaceFurniture(furnitureType, tile);
            });

            WorldController.Instance.World.JobQueue.Enqueue(job);
            Debug.Log("Job added.");
        }
        // We are in bulldoze mode
        else if (_buildModeTile == TileType.Ground && tile.Furniture != null)
        {
            // TODO: Currently we are not able to handle deletion of Furniture due to refactoring of classes

            // tilesFurnitureRemoved.Add(tile);
            // WorldController.Instance.OnFurnitureRemoved(tile.Furniture);
            // if (tilesFurnitureRemoved.Count != 0)
            // {
            //     Debug.Log("Furniture to remove " + tilesFurnitureRemoved.Count);
            //     // TODO: Currently we are not able to handle deletion of Furniture due to refactoring of classes
            //     WorldController.Instance.UpdateFurnitureSprites(tilesFurnitureRemoved);
            // }

        }
        else
        {
            tile.Type = _buildModeTile;
        }
    }
}
