using UnityEngine;
using UnityEngine.SceneManagement;
using GlobalThings;

namespace ClearSky
{
    public class SimplePlayerController : MonoBehaviour
    {
        [SerializeField] private float movePower = 10f;
        [SerializeField] private float airGravity;
        [SerializeField] private float maxSpeed = 10f;
        [SerializeField] private int health;

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


        void Start()
        {
            coll = GetComponent<BoxCollider2D>();
            rb = GetComponent<Rigidbody2D>();
            spell = Spells.Dragging;
            anim = GetComponent<Animator>();
        }

        private void Update()
        {
            //mooving
            anim.SetBool("isRun", false);

            if (Input.GetAxisRaw("Horizontal") < 0)
            {
                direction = -1;

                transform.localScale = new Vector3(direction, 1, 1);
                if (!anim.GetBool("isJump"))
                    anim.SetBool("isRun", true);

            }
            if (Input.GetAxisRaw("Horizontal") > 0)
            {
                direction = 1;

                transform.localScale = new Vector3(direction, 1, 1);
                if (!anim.GetBool("isJump"))
                    anim.SetBool("isRun", true);

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

            //dying
            if(health <= 0)
            {
                Destroy(gameObject);
                SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().name);
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

            if(rb.velocity.magnitude <= 5)
            {
                rb.AddForce(new Vector2(horizontal * movePower, airGravity * Physics2D.gravity.y), ForceMode2D.Force);
            }
            if(horizontal == 0)
            {
                rb.velocity = new Vector2(0, 0);
            }
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if(collision.gameObject.tag == "Box" && collision.gameObject.GetComponent<Rigidbody2D>().velocity.magnitude >= 40)
            {
                health--;
            }
        }
    }
}