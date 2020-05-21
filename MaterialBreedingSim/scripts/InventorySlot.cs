//Inventory slot controls each slot in the hotbar.
//Specifically, stores the material each slot holds (if any) and whether
//or not they're selected.

using Godot;
using System;

public class InventorySlot : Panel
{
  [Export]
  public readonly float ALPHA_SELECTED = 1f;
  [Export]
  public readonly float ALPHA_UNSELECTED = 0.5f;
  public Material mat;

  private bool selected;
  private TextureRect displayMaterial;
  private int index;

  // Called when the node enters the scene tree for the first time.
  public override void _Ready()
  {
    mat = null;
    index = Int32.Parse(this.Name);
    displayMaterial = GetNode<TextureRect>("Inside/Material");
    selected = true;

    Node inv = GetNode("..");
    inv.Connect("MaterialChanged", this, "ChangeMaterial");
    inv.Connect("SelectChanged", this, "ChangeSelected");
  }

  //Called when Inventory emits its MaterialChanged signal
  public void ChangeMaterial(int index, Material mat) {
    if (index == this.index) {
      this.mat = mat;
      displayMaterial.Texture = mat.tex;
    }
  }

  //Called when Inventory emits its SelectChanged signal
  public void ChangeSelected(int index) {
    //If index == this.index, this is selected now and wasn't before
    if (this.index == index) {
      //Now selected
      selected = true;
      Color c = this.Modulate;
      c.a = ALPHA_SELECTED;
      this.Modulate = c;
    //Else, if selected == true, this was selected and now isn't
    } else if (selected) {
      //Was selected, now isn't
      selected = false;
      Color c = this.Modulate;
      c.a = ALPHA_UNSELECTED;
      this.Modulate = c;
    }
  }
}
