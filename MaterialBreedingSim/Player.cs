using Godot;
using System;

public class Player : KinematicBody
{
    public int spd_XZ;
    public int spd_jump;
    public int acc_XZ;
    public float acc_grav;
    public Vector3 velocity;
    public float mouse_sens;
    public float cam_angle;

    private Spatial head;
    private Camera cam;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
      head = GetNode<Spatial>("Head");
      cam = GetNode<Camera>("Head/Camera");

      spd_XZ = 10;
      spd_jump = 30;
      acc_XZ = 5;
      acc_grav = 0.98f;
      velocity = new Vector3(0,0,0);
      mouse_sens = 0.3f;
      cam_angle = 0f;
    }

    public override void _Input(InputEvent @event) {
      if (@event is InputEventMouseMotion event_mouse_motion) {
     	  head.RotateY(Deg2Rad(-1 * event_mouse_motion.Relative.x * mouse_sens));

      	float dx_rot = event_mouse_motion.Relative.y * mouse_sens;
      	if (cam_angle - dx_rot > -90f && cam_angle - dx_rot < 90) {
      	  cam.RotateX(Deg2Rad(-1 * dx_rot));
      	  cam_angle -= dx_rot;
      	}
      }
    }

    private float Deg2Rad(float degrees) {
      return (float)Math.PI * degrees / 180;
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

      velocity = velocity.LinearInterpolate(direction * spd_XZ, acc_XZ * delta);
      if (Input.IsActionJustPressed("player_jump") && IsOnFloor()) {
        velocity.y += spd_jump;
      }

      velocity.y -= acc_grav;

      velocity = this.MoveAndSlide(velocity, new Vector3(0,1,0));
    }
}
