using Godot;
using System;

public class Enemy : Node2D
{
    private Sprite sprite;
    private Node2D explosion;
    private int speed;

    private const int BASE_SPEED = 100;
    private readonly string[] sprites = {"enemy1_blue.png", "enemy1_green.png",
        "enemy1_red.png", "enemy1_yellow.png",
        "enemy2_blue.png", "enemy2_pink.png",
        "enemy2_red.png", "enemy2_yellow.png"};

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
      speed = BASE_SPEED + globals.currentStage * 10;
      explosion = (Node2D)ResourceLoader.Load<PackedScene>("res://Explosion.tscn").Instance();
    }

    public override void _EnterTree() {
      sprite = new Sprite();
      Random rng = new Random();
      sprite.Texture = ResourceLoader.Load<Texture>("res://assets/graphics/enemies/" +
          sprites[rng.Next(0,sprites.Length)]);
      AddChild(sprite);
    }

  public override void _Process(float delta)
  {
    MoveLocalX(-1 * delta * speed);
  }

  public void _on_Area2D_area_entered(Area2D area) {
    if (area.GetCollisionLayerBit(3)) {
      explosion.Position = this.Position;
      GetParent().AddChild(explosion);
      ((AnimationPlayer)explosion.GetNode("AnimationPlayer")).Play("Explode");
      globals.kills += 1;
      QueueFree();
    }
  }
}
