using Godot;
using System;

public class Block : Spatial
{
    private CSGBox box;

    [Export]
    public float side_length = 1;
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
      box = GetNode<CSGBox>("Block");
      box.Width = side_length;
      box.Height = side_length;
      box.Depth = side_length;
    }

}
