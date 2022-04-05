using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
	CharacterController controller;
	AudioSource source;

	Vector3 velocity;
	bool isGrounded;
	bool isMoving;
	public bool isLeaning;

	public Transform ground;
	public float distance = 0.3f;

	public float speed;
	public float jumpHeight;
	public float gravity;

	public float originalHeight;
	public float crouchHeight;

	public bool canMove = true;

	public LayerMask mask;

	public AudioClip[] defaultSounds;
	public AudioClip[] grassSounds;
	public AudioClip[] snowSounds;

	int soundControl;

	// 0 = Default
	// 1 = Grass
	// 2 = Snow

	public float timeBetweenSteps;
	float timer;

	private void Start()
	{
		controller = GetComponent<CharacterController>();
		source = GetComponent<AudioSource>();
	}

	private void OnControllerColliderHit(ControllerColliderHit hit)
	{
		//switch (hit.transform.tag)
		//	{
		//	case "Ground": soundControl = 0; break;
		//	case "Grass":  soundControl = 1; break;
		//		case "Snow":   soundControl = 2; break;
		//	}
	}

	private void Update()
	{
		#region Movement
		float horizontal = Input.GetAxis("Horizontal");
		float vertical = Input.GetAxis("Vertical");
		
		Vector3 move = transform.right * horizontal + transform.forward * vertical;

		if(canMove)
			controller.Move(move * speed * Time.deltaTime);
		#endregion

		//	#region Footsteps
		//	if (horizontal != 0 || vertical != 0)
		//		isMoving = true;
		//else
		//isMoving = false;

		//// (isMoving && isGrounded && !isLeaning)
		//
		// -= Time.deltaTime;

		//(timer <= 0)
		//
		//switch (soundControl)
		//	{
		//	case 0:	source.clip = defaultSounds[Random.Range(0, defaultSounds.Length)];	break;
		//v		case 1:	source.clip = grassSounds[Random.Range(0, grassSounds.Length)];	    break;
		//		case 2:	source.clip = snowSounds[Random.Range(0, snowSounds.Length)];	    break;
		//	}

		//	timer = timeBetweenSteps;
		//source.pitch = Random.Range(0.85f, 1.15f);
		//		source.Play();
		//		//}
		//	}
		//	else
		//	{
		//		timer = timeBetweenSteps;
		//	}

		//	#endregion

		#region Jump
		if (Input.GetKeyDown(KeyCode.Space) && isGrounded && !isLeaning)
		{
			velocity.y += Mathf.Sqrt(jumpHeight * -2.0f * gravity);
		}
		#endregion

		#region Gravity
		isGrounded = Physics.CheckSphere(ground.position, distance, mask);

		if(isGrounded && velocity.y < 0)
		{
			velocity.y = -2f;
		}

		velocity.y += gravity * Time.deltaTime;
		controller.Move(velocity * Time.deltaTime);
		#endregion

		#region Basic Crouch
		if (Input.GetKeyDown(KeyCode.LeftControl))
		{
			controller.height = crouchHeight;
			speed = 2.0f;
			jumpHeight = 0;
		}

		if (Input.GetKeyUp(KeyCode.LeftControl))
		{
			speed = 5.0f;
			jumpHeight = 2;
			controller.height = originalHeight;
		}
		#endregion

		#region Basic Running
		if (Input.GetKeyDown(KeyCode.LeftShift))
		{
			speed = 7.0f;
			timeBetweenSteps = 0.3f;
		}

		if (Input.GetKeyUp(KeyCode.LeftShift))
		{
			speed = 5.0f;
			timeBetweenSteps = 0.5f;
		}
		#endregion
	}
}
