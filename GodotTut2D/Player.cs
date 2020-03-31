using Godot;
using System;

public class Player : Node2D
{
    public int vertVel {get; set;}
    public int horVel {get; set;}

    private PackedScene bulletClass;
    private Node2D explosion;
    private Timer shootCD;
    private bool dying;

    private const int HOR_SPEED = 150;
    private const int VERT_SPEED_INCREMENT = 10;
    private const int MAX_VERTICAL_SPEED = 200;
    private const float MAX_RATE_OF_FIRE = 3f;//max bullets fired per second

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
      bulletClass = ResourceLoader.Load<PackedScene>("res://Bullet.tscn");
      explosion = (Node2D)ResourceLoader.Load<PackedScene>("res://Explosion.tscn").Instance();
      shootCD = GetNode<Timer>("ShootCoolDown");
      shootCD.WaitTime = 1/MAX_RATE_OF_FIRE;
      vertVel = 0;
      horVel = HOR_SPEED;
      dying = false;
    }

  // Called every frame. 'delta' is the elapsed time since the previous frame.
  public override void _Process(float delta)
  {
    MoveLocalX(horVel * delta);

    float sizeY = GetViewportRect().Size.y;
    if (this.Position.y > 1 && this.Position.y <= sizeY) {
      MoveLocalY(vertVel * delta);
    } else {
      if (this.Position.y < 1) {
        MoveLocalY(VERT_SPEED_INCREMENT);
        vertVel = -1 * vertVel;
      } else if (this.Position.y > sizeY) {
        MoveLocalY(-1 * VERT_SPEED_INCREMENT);
        vertVel = -1 * vertVel;
      }
    }

    if (dying && shootCD.TimeLeft == 0) {
      ((GameScene)GetNode("/root/GameSceneRoot")).PlayerDied();
      QueueFree();
    }
  }

  //Handles user input events
  public override void _Input(InputEvent @event) {
    if (@event.IsAction("player_up") && vertVel >= -1 * MAX_VERTICAL_SPEED && !dying) {
      vertVel -= VERT_SPEED_INCREMENT;
    } else if (@event.IsAction("player_down") && vertVel <= MAX_VERTICAL_SPEED && !dying) {
      vertVel += VERT_SPEED_INCREMENT;
    } else if (@event.IsAction("player_shoot") && shootCD.TimeLeft == 0 && !dying) {
      Bullet bullet = (Bullet)bulletClass.Instance();
      bullet.Position = new Vector2(this.Position.x, this.Position.y + 20);
      GetNode("/root/GameSceneRoot").AddChild(bullet);
      shootCD.Start();
    }
  }

  public void _on_Area2D_area_entered(Area2D area) {
    if (area.GetCollisionLayerBit(2)) {
      explode();
    }
  }

  public void explode() {
    explosion.Position = this.Position;
    GetParent().AddChild(explosion);
    ((AnimationPlayer)explosion.GetNode("AnimationPlayer")).Play("Explode");
    shootCD.WaitTime = 2.5f;
    shootCD.Start();
    ((Sprite)GetNode("Sprite")).Visible = false;
    horVel = 0;
    vertVel = 0;
    dying = true;
  }
}
