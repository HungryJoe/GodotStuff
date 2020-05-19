using Godot;
using System;

public class Block : MeshInstance
{
    [Export]
    public Texture tex = GD.Load<Texture>("res://icon.png");
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
	SpatialMaterial mat = new SpatialMaterial();
	mat.AlbedoTexture = tex;
	int count = this.GetSurfaceMaterialCount();
	for (int i = 0; i < count; ++i) {
            this.SetSurfaceMaterial(i, mat);
        }
    }
}
