using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using GlobalThings;
using Support;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.PackageManager;

[RequireComponent(typeof(LineRenderer))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
public class SimplePlayerController : MonoBehaviour
    {
        [SerializeField] private float movePower = 10f;
        [SerializeField] private float airGravity;
        [SerializeField] private float maxSpeed = 10f;
        [SerializeField] private Transform rayStartPos;
        [SerializeField] private Transform playerPos;
        [SerializeField] private GameObject cum;
        [SerializeField] private int maxLength;
        [SerializeField] private int maxReflectionCount;
        

        private Rigidbody2D selectedObject;
        private Vector3 mousePosition;
        private Vector3 lastPosition;
        private Vector3 offset;
        private Vector2 mouseForce;
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
            if(GlobalSpells.GlobalSpellState == Spells.Dragging)
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
                        if(targetObject.gameObject.tag == "Box")
                        {
                            selectedObject = targetObject.transform.gameObject.GetComponent<Rigidbody2D>();
                            offset = selectedObject.transform.position - mousePosition;
                        }
                    }
                }
                if (Input.GetMouseButtonUp(0) && selectedObject)
                {
                    selectedObject.velocity = Vector2.zero;
                    selectedObject.AddForce(mouseForce, ForceMode2D.Impulse);
                    selectedObject = null;
                }
            }
            
            //prototype of the second spell
            if (Input.GetMouseButton(1))
            {
                mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                Vector3 direction = new Vector3(mousePosition.x - rayStartPos.position.x, mousePosition.y - rayStartPos.position.y);
                Vectors.Normalize(ref direction);

                hitPositions.Clear();
                hitPositions.Add(rayStartPos.position);

                Ray2D ray2d = new Ray2D(rayStartPos.position, direction);
                
                while(hitPositions.Count <= maxReflectionCount)
                {
                    RaycastHit2D hitInfo = Physics2D.Raycast(ray2d.origin, ray2d.direction);

                    if (hitInfo.collider != null)
                    {
                        hitPositions.Add(hitInfo.point);

                        ray2d.origin = hitInfo.point - ray2d.direction * 0.01f;
                        ray2d.direction = Vector2.Reflect(ray2d.direction, hitInfo.normal);
                    }
                    else
                    {
                        hitPositions.Add(ray2d.origin + ray2d.direction * maxLength);
                        break;
                    }
                }

                lr.startWidth = 1f;
                lr.endWidth = 1f;
                lr.positionCount = hitPositions.Count;
                lr.SetPositions(hitPositions.ToArray());
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

            rb.AddForce(new Vector2(horizontal * movePower, airGravity * Physics2D.gravity.y), ForceMode2D.Force);
            if(rb.velocity.magnitude >= 5)
            {
                rb.velocity = new Vector2(0, 0);
            }

            if (horizontal == 0)
            {
                rb.velocity = new Vector2(0, 0);
            }
        }

        private void OnTriggerEnter2D(Collider2D col)
        {
            if(col.gameObject.tag == "CameraPoint")
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
