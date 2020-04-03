using Godot;
using System;

public abstract class Interactable : Node
{

  public abstract string GetInteractionText();

  public abstract void Interact();
}
