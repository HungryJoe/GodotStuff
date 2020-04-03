using Godot;
using System;

public class LightSwitch : Interactable
{
  [Export]
  private NodePath light_path;
  [Export]
  private bool on_by_default = true;
  [Export]
  private float energy_on = 1;
  [Export]
  private float energy_off = 0;
  
  private Light light;
  private bool is_on;


  public override void _Ready() {
    light = GetNode<Light>(light_path);
    is_on = on_by_default;
    SetLightEnergy();
  }

  public override string GetInteractionText() {
    if (is_on) {
      return "turn light off";
    } else {
      return "turn light on";
    }
  }

  public override void Interact() {
    is_on = !is_on;
    SetLightEnergy();
  }

  private void SetLightEnergy() {
    if (is_on) {
      light.LightEnergy = energy_on;
    } else {
      light.LightEnergy = energy_off;
    }
  }

}
