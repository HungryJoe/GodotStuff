using Godot;
using Godot.Collections;
using System.Collections.Generic;

public class Inventory : HBoxContainer
{
    [Export]
    public readonly int INVENTORY_SIZE = 8;
    private List<InventorySlot> inv;
    private int selected;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
      inv = new List<InventorySlot>(9);
      Array children = this.GetChildren();//Create a deep copy of this array because I don't want to deal with Godot's data structures
      foreach (InventorySlot child in children) {
        inv.Add(child);
      }
      selected = 0;
      inv[selected].selected = true;
    }

    public override void _Input(InputEvent @event) {
      if (@event.IsActionPressed("inv_select_next")) {
	inv[selected].selected = false;
	selected = mod(selected + 1, INVENTORY_SIZE);
	inv[selected].selected = true;
      } else if (@event.IsActionPressed("inv_select_prev")) {
	inv[selected].selected = false;
	selected = mod(selected - 1, INVENTORY_SIZE);
	inv[selected].selected = true;
      }
    }

    private int mod(int a, int modulus) {
      int residue = a % modulus;
      while (residue < 0) {
	residue += modulus;
      }
      return residue;
    }
}
