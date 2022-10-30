using UnityEngine;

namespace ClearSky
{
    enum Spells
    {
        Dragging
    }

    public class SimplePlayerController : MonoBehaviour
    {
        [SerializeField] private float movePower = 10f;

        private Animator anim;
        private int direction = 1;
        private Spells spell = new Spells();


        void Start()
        {
            anim = GetComponent<Animator>();
        }

        private void Update()
        {
            Vector3 moveVelocity = Vector3.zero;
            anim.SetBool("isRun", false);


            if (Input.GetAxisRaw("Horizontal") < 0)
            {
                direction = -1;
                moveVelocity = Vector3.left;

                transform.localScale = new Vector3(direction, 1, 1);
                if (!anim.GetBool("isJump"))
                    anim.SetBool("isRun", true);

            }
            if (Input.GetAxisRaw("Horizontal") > 0)
            {
                direction = 1;
                moveVelocity = Vector3.right;

                transform.localScale = new Vector3(direction, 1, 1);
                if (!anim.GetBool("isJump"))
                    anim.SetBool("isRun", true);

            }
            transform.position += moveVelocity * movePower * Time.deltaTime;
        }
    }
}