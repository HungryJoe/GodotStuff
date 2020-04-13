using Godot;
using System;

public class Camera : Godot.Camera
{
  private bool cap_mouse;

  public override void _Ready()
  {
    cap_mouse = true;
    SwitchMouseMode();
  }

  public override void _Process(float delta)
  {
    if (Input.IsActionJustReleased("player_pause")) {
      cap_mouse = !cap_mouse;
      SwitchMouseMode();
    }
  }

  private void SwitchMouseMode() {
    if (cap_mouse) {
      Input.SetMouseMode(Input.MouseMode.Captured);
    } else {
      Input.SetMouseMode(Input.MouseMode.Visible);
    }
  }
}
