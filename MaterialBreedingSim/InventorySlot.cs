using Godot;
using System;

public class InventorySlot : Panel
{
    [Export]
    public readonly float ALPHA_SELECTED = 1f;
    [Export]
    public readonly float ALPHA_UNSELECTED = 0.5f;
    private Texture tex;
    public bool selected;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
      tex = GD.Load<Texture>("res://textures/stone.png");
      TextureRect material = GetNode<TextureRect>("Inside/Material");
      material.Texture = tex;
      selected = false;
    }

  // Called every frame. 'delta' is the elapsed time since the previous frame.
  public override void _Process(float delta)
  {
    if (selected) {
      Color c = this.Modulate;
      c.a = ALPHA_SELECTED;
      this.Modulate = c;
    } else {
      Color c = this.Modulate;
      c.a = ALPHA_UNSELECTED;
      this.Modulate = c;
    } 
  }
}
