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

### 몬스터 스폰하기

```
private Queue<GameObject> enemyPool = new Queue<GameObject>();

void Start()
{
    for(int i = 0; i < initialPoolSize; i++)
    {
        GameObject enemy = Instantiate(enemyPrefab);
        enemy.SetActive(false);
        enemyPool.Enqueue(enemy);
    }
}
```
오브젝트 풀링을 이용하여 몬스터들을 관리했습니다.</br>
몬스터를 어느정도 구현하고 SetActive를 false로 하여 우선 화면상에서 안보이게 하고 큐에 담았습니다.

```
InvokeRepeating(nameof(SpawnEnemy), 0f, spawnCounter);

void SpawnEnemy()
{
    GameObject enemy = enemyPool.GetEnemy(SelectSpawnPoint());
}

public GameObject GetEnemy(Vector3 spawnPosition)
{
    GameObject enemy;
    if (enemyPool.Count > 0)
    {
        enemy = enemyPool.Dequeue();
    }
    else
    {
        enemy = Instantiate(enemyPrefab); // 풀에 남은 게 없으면 새로 생성
    }

    enemy.transform.position = spawnPosition;
    enemy.SetActive(true);
    return enemy;
}
```
몬스터 스포너는 유니티 내장함수를 통해 소환시간을 카운트하며 몬스터를 큐에서 하나씩 꺼내 활성화시킵니다.

```
void Update()
{
    if (Vector3.Distance(target.position, transform.position) > despawnDistance)
    {
        enemyPool.ReturnEnemy(gameObject); // 몬스터 반환
    }
}

public void ReturnEnemy(GameObject enemy)
{
    enemy.SetActive(false);
    enemyPool.Enqueue(enemy);
}
```
생성된 몬스터들은 자신과 플레이어의 거리를 체크하고 일정 거리만큼 멀어졌다면 비활성화하게 됩니다.

### FireBall 스킬 구현

![fireball](https://github.com/user-attachments/assets/6a43cf1e-af77-4e45-879f-6b5618fa51a2)
</br>
Fireball 스킬은 플레이어의 주변을 돌아다니는 화염구입니다.</br>
플레이어에게 Weapons 객체를 부착하고 회전하는 용도로 사용할 RoundWeapon객체를 Weapon의 자식 객체로 만들었습니다.

```
void Update()
{
    holder.rotation = Quaternion.Euler(0f, 0f, holder.rotation.eulerAngles.z + (rotateSpeed * Time.deltaTime));
    
    spawnCounter -= Time.deltaTime;
    if (spawnCounter <= 0)
    {
        spawnCounter = timeBetweenSpawn;

        Instantiate(fireballToSpawn, fireballToSpawn.position, fireballToSpawn.rotation, holder).gameObject.SetActive(true);
    }
}
```
RoundWeapon은 화염구를 담을 Holder 객체를 가지고 있으며 이 Holder 객체를 매 프레임마다 회전시킵니다.</br>
또한 일정 시간이 지나면 화염구를 소환시키도록 했습니다. </br>

```
void Update()
{
    transform.localScale = Vector3.MoveTowards(transform.localScale, targetSize, growSpeed * Time.deltaTime);
    
    lifeTime -= Time.deltaTime;
    if(lifeTime <= 0)
    {
        targetSize = Vector3.zero;

        if(transform.localScale.x == 0f)
        {
            Destroy(gameObject);
        }
    }
}

private void OnTriggerEnter2D(Collider2D collision)
{
    if(collision.tag == "Enemy")
    {
        collision.GetComponent<EnemyController>().TakeDamage(Damage, shouldKnockBack);
    }
}
```
화염구에게 시각적 효과를 주기 위해 스폰시 점점 커졌다가 일정 크기만큼 커지면 다시 작아지고 파괴됩니다.</br>
화염구가 부딪힌 대상이 EnemyController를 가진 객체라면 TakeDamage를 호출해 몬스터에게 피해를 입힙니다.</br>

```
void Update()
{
    if(knockBackCounter > 0)
    {
        knockBackCounter -= Time.deltaTime;

        if(moveSpeed > 0)
        {
            moveSpeed = -moveSpeed * 2f;
        }

        if(knockBackCounter <= 0)
        {
            moveSpeed = Mathf.Abs(moveSpeed * .5f);
        }
    }
}
```    
또한 몬스터가 넉백류 스킬에 맞으면 넉백시간동안 반대 방향으로 밀려나도록 했으며, 넉백동안에는 반대 반향으로 밀려나도 sprite가 좌우반전이 안일어나도록 했습니다.</br>
