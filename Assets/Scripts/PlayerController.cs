using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Shapes2D;

// By: Noah McDougall & Nicolas Assakura Miyazaki


public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private FreezeProjectile projectilePrefab;
    [SerializeField]
    private Vector2 projectileVelocity;
    [SerializeField]
    [Tooltip("0 = spawn inside the player, 1 = spawn one unit in front of the player, etc.")]
    private float horizontalProjSpawnOffset;
    [SerializeField]
    private Shape shootIndicator;
    [SerializeField]
    private Color shootCooldownColor;
    [SerializeField]
    private Color shootRegularColor;
    
    [Tooltip("In seconds")]
    public float projectileCooldown;

    private float projFireTimer;
    private bool canFireProj;

    [SerializeField]
    private Rigidbody2D player;  //The rigid body is a Unity class that is used for physics objects. We can apply forces to move a rigidbody
    private Vector3 m_ToApplyMove;
    [SerializeField]
    private float m_jumpForce; //This is the force that we will want to apply to the rigidbody
    [SerializeField]
    private float m_SpeedForce; //This is the force that we will want to apply to the rigidbody
    
    [SerializeField]
    private Text scoreDisplay; //This is a Unity UI Text Object that you can display the score in by setting the text field of this object.
    [SerializeField] private Text healthDisplay;
    [SerializeField] private Text jumpMeterDisplay;
    [SerializeField] public int health;
    [SerializeField]
    private PlayerAudioManager audioManager;
    public int score; //An internal field to store the score in.
    public GameObject GameOver;
    private float scoreTimer = 0.0f;
    private Animator m_Anim;
    private SpriteRenderer m_Renderer;

    [SerializeField] public int jumpMeter;
    [SerializeField] private int jumpRefreshTime;
    //Does the jump timer:
    private float jumpTimer = 0.0f;
    // Start is called before the first frame update
    
    void Start()
    {
        //Here is where you should initalize fields.
        score = 0;
        scoreDisplay.text = $"{score}";
        healthDisplay.text = health.ToString();
        jumpMeterDisplay.text = jumpMeter.ToString();
        projFireTimer = 0;
        canFireProj = true;
        m_Anim = GetComponent<Animator>();
        m_Renderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    private void Update()
    {
        scoreTimer += Time.deltaTime;
        if(scoreTimer > 5)
        {
        scoreTimer -= scoreTimer;
        score += 100;
        scoreDisplay.text = $"{score}";
        }
        jumpMeterDisplay.text = jumpMeter.ToString();
        if(jumpMeter < 5 )
        {
            jumpTimer += Time.deltaTime;
            if(jumpTimer > jumpRefreshTime)
            {
                jumpMeter += 1;
                jumpTimer -= jumpTimer;
                
            }
        }
        //Detect if the key is pressed down.
        if(jumpMeter > 0)
        {
            if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
            {
                m_ToApplyMove += new Vector3(0, m_jumpForce, 0);
                jumpMeter -= 1;
            }
        }
        
        if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
        {
            m_ToApplyMove += new Vector3(-m_SpeedForce, 0, 0);
            if (m_Renderer != null)
            {
                m_Renderer.flipX = true;
            }
        }
        if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
        {
            m_ToApplyMove += new Vector3(m_SpeedForce, 0, 0);
            if (m_Renderer != null)
            {
                m_Renderer.flipX = false;
            }
        }
        
        if(health < 1)
        {
            Destroy(this.gameObject);
            GameStateManager.GameOver();
        }

        // Logic for shooting the projectile
        if (Input.GetKeyDown(KeyCode.Space) && canFireProj)
        {
            Vector3 spawnLocation = new Vector3(transform.position.x + horizontalProjSpawnOffset,
                                                transform.position.y,
                                                0);
            Instantiate(projectilePrefab, spawnLocation, transform.rotation).Fire(projectileVelocity);
            canFireProj = false;

            audioManager.PlayClip(0);
            shootIndicator.settings.fillColor = shootCooldownColor;
            shootIndicator.settings.endAngle = 1;
        }

        // Logic for projectile cooldown (to prevent spam)
        if (!canFireProj)
        {
            projFireTimer += Time.deltaTime;
            shootIndicator.settings.endAngle = Mathf.Lerp(1, 360, projFireTimer / projectileCooldown);
            if (projFireTimer >= projectileCooldown)
            {
                shootIndicator.settings.fillColor = shootRegularColor;
                projFireTimer = 0;
                canFireProj = true;
            }
        }
        
        
    }
    private void FixedUpdate()
    {
        player.AddForce(m_ToApplyMove);
        m_ToApplyMove = Vector3.zero;

    }

    public void AddScore(int toAdd)
    {
        score += toAdd;
        scoreDisplay.text = $"{score}";
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {

        //This function gets called when the collider on this object comes in contact with another collider.

        //If the player runs into a pillar object we want to end the game
        
        
        if (collision.gameObject.CompareTag("GameOver"))
        {
            Destroy(this.gameObject);
            GameStateManager.GameOver();
        }


      
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {


        //This method gets called when this object collides with another object that has it's collider set to "trigger" mode.

        if (collision.gameObject.CompareTag("GameOver"))
        {
            Destroy(this.gameObject);
            GameStateManager.GameOver();
        }

        //If the player enters a score trigger we want to increase the score and update the score display

        //If the player falls out of the world we want to end the game.

        

        if (collision.gameObject.tag == "Enemy")
        {
            health -= 1;
            healthDisplay.text = health.ToString();
        }

    }
}
