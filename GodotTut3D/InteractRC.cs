using Godot;
using System;

public class InteractRC : RayCast
{
  private Label interact_label;
  private string interact_text;
  private string interact_key;

  public override void _Ready() {
    interact_label = GetNode<Label>("/root/World/UI/CanInteract");
    interact_text = null;
    interact_key = ((InputEvent)InputMap.GetActionList("player_interact")[0]).AsText();

    SetInteractionText(null);
  }
  
  public override void _Process(float delta) {
    interact_text = null;

    if (IsColliding()) {
      Godot.Object collider = GetCollider();
      if (collider is Interactable curr_interactor) {
        interact_text = curr_interactor.GetInteractionText();

        if (Input.IsActionJustPressed("player_interact")) {
          curr_interactor.Interact();
        }
      }
    }
    
    SetInteractionText(interact_text);
  }

  private void SetInteractionText(string text) {
    if (text == null) {
      interact_label.Text = "";
      interact_label.PercentVisible = 0;
    } else {
      interact_label.PercentVisible = 1;
      interact_label.Text = String.Format("Press {0} to {1}", interact_key, text);
    }
  }
}
