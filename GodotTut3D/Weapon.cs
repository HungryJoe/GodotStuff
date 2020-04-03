using Godot;
using System;

public class Weapon : Node
{
  [Export]
  private float fire_rate = 2;
  [Export]
  private int clip_size = 10;
  [Export]
  private float reload_time = 1;
  [Export]
  private int fire_range = 50;

  private int current_ammo;
  private Timer shootCD;
  private Timer reloadCD;
  private bool reloading;
  private RayCast raycast;
  private Label ammo_label;

  public override void _Ready() {
    current_ammo = clip_size;
    shootCD = GetNode<Timer>("ShootCoolDown");
    shootCD.WaitTime = 1/fire_rate;
    reloadCD = GetNode<Timer>("ReloadCoolDown");
    reloadCD.WaitTime = reload_time;
    reloading = false;
    raycast = GetNode<RayCast>("../Head/Camera/WeaponRC");
    raycast.CastTo = new Vector3(0, 0, -1 * fire_range);
    ammo_label = GetNode<Label>("/root/World/UI/Ammo");
  }

  public override void _Process(float delta) {
    ammo_label.Text = String.Format("{0:D3} / {1:D3}", current_ammo, clip_size);

    if (Input.IsActionJustPressed("player_primary_fire") && CanFire()) {
      Fire();
    } else if (Input.IsActionJustPressed("player_reload") && !reloading) {
      GD.Print("Reloading");
      reloadCD.Start();
      reloading = true;
    }

    if (reloading && reloadCD.TimeLeft == 0) {
      reloading = false;
      current_ammo = clip_size;
      GD.Print("Reloaded");
    }
  }

  private void Fire() {
    GD.Print("Fired Weapon");
    shootCD.Start();
    current_ammo -= 1;

    if (raycast.IsColliding()) {
      Godot.Object collider = raycast.GetCollider();
      if (collider is Node collider_node && collider_node.IsInGroup("targets")) {
        GD.Print(String.Format("Killed {0}", collider_node.Name));
        collider_node.QueueFree();
      }
    }
  }


  private bool CanFire() {
    return shootCD.TimeLeft == 0 && current_ammo > 0 && !reloading;
  }
}
