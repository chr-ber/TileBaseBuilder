﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Assets.Events;

public enum TileType { Empty, Ground, Gras, Floor, Water, Stone };
public class Tile
{

    TileType type = TileType.Empty;

    public Job PendingFurnitureJob;

    public event EventHandler<TileChangedEventArgs> TileChanged;

    public TileType Type
    {
        get => type;
        set
        {
            if (type != value)
            {
                type = value;
                OnTileChanged();
            }
        }
    }

    public LooseObject LooseObject { get; protected set; }
    public Furniture Furniture { get; protected set; }
    public World World { get; protected set; }

    public int X { get; private set; }
    public int Y { get; private set; }

    public Tile(World world, int x, int y)
    {
        this.World = world;
        this.X = x;
        this.Y = y;
    }

    private void OnTileChanged() => TileChanged.Invoke(this, new TileChangedEventArgs { Tile = this });

    public bool PlaceFurniture(Furniture objInstance)
    {
        if (objInstance == null)
        {
            Furniture = null;
            return true;
        }
        if (Furniture != null)
        {
            Debug.LogError("Trying to assign Furniture to a tile that already has one!");
            return false;
        }
        if (Type != TileType.Floor)
        {
            Debug.LogError("Walls can only be built on Tiles of Type Floor.");
            return false;
        }
        Furniture = objInstance;
        return true;
    }

    public bool RemoveFurniture()
    {
        if (Furniture == null)
        {
            Debug.LogError("Trying to remove Furniture from a tile that already has one!");
            return false;
        }
        Furniture = null;
        return true;
    }

    // Tells us if two Tiles are adjacent
    public bool IsNeighbor(Tile tile, bool diagonal = false)
    {
        if (tile.X == X && (tile.Y == Y - 1 || tile.Y == Y + 1))
            return true;
        if (tile.Y == Y && (tile.X == X - 1 || tile.X == X + 1))
            return true;

        if (diagonal)
        {
            if (tile.X == X + 1 && (tile.Y == Y - 1 || tile.Y == Y + 1))
                return true;
            if (tile.X == X - 1 && (tile.Y == Y - 1 || tile.Y == Y + 1))
                return true;
        }
        return false;
    }
}