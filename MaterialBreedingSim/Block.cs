using Godot;
using System;

public class Block : StaticBody
{
    [Export]
    public Texture tex = GD.Load<Texture>("res://icon.png");
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        MeshInstance mesh = GetNode<MeshInstance>("Mesh");
	SpatialMaterial mat = new SpatialMaterial();
	mat.AlbedoTexture = tex;
	int count = mesh.GetSurfaceMaterialCount();
	for (int i = 0; i < count; ++i) {
            mesh.SetSurfaceMaterial(i, mat);
        }
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}
