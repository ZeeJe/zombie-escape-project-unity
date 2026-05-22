using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float jumpForce = 10f; 
    private Rigidbody2D rb;       
    private Animator anim;
    private bool isGrounded = true; 

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        Time.timeScale = 1; 
    }

    void Update()
    {
        // Стрибаємо тільки якщо натиснуто Пропуск І ми на землі
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            anim.SetTrigger("Jump");
            anim.SetBool("isGrounded", false); // Кажемо аніматору: ми відірвалися від землі!
            
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            isGrounded = false; 
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Перевіряємо торкання землі
        if (collision.gameObject.name == "Ground")
        {
            isGrounded = true; 
            anim.SetBool("isGrounded", true); // Кажемо аніматору: ура, ми приземлилися, вмикай біг!
        }
    }

    public void Die()
    {
        anim.SetBool("Death", true);
    }
}