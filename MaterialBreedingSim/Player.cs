using Godot;
using System;

public class Player : KinematicBody
{
    public int spd_XZ;
    public float dec_XZ;
    public int spd_jump;
    public float acc_grav;
    public Vector3 velocity;
    public float mouse_sens;
    public float cam_angle;

    private Spatial head;
    private Camera cam;
    private RayCast raycast;
    private PackedScene block;
    private InventorySlot selected;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
      head = GetNode<Spatial>("Head");
      cam = GetNode<Camera>("Head/Camera");
      raycast = GetNode<RayCast>("Head/Camera/PlaceBlocksRC");
      block = ResourceLoader.Load<PackedScene>("res://Block.tscn");
      selected = null;

      spd_XZ = 15;
      dec_XZ = 1f;
      spd_jump = 30;
      acc_grav = 2f;
      velocity = new Vector3(0,0,0);
      mouse_sens = 0.8f;
      cam_angle = 0;


    }
    public override void _Input(InputEvent @event) {
      if (@event is InputEventMouseMotion event_mouse_motion) {
     	  head.RotateY(Deg2Rad(-1 * event_mouse_motion.Relative.x * mouse_sens));

      	float dx_rot = event_mouse_motion.Relative.y * mouse_sens;
      	if (cam_angle - dx_rot > -90 && cam_angle - dx_rot < 90) {
      	  cam.RotateX(Deg2Rad(-1 * dx_rot));
      	  cam_angle -= dx_rot;
      	}
      }

      //Place blocks!
      if (@event.IsActionPressed("player_block_place")) {
        if (raycast.IsColliding()) {
          Block bk_inst = (Block)block.Instance();
          bk_inst.Translation = raycast.GetCollisionPoint();
          GetNode("/root/WorldRoot").AddChild(bk_inst);
        }
      }
    }

    public override void _PhysicsProcess(float delta) {
      Basis head_basis = head.Transform.basis;
      Vector3 direction = new Vector3(0,0,0);
      if (Input.IsActionPressed("player_forward")) {
        direction -= head_basis.z;
      }
      if (Input.IsActionPressed("player_backward")) {
        direction += head_basis.z;
      }
      if (Input.IsActionPressed("player_left")) {
	       direction -= head_basis.x;
      }
      if (Input.IsActionPressed("player_right")) {
	       direction += head_basis.x;
     }
      direction = direction.Normalized();
      
      float oldY = velocity.y;
      velocity = spd_XZ * direction;
      velocity.y = oldY;
      if (Input.IsActionPressed("player_jump") && IsOnFloor()) {
        velocity.y += spd_jump;
      } else if (IsOnFloor()) {
        velocity.y = 0;
      } else {
        velocity.y -= acc_grav;
      }

      this.MoveAndSlide(velocity, new Vector3(0,1,0), false, 1, 0f, false);
    }

    private float Deg2Rad(float degrees) {
      return (float)Math.PI * degrees / 180;
    }
}
