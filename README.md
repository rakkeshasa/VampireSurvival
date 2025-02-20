# VampireSurvival
![main](https://github.com/user-attachments/assets/22359ac1-a44d-47aa-aac1-553c9e7a3406)</br>
Unity로 뱀파이어 서바이벌 모작 제작</br>

목차
---
- [간단한 소개](#간단한-소개)
- [플레이 영상](#플레이-영상)

## 간단한 소개

</BR>
Vampire Survivor를 모작한 게임입니다.</BR>
플레이어는 자신이 가진 무기를 가지고 몰려오는 몬스터 웨이브를 막아내면서 살아남는게 묵표입니다.</BR>
몬스터를 잡으면 경험치와 골드를 획득할 수 있으며, 레벨업을 할 때마다 무기를 강화하거나 캐릭터를 성장시킬 수 있습니다.</BR></BR>

## 플레이 영상
[![Video Label](http://img.youtube.com/vi/FfWsosPWtG4/0.jpg)](https://youtu.be/FfWsosPWtG4)
</br>
👀Link: https://youtu.be/FfWsosPWtG4</BR>
이미지나 주소 클릭하시면 영상을 보실 수 있습니다. </BR>

## 기능 구현

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

```
private List<DamageUI> DamagePool = new List<DamageUI>();

public void SetDamageUI(float damage, Vector3 location)
{
    DamageUI newUI = SpawnDamageUI();
    newUI.Setup(damage);
    newUI.gameObject.SetActive(true);
    newUI.transform.position = location;

}

public DamageUI SpawnDamageUI()
{
    DamageUI OutputUI = null;

    if(DamagePool.Count == 0)
    {
        OutputUI = Instantiate(damageUI, canvas);
    }
    else
    {
        OutputUI = DamagePool[0];
        DamagePool.RemoveAt(0);
    }

    return OutputUI;
}

public void DespawnDamageUI(DamageUI despawnUI)
{
    despawnUI.gameObject.SetActive(false);
    DamagePool.Add(despawnUI);
}
```
플레이어의 공격이 여러 몬스터에게 맞을 수 있고 몬스터가 피격되면 데미지 수치가 UI에 출력됩니다.</br>
UI를 담당하는 오브젝트에 캔버스를 붙여 캔버스에 맞은 몬스터 수만큼 Text창이 나타나도록 했습니다.</br>
풀링을 이용해 데미지를 출력하는 Text들을 관리했습니다.</br></br>

### HolyZone 무기 구현
HolyZone는 몬스터가 플레이어한테 일정 거리이내에 들어오면 지속적인 피해를 입는 무기입니다.</br>
HolyZone무기에 원형 콜라이더를 붙여 OnTriggerEnter 함수를 통해 가까이 있는 모든 몬스터들을 리스트에 담았습니다.</br>

```
if(infiniteDamager)
{
    damageCounter -= Time.deltaTime;

    if(damageCounter <= 0)
    {
        damageCounter = damageInterval;
        for(int i = 0; i < enemiesInRange.Count; i++)
        {
            if (enemiesInRange[i] != null)
            {
                enemiesInRange[i].TakeDamage(Damage, shouldKnockBack);
            }
            else
            {
                enemiesInRange.RemoveAt(i);
                i--;
            }
        }
    }
}
```
지속 피해를 구현하기 위해 카운터를 통해 몇 틱마다 데미지를 입힐지 계산했으며, 리스트에 담긴 몬스터들을 순회하면서 데미지를 입혔습니다.</br>
몬스터가 무기 구역을 벗어나면 리스트에서 제거해 더 이상 피해를 입지 않게 했습니다.</br></br>

### Dagger 무기 구현
Dagger 무기는 빠르게 일직선 상으로 단검을 던지는 스킬입니다.</br>

```
public LayerMask hitEnemy;

Collider2D[] enemies = Physics2D.OverlapCircleAll(transform.position, weaponRange * stats[weaponLevel].range, hitEnemy);
if(enemies.Length > 0 )
{
    for(int i = 0; i < stats[weaponLevel].amount; i++)
    {
        // 투사체에 맞은 개체 중 랜덤하게 뽑아 데미지 주기
        Vector3 targetPos = enemies[Random.Range(0, enemies.Length)].transform.position;
        
        Vector3 dir = targetPos - transform.position;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        projectile.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        Instantiate(projectile, projectile.transform.position, projectile.transform.rotation).gameObject.SetActive(true);
    }
}
```
Dagger가 몬스터를 향해 방향을 맞추기 위해 몬스터의 위치를 파악해야합니다.</br>
몬스터의 위치는 몬스터에게 Enemy 레이어를 적용해서 무기 레벨의 범위 안에 있는 몬스터를 탐색하고 배열에 넣습니다.</br>
배열에 있는 몬스터 중 랜덤하게 1개를 뽑아 해당 몬스터 방향으로 Dagger를 소환하고 나아가게 합니다.</br></br>

### Sword 무기 구현
Sword 무기는 플레이어로부터 정해진 방향으로 찔러 방향에는 제한이 있지만 데미지는 강력한 무기입니다.</br>

```
for(int i = 0; i < stats[weaponLevel].amount; i++)
{
    float rot = (360f / stats[weaponLevel].amount) * i;
    Instantiate(damager, damager.transform.position, Quaternion.Euler(0f, 0f, damager.transform.rotation.eulerAngles.z + rot), transform).gameObject.SetActive(true);
}
```
무기가 업그레이드 하면서 찌르는 무기가 1개씩 증가하며 일정한 각도로 검을 찌르게 하기 위해 360도를 무기의 개수만큼 나눠 각도를 구했습니다.</br>
이후 해당 각도에 맞춰 무기를 일정 거리만큼 나가게 해 찌르는 것처럼 보이게 했습니다.</br.

### Axe 무기 구현
Axe 무기는 현실에서 도끼를 던지는 것처럼 포물선을 그리며 아래로 떨어지는 무기입니다.</br>

```
public float throwPower;
public Rigidbody2D rb;

rb.linearVelocity = new Vector2(Random.Range(-throwPower, throwPower), throwPower);
transform.rotation = Quaternion.Euler(0f, 0f, transform.rotation.eulerAngles.z + (rotateSpeed * 360f * Time.deltaTime * Mathf.Sign(rb.linearVelocityX)));
```
무기가 포물선을 그리면서 떨어지게 하기 위해 RigidBody를 붙여 중력 효과를 입혔으며 던지는 힘을 따로 받아 RigidBody에 나아가는 힘을 적용했습니다.</br>
또한 무기가 굴러가는거처럼 회전시키기 위해 rotation을 싸인 함수를 이용해 회전시켰습니다.</br></br>


### 몬스터 웨이브 시스템

```
private void Update()
{
    if (PlayerHealthController.instance.gameObject.activeSelf)
    {
        if (currentWave < waves.Count)
        {
            waveCounter -= Time.deltaTime;
            if (waveCounter <= 0)
            {
                GoToNextWave();
            }

            spawnCounter -= Time.deltaTime;
            if (spawnCounter <= 0)
            {
                spawnCounter = waves[currentWave].spawnInterval;
                GameObject newEnemy = Instantiate(waves[currentWave].enemy, SelectSpawnPoint(), Quaternion.identity);
            }
        }
    }

    transform.position = target.position;
}

public void GoToNextWave()
{
    waves[currentWave].spawnInterval -= .2f;
    waves[currentWave].spawnInterval = Mathf.Max(waves[currentWave].spawnInterval, .25f);
    currentWave++;

    if(currentWave >= waves.Count)
    {
        currentWave = 0;
    }

    waveCounter = waves[currentWave].waveDuration;
    spawnCounter = waves[currentWave].spawnInterval;
}
```
몬스터와 웨이브 시간, 몬스터 스폰 시간을 담는 waveInfo 클래스를 생성하여 웨이브를 관리하는 리스트를 생성했습니다.</br>
1개의 웨이브에서 1개의 몬스터만 나오도록 했으며 웨이브 시간이 지나면 다음 웨이브로 넘어가면서 다음 리스트에 있는 몬스터를 스폰합니다.</br>
이를 위해 Update 함수에서 웨이브 시간을 체크하고 웨이브 시간동안 현재 단계의 몬스터를 스폰하도록 했으며 웨이브 시간이 다 됐으면 다음 웨이브로 넘어가게 했습니다.</BR>
다음 웨이브로 넘어가게 해주는 GoToNextWave는 리스트의 인덱스를 증가시키고 다음 웨이브의 시간과 몬스터 스폰 시간을 세팅해줍니다.</br>
만약 리스트를 다 순회했다면 인덱스를 0으로 설정해 다시 처음부터 순회하도록 했으며, 반복됐을 때 더 박진감 넘치는 플레이를 위해 몬스터 스폰 시간을 줄여 더 많은 몬스터가 나오도록 했습니다.</br></br>

### 레벨 시스템

```
void Update()
{
    if(magnetic)
    {
        transform.position = Vector3.MoveTowards(transform.position, PlayerHealthController.instance.transform.position, speed * Time.deltaTime);
    }
    else
    {
        checkCounter -= Time.deltaTime;
        if(checkCounter < 0)
        {
            checkCounter = timeBetweenChecks;

            if (Vector3.Distance(transform.position, PlayerHealthController.instance.transform.position) < player.pickupRange)
            {
                magnetic = true;
                speed += player.moveSpeed;
            }
        }
    }
}

public void GetExp(int exp)
{
    currentEXP += exp;
    if(currentEXP >= expLevels[currentLevel])
    {
        LevelUp();
    }
    UIController.instance.UpdateExp(currentEXP, expLevels[currentLevel], currentLevel);
}
```
몬스터가 죽으면 경험치 코인을 떨어뜨리고, 플레이어가 코인을 획득할 수 있는 반경내에 접근하면 플레이어에게 흡수됩니다.</br>
플레이어에게 흡수되면 트리거 이벤트가 발생하고 GetExp 함수를 호출해 플레이어의 Exp 값을 증가시키고 UI에 적용합니다.</BR>
만약 획득한 경험치를 통해 레벨업할 조건이 된다면 플레이어의 레벨을 올리고, 레벨업하고 남은 경험치를 보존합니다.</BR></BR>

### 무기 업그레이드

```
private void LevelUp()
{
    currentEXP -= expLevels[currentLevel];
    currentLevel++;

    UIController.instance.levelUpPanel.SetActive(true);
    Time.timeScale = 0f;
    UIController.instance.levelUpButtons[1].UpdateButton(PlayerController.instance.activeWeapon);
}

public void LevelUp()
{
    if(weaponLevel < stats.Count - 1)
    {
        weaponLevel++;
        statsUpdated = true;
        if(weaponLevel >= stats.Count - 1)
        {
            PlayerController.instance.maxLevelWepons.Add(this);
            PlayerController.instance.activeWeapons.Remove(this);
        }
    }
}

if(statsUpdated)
{
    damager.Damage = stats[weaponLevel].damage;
    transform.localScale = Vector3.one * stats[weaponLevel].range;
    spawnInterval = stats[weaponLevel].attackInterval;
    damager.lifeTime = stats[weaponLevel].duration;
    spawnCounter = 0;
}
```
플레이어가 레벨업을 하게 되면 무기 업그레이드 창이 뜨며 3개의 선택창에서 업그레이드할 무기를 고르게 됩니다.</br>
업그레이드할 무기를 고르면 해당 무기의 레벨을 올리는 함수를 호출하고 해당 레벨에 맞는 스탯을 갖게 했습니다.</br></br>


```
if(availableWeapons.Count > 0)
{
    int selected = Random.Range(0, availableWeapons.Count);
    upgradeWeapons.Add(availableWeapons[selected]);
    availableWeapons.RemoveAt(selected);
}

if(PlayerController.instance.activeWeapons.Count + PlayerController.instance.maxLevelWepons.Count < PlayerController.instance.maxWeapon)
{
    availableWeapons.AddRange(PlayerController.instance.inactiveWeapons);
}

for(int i = upgradeWeapons.Count; i < 3; i++)
{
    if (availableWeapons.Count > 0)
    {
        int selected = Random.Range(0, availableWeapons.Count);
        upgradeWeapons.Add(availableWeapons[selected]);
        availableWeapons.RemoveAt(selected);
    }
}

for(int i = 0; i < upgradeWeapons.Count; i++)
{
    UIController.instance.levelUpButtons[i].UpdateButton(upgradeWeapons[i]);
}

for(int i = 0; i < UIController.instance.levelUpButtons.Length; i++)
{
    if(i < upgradeWeapons.Count)
    {
        UIController.instance.levelUpButtons[i].gameObject.SetActive(true);
    }
    else
    {
        UIController.instance.levelUpButtons[i].gameObject.SetActive(false);
    }
}
```
플레이어는 총 3개의 무기를 가질 수 있으며 무기 슬롯이 꽉 차면 업그레이드 창밖에 안뜨게 했습니다.</BR>
우선 활성화된 무기를 업그레이드 창에 띄워줄 목록인 availableWeapons 리스트에 넣고 이 중에서도 업그레이드 창에서 랜덤하게 뜰 수 있도록 했습니다.</br>
만약 플레이어가 무기를 3개 미만으로 가지고 있다면 아직 비활성화된 무기를 모두 갖고와 availableWeapons 리스트에 넣어 새로운 무기를 습득할 수 있게 했습니다.</br>
무기가 풀업그레이드가 되면 업그레이드 창에서 안뜨게 하기 위해 따로 maxLevelWeapons를 관리했으며 풀업그레이드 무기는 availableWeapons리스트에 안넣는 대신 따로 수를 카운트해 업그레이드 창이 갱신이 안되도록 했습니다.</br>
업그레이드 창이 갱신이 안될 경우에는 보여줄 필요가 없기 때문에 해당 오브젝트를 비활성화하여 안보이게 했습니다.</br></br>
