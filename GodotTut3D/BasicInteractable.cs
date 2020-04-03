using Godot;
using System;

public class BasicInteractable : Interactable
{
  [Export]
  private string interaction_text;

  public override void _Ready() {
    interaction_text = "Interact";
  }

  public override string GetInteractionText() {
    return interaction_text;
  }

  public override void Interact() {
    GD.Print(String.Format("Interacted with {0}", Name));
  }
}
