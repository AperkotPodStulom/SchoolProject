using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using GlobalThings;
using Support;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.PackageManager;
using System.Security.Cryptography;

[RequireComponent(typeof(Animator))]
public class SimplePlayerController : MonoBehaviour
{
	[SerializeField] private float movePower = 10f;
	[SerializeField] private float airGravity;
	[SerializeField] private float maxSpeed = 10f;
	[SerializeField] private Transform rayStartPos;
	[SerializeField] private Transform playerPos;
	[SerializeField] private GameObject cum;


	private Rigidbody2D selectedObject;
	private Vector3 mousePosition;
	private Vector3 lastPosition;
	private Vector3 offset;
	private Vector3 mouseForce;
	private Animator anim;
	private Rigidbody2D rb;
	private int direction = 1;
	private Spells spell = new Spells();
	private BoxCollider2D coll;
	private Vector3 cameraVelocity;
	private float defaultSize;
	private Collider2D collideCumPos;
	private LineRenderer lr;
	private List<Vector3> hitPositions = new List<Vector3>();

	private void Awake()
	{
		lr = GetComponent<LineRenderer>();
	}

	private void Start()
	{
		defaultSize = cum.GetComponent<Camera>().orthographicSize;
		coll = GetComponent<BoxCollider2D>();
		rb = GetComponent<Rigidbody2D>();
		spell = Spells.Dragging;
		anim = GetComponent<Animator>();
		collideCumPos = null;
	}

	private void Update()
	{
		//camera mooving
		if (collideCumPos)
		{
			cum.transform.position = Vector3.SmoothDamp(cum.transform.position,
				collideCumPos.gameObject.transform.position, ref cameraVelocity, 0.3f);
		}
		else
		{
			cum.transform.position = Vector3.SmoothDamp(cum.transform.position, new Vector3(playerPos.transform.position.x, playerPos.transform.position.y, cum.transform.position.z), ref cameraVelocity, 0.3f);
			cum.GetComponent<Camera>().orthographicSize = defaultSize;
		}


		//mooving
		anim.SetBool("Running", false);
		anim.SetBool("Idle", true);

		if (Input.GetKey(KeyCode.A))
		{
			anim.SetBool("Idle", false);

			direction = -15;
			transform.localScale = new Vector3(direction, 15, 1);

			anim.SetBool("Running", true);
		}

		if (Input.GetKey(KeyCode.D))
		{
			anim.SetBool("Idle", false);

			direction = 15;
			transform.localScale = new Vector3(direction, 15, 1);

			anim.SetBool("Running", true);
		}



		//spell-casting
		if (GlobalSpells.GlobalSpellState == Spells.Dragging)
		{
			mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			if (selectedObject)
			{
				mouseForce = (mousePosition - lastPosition) / Time.deltaTime;
				mouseForce = Vector2.ClampMagnitude(mouseForce, maxSpeed);
				lastPosition = mousePosition;
			}
			if (Input.GetMouseButtonDown(0))
			{
				Collider2D targetObject = Physics2D.OverlapPoint(mousePosition);
				if (targetObject)
				{
					if (targetObject.gameObject.tag == "Box")
					{
						selectedObject = targetObject.transform.gameObject.GetComponent<Rigidbody2D>();
						offset = selectedObject.transform.position - mousePosition;
					}
				}
			}
			if (Input.GetMouseButtonUp(0) && selectedObject)
			{
				selectedObject.AddForce(mouseForce * selectedObject.gravityScale, ForceMode2D.Impulse);
				selectedObject = null;
			}
		}

		//prototype of the second spell
		if (Input.GetMouseButton(1))
		{
			Vector3 direction = new Vector3(mousePosition.x - rayStartPos.position.x, mousePosition.y - rayStartPos.position.y);
			Vectors.Normalize(ref direction);

			RaycastHit2D[] raycast = Physics2D.RaycastAll(rayStartPos.position, direction);
			foreach (var hit in raycast)
			{
				if (hit.collider.tag == "Box")
					hit.collider.GetComponent<Rigidbody2D>().AddForce(new Vector3(direction.x * 60, direction.y * 60), ForceMode2D.Impulse);
			}
		}
	}
	
	private void FixedUpdate()
	{
		if (selectedObject)
		{
			selectedObject.MovePosition(mousePosition + offset);
		}

		MovementLogic();


	}
	private void MovementLogic()
	{
		float horizontal = Input.GetAxisRaw("Horizontal");
		rb.AddForce(new Vector3(horizontal, 0f));
	}

	private void OnTriggerEnter2D(Collider2D col)
	{
		if (col.gameObject.tag == "CameraPoint")
		{
			collideCumPos = col;
		}
	}

	private void OnTriggerExit2D(Collider2D collider)
	{
		if (collider.gameObject.tag == "CameraPoint")
			collideCumPos = null;
	}
}
