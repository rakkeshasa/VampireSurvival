# VampireSurvival
Unity로 뱀서류 게임 만들기

---

### 플레이어 이동 구현

```
Vector3 moveInput = new Vector3(0f, 0f, 0f);
moveInput.x = Input.GetAxisRaw("Horizontal");
moveInput.y = Input.GetAxisRaw("Vertical");
moveInput.Normalize();
transform.position += moveInput * moveSpeed * Time.deltaTime;

if(moveInput != Vector3.zero)
{
    anim.SetBool("IsMoving", true);
}
else
{
    anim.SetBool("IsMoving", false);
}
```
키 입력에 따라 상하좌우 이동을 하며 대각선 이동시 √2배 더 빨라져 Normalize함수를 통해 정규화 했습니다.</br>
입력 값에 따라 애니메이션을 다르게 해주기 위해 Animation 컴포넌트를 가져와 상태를 세팅했습니다.</br></br>

### 몬스터 구현

```
target = FindAnyObjectByType<PlayerController>().transform;
rb = gameObject.GetComponent<Rigidbody2D>();
rb.linearVelocity = (target.position - transform.position).normalized * moveSpeed;
```
PlayerController를 갖는 객체를 찾아 target으로 두고 플레이어의 위치에서 몬스터의 위치를 빼 방향벡터를 구해 플레이어를 쫓아가게 했습니다.</br></br>

### 데미지 주기

```
public class EnemyController : MonoBehaviour
{
    void Update()
    {
        if(hitCounter > 0f)
        {
            hitCounter -= Time.deltaTime;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Player" && hitCounter <= 0f)
        {
            PlayerHealthController.instance.TakeDamage(damage);
            hitCounter = delayTime;
        }
    }
}

public class PlayerHealthController : MonoBehaviour
{
    public static PlayerHealthController instance;

    private void Awake()
    {
        instance = this;
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;

        if (currentHealth < 0)
        {
           gameObject.SetActive(false);
        }

        healthSlider.value = currentHealth;
    }
}
```
PlayerHealthController는 플레이어가 피격시 체력을 깍고 UI의 슬라이더의 수치를 세팅하는 클래스입니다.</br>
싱글톤 방식으로 구현해서 EnemyController가 접근하도록 했습니다. </br>
EnemyController는 플레이어의 콜라이더와 충돌 시 인스턴스를 갖고와서 TakeDamage를 호출합니다.</br>
또한 매번 부딪힐때마다 체력이 빠르게 깍이는 현상을 방지하기 위해 피격된 후 시간을 측정해 일정 시간 뒤에 다시 데미지가 들어가도록 했습니다.</br></br>

