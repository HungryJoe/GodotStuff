using Godot;
using System;

public class Bullet : Node2D
{
    private const int HOR_SPEED = 400;

  // Called every frame. 'delta' is the elapsed time since the previous frame.
  public override void _Process(float delta)
  {
    MoveLocalX(HOR_SPEED * delta);
  }

  public void _on_Area2D_area_entered(Area2D area) {
    if (area.GetCollisionLayerBit(2)) {
      QueueFree();//Delete
    }
  }
}
