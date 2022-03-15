using UnityEngine;
using System.Collections;
using UnityEngine.Tilemaps;

public class SimplePlatformController : MonoBehaviour
{

    [HideInInspector] public bool facingRight = true;
    [HideInInspector] public bool jump = false;

    public float moveForce = 365f;
    private float maxSpeed = 0.1f;
    public float jumpForce = 1000f;
    public Transform groundCheck;
    private Tilemap map;
    private GameObject player;
    private GameManager gM;
    private bool grounded = false;
    private bool rWall = false;
    private bool lWall = false;
    private Animator anim;
    private Rigidbody2D rb2d;
    private Grid grid;
    private Vector3 velocity;
    private Vector3Int velocityInt;
    private Vector3Int playerPosInt;



    // Use this for initialization
    void Awake()
    {
        gM = GameObject.Find("GameManager").GetComponent<GameManager>();
        anim = GetComponent<Animator>();
        player = GameObject.Find("hero");
        grid = gM.chunkPrefab.GetComponent<Grid>();
    }

    private bool checkWall(Vector3 playerPos)
    {
        bool wall;

        if (map.GetTile(map.WorldToCell(playerPos)) != null && grounded)
        {
            velocity = new Vector3(0, 0, 0);
            wall = true;
        }
        else if (map.GetTile(map.WorldToCell(playerPos)) != null && !grounded)
        {
            velocity += new Vector3(0, -0.01f, 0);
            wall = true;
        }
        else
        {
            wall = false;
        }

        return wall;
    }

    void FixedUpdate()
    {
        Vector3 playerPos = player.transform.position;
        map = gM.GetMapAtPosition(playerPos);
        playerPos.y -= 1;

        if (map.WorldToCell(playerPos).y > playerPos.y && map.GetTile(map.WorldToCell(playerPos)) != null)
        {
            player.transform.position = new Vector3(playerPos.x, playerPos.y + grid.transform.localScale.y, 0);
            grounded = true;
        }
        else
        {
            grounded = false;

        }

        playerPos.y += 1;
        playerPos.x += 0.5f;

        rWall = checkWall(playerPos);

        playerPos.x -= 1f;

        lWall = checkWall(playerPos);

        if (grounded)
        {
            if (!rWall && !lWall)
            {
                velocity = new Vector3(velocity.x, 0, 0);
                if (Input.GetAxis("Horizontal") > 0 && (Mathf.Abs(velocity.x) <= maxSpeed || velocity.x < 0))
                {
                    velocity += new Vector3(0.01f, 0, 0);
                }
                else if (Input.GetAxis("Horizontal") < 0 && (Mathf.Abs(velocity.x) <= maxSpeed || velocity.x > 0))
                {
                    velocity += new Vector3(-0.01f, 0, 0);

                }
                else if (velocity.x != 0 && !Input.GetButton("Horizontal"))
                {
                    velocity -= velocity;
                }

                if (Input.GetButtonDown("Jump"))
                {
                    velocity += new Vector3(0, 0.2f, 0);
                }
            }
            else
            {
                playerPos.x -= 1;
                playerPosInt = map.WorldToCell(playerPos);

                if (map.GetTile(playerPosInt) == null && Input.GetAxis("Horizontal") < 0)
                {
                    velocity += new Vector3(-0.01f, 0, 0);
                }

                playerPos.x += 2;
                playerPosInt = map.WorldToCell(playerPos);

                if (map.GetTile(playerPosInt) == null && Input.GetAxis("Horizontal") > 0)
                {
                    velocity += new Vector3(0.01f, 0, 0);
                }
            }
        }
        if (!grounded)
        {
            if (!rWall && !lWall)
            {
                if (Input.GetAxis("Horizontal") > 0 && Mathf.Abs(velocity.x) <= maxSpeed)
                {
                    velocity += new Vector3(0.01f, -0.01f, 0);
                }
                else if ((Input.GetAxis("Horizontal") < 0) && Mathf.Abs(velocity.x) <= maxSpeed)
                {
                    velocity += new Vector3(-0.01f, -0.01f, 0);
                }
                else
                {
                    velocity += new Vector3(0, -0.01f, 0);
                }
            }
            else
            {
                playerPos.x -= 1;
                playerPosInt = map.WorldToCell(playerPos);

                if (map.GetTile(playerPosInt) == null && Input.GetAxis("Horizontal") < 0)
                {
                    velocity += new Vector3(0, -0.01f, 0);
                }

                playerPos.x += 2;
                playerPosInt = map.WorldToCell(playerPos);

                if (map.GetTile(playerPosInt) == null && Input.GetAxis("Horizontal") > 0)
                {
                    velocity += new Vector3(0, -0.01f, 0);
                }
            }
        }

        player.transform.position += new Vector3(velocity.x, velocity.y, 0);
    }


    void Flip()
    {
        facingRight = !facingRight;
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }
}
