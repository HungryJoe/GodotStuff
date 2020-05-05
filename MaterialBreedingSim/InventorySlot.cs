using Godot;
using System;

public class InventorySlot : Panel
{
    private Texture tex;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
      tex = GD.Load<Texture>("res://textures/stone.png");
      TextureRect material = GetNode<TextureRect>("Inside/Material");
      material.Texture = tex;
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}
