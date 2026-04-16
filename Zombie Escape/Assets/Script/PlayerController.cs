using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float jumpForce = 10f; // Сила стрибка (можна буде міняти в Unity)
    private Rigidbody2D rb;       // Посилання на фізику
    private bool isGrounded = true; // Перевірка, чи стоїмо ми на землі

    void Start()
{
    rb = GetComponent<Rigidbody2D>();
    Time.timeScale = 1; // Гарантуємо, що гра не почнеться на паузі
}

    void Update()
    {
        // Якщо натиснуто Пробіл і ми стоїмо на землі
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            // Штовхаємо гравця вгору
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            isGrounded = false; // Тепер ми в повітрі, стрибати більше не можна
        }
    }

    // Коли гравець з чимось стикається
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Якщо він торкнувся об'єкта з назвою "Ground"
        if (collision.gameObject.name == "Ground")
        {
            isGrounded = true; // Знову дозволяємо стрибати
        }
    }
}