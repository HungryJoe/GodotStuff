using Godot;
using Godot.Collections;
using System.Collections.Generic;
using System;

public class Inventory : HBoxContainer
{
    [Signal]
    public delegate void MaterialChanged(int index);

    private System.Collections.Generic.Dictionary<string, Material> mats;
    private int inv_size;
    private List<InventorySlot> inv;
    private int selected;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
      //Initalize materials hashmap from scratch, for now
      mats = new System.Collections.Generic.Dictionary<string, Material>(4);
      mats.Add("stone", new Material(GD.Load<Texture>("res://textures/stone.png")));
      mats.Add("wood", new Material(GD.Load<Texture>("res://textures/wood.png")));
      mats.Add("metal", new Material(GD.Load<Texture>("res://textures/metal.png")));
      mats.Add("gel", new Material(GD.Load<Texture>("res://textures/gel.png")));

      inv_size = this.GetChildCount();
      inv = new List<InventorySlot>(inv_size);
      Godot.Collections.Array children = this.GetChildren();//Create a deep copy of this array because I don't want to deal with Godot's data structures
      foreach (InventorySlot child in children) {
        inv.Add(child);
      }
      selected = 0;
      inv[selected].selected = true;

      EmitSignal(nameof(MaterialChanged), 0, mats["stone"]);
      EmitSignal(nameof(MaterialChanged), 1, mats["wood"]);
      EmitSignal(nameof(MaterialChanged), 2, mats["metal"]);
      EmitSignal(nameof(MaterialChanged), 3, mats["gel"]);
    }

    public override void _Input(InputEvent @event) {
      if (@event.IsActionPressed("inv_select_next")) {
	inv[selected].selected = false;
	selected = mod(selected + 1, inv_size);
	inv[selected].selected = true;
      } else if (@event.IsActionPressed("inv_select_prev")) {
	inv[selected].selected = false;
	selected = mod(selected - 1, inv_size);
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
