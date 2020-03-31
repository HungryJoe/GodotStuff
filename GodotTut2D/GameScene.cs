using Godot;
using System;

public class GameScene : Node2D
{
    enum GameState { Loading, Running, GameOver }

    private GameState state;
    private PackedScene enemyClass;
    private Player player;
    private Camera2D cam;

    private const int BASE_ENEMIES = 10;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
      state = GameState.Loading;
      enemyClass = ResourceLoader.Load<PackedScene>("res://Enemy.tscn");

      player = (Player)ResourceLoader.Load<PackedScene>("res://Player.tscn").Instance();
      player.Position = new Vector2(300, 720/2);

      cam = new Camera2D();
      cam.Position = new Vector2(360, cam.Position.y);
      cam.MakeCurrent();
      player.AddChild(cam);
      AddChild(player);

      ((Label)GetNode("HUD/Stage")).Text = "Stage " + globals.currentStage;

      SpawnEnemies();
      state = GameState.Running;
    }

  // Called every frame. 'delta' is the elapsed time since the previous frame.
  public override void _Process(float delta)
  {
    ((Label)GetNode("HUD/Kills")).Text = "Kills: " + globals.kills;
  }

  public override void _Input(InputEvent @event) {
    if (state == GameState.GameOver && @event.IsAction("ui_accept")) {
      GetTree().ChangeScene("TitleScene.tscn");
    }
  }

  public void _on_Area2D_area_entered(Area2D area) {
    if (area.GetCollisionLayerBit(4) && state == GameState.Running) {
      //Trigger next stage
      player.vertVel = 0;
      globals.currentStage += 1;
      GetTree().ReloadCurrentScene();
    }
  }

  public void PlayerDied() {
    foreach (Node child in player.GetChildren()) {
      child.QueueFree();
    }
    RemoveChild(player);

    ((Label)GetNode("HUD/Stage")).Text = "Game Over";
    ((Control)GetNode("HUD/Stage")).RectPosition = new Vector2(1280/2, 720/2);
  }

  public void SpawnEnemy(int x, int y) {
    Enemy enemy = (Enemy)enemyClass.Instance();
    enemy.Position = new Vector2(x, y);
    AddChild(enemy);
  }

  public void SpawnEnemies() {
    Random rng = new Random();
    for (int i = 0; i < BASE_ENEMIES + globals.currentStage; ++i) {
      SpawnEnemy(700 + rng.Next(0,5000), rng.Next((int)GetViewportRect().Size.y));
    }
  }

  public void SpawnBossWave() {
    Random rng = new Random();
    for (int i = 0; i < BASE_ENEMIES; ++i) {
      SpawnEnemy(3800 + rng.Next(0,220), rng.Next((int)GetViewportRect().Size.y));
    }
  }
}
