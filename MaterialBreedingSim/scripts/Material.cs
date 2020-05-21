//Material stores relevant data on each of the four materials in this game

using Godot;
using System;

public class Material : Node
{
  public Texture tex;
  public int ml_idx;//This material's index in the GridMap's MeshLibrary

  public Material(string name) {
    this.tex = GD.Load<Texture>(String.Format("res://textures/{0}.png", name));
    MeshLibrary ml = GD.Load<MeshLibrary>("res://Blocks_ML.meshlib");
    this.ml_idx = ml.FindItemByName(name);
  }
}
