# VampireSurvival
![main](https://github.com/user-attachments/assets/22359ac1-a44d-47aa-aac1-553c9e7a3406)</br>
Unity로 뱀파이어 서바이벌 모작 제작</br>

목차
---
- [간단한 소개](#간단한-소개)
- [플레이 영상](#플레이-영상)

## 간단한 소개
![intro](https://github.com/user-attachments/assets/e6cc4db2-19d7-4a5d-a1b6-f18d8ca7d749)

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

### 몬스터 웨이브 구현

몬스터 웨이브를 플레이어의 시야밖에서 소환되어 캐릭터에게 접근하도록 하기 위해 카메라 범위 밖에 좌표를 두었습니다.</br>

![spawner](https://github.com/user-attachments/assets/d162e665-b0b0-471c-bcca-3b4e2c72faae)
</br></br>

```
bool spawnVerticalEdge = Random.Range(0f, 1f) > .5f;
if(spawnVerticalEdge)
{
    spawnPoint.y = Random.Range(minSpawn.position.y, maxSpawn.position.y);
    if (Random.Range(0f, 1f) > .5f)
        spawnPoint.x = maxSpawn.position.x;
    else
        spawnPoint.x = minSpawn.position.x;
}
else
{
    spawnPoint.x = Random.Range(minSpawn.position.x, maxSpawn.position.x);
    if (Random.Range(0f, 1f) > .5f)
        spawnPoint.y = maxSpawn.position.y;
    else
        spawnPoint.y = minSpawn.position.y;
}
```
Random클래스를 이용해 몬스터의 스폰 위치를 무작위로 결정되게 했습니다.</br>
처음에는 스폰위치를 X축과 Y축 중 하나를 선택하고, 선택된 축에서 랜덤한 좌표를 하나 뽑았습니다.</BR>
예시로 X축이 선택됐고, X축에서 (3, Y)를 Random을 통해 선택했다면 Y에 들어갈 수 있는 좌표는 MinPoint와 MaxPoint의 y좌표입니다.</br>
두 개의 포인트 중 한 개를 골라 몬스터가 여러 방향에서 무작위적으로 튀어나오게 했습니다.</br></br>

몬스터 웨이브는 WaveInfo클래스를 통해 웨이브 관련 정보들을 관리하도록 했습니다.</br>
WaveInfo는 스폰될 몬스터의 프리팹, 웨이브의 시간, 스폰 주기 정보를 갖고 있으며, 웨이브가 넘어갈 수록 스폰 주기를 단축시켜 게임 후반부에는 더 많은 몬스터가 몰려올 수 있도록 했습니다.</br>

```
if (PlayerController.instance.gameObject.activeSelf)
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
```
플레이어가 죽어서 사라지면 더 이상 몬스터가 스폰되지 않도록 PlayerController를 갖는 gameObject가 활성화상태인지 우선 체크하도록 했습니다.</br>
우선 현재 웨이브 시간이 남았나 확인하며 남지 않았다면 다음 웨이브로 갈 수 있도록 WaveInfo타입 리스트의 인덱스를 증가시켜 다음 WaveInfo정보를 갖고오게 했습니다.</br>
웨이브 시간이 남아있다면 스폰 주기에 따라 몬스터를 1마리씩 게임에 동적으로 생성시킵니다.</br>
이번 웨이브의 시간이 10초고 스폰 주기가 1초라면, 10초동안 같은 몬스터가 1초마다 동적으로 생성됩니다.</br></br>

동적으로 생성된 몬스터는 EnemyController를 갖고 있으며 플레이어의 좌표를 PlayerController의 transform으로 얻어옵니다.</br>
플레이어의 속도가 몬스터보다 빨라 몬스터가 너무 멀어졌다면 자신의 좌표와 플레이어의 좌표를 통해 거리를 계산해 일정 거리만큼 멀어졌다면 Destroy하도록 했습니다.</br></br>

### 데미지 주기

몬스터 스폰 시스템을 통해 동적으로 생성된 몬스터는 플레이어에게 접근하며 몬스터가 가진 콜라이더가 플레이어의 콜라이더에 부딪히면 데미지를 입히도록 했습니다.</br>
따라서 EnemyController에 콜라이더가 충돌시 호출되는 OnCollisionEnter함수를 이용해 처리했습니다.</br>

```
private void OnCollisionEnter2D(Collision2D collision)
{
    if(collision.gameObject.tag == "Player")
    {
        PlayerHealthController.instance.TakeDamage(damage);
    }
}

public void TakeDamage(float damage)
{
    currentHealth -= damage;
    if (currentHealth < 0)
       gameObject.SetActive(false);

    healthSlider.value = currentHealth;
}
```
몬스터와 부딪힌 gameObject가 Player 태그를 갖고 있는지 체크해 플레이어 캐릭터인지 확인합니다.</br>
플레이어 캐릭터라면 데미지를 처리하는 함수를 통해 플레이어의 현재 체력을 감소시켰습니다.</br>
반대로 플레이어의 무기에 몬스터를 피해를 입히기 위해 모든 무기에 콜라이더를 적용시켜 몬스터와의 충돌을 확인할 수 있게 했습니다.</br>

```
public void TakeDamage(float damage)
{
    health -= damage;

    if(health <= 0)
    {
        Destroy(gameObject);
        PlayerLevelController.instance.SpawnExp(transform.position, exp);
        CoinController.instance.DropCoin(transform.position, coinValue);
    }
    if(shouldKnockback)
        knockBackCounter = knockBackTime;

    DamageUIController.instance.SetDamageUI(damage, transform.position);
}
```
몬스터는 죽으면 경험치와 코인을 바닥에 떨어뜨리고, 만약 피해입은 무기가 넉백류 스킬이라면 넉백 시간을 체크하는 카운터를 두어 Update문에서 넉백 시간동안 몬스터가 밀려나도록 했습니다.</br></br>

### 데미지 표시

![damageText](https://github.com/user-attachments/assets/38f76146-71ff-47f4-ae4c-37b1437c4846)
</br>
플레이어의 무기에 몬스터가 피해를 입으면 몬스터의 위치에 데미지가 표시됩니다.</br>
데미지 표시는 피격된 몬스터의 수만큼 Canvas에서 Text를 통해 수치를 표현하게 됩니다.</br>
데미지 Text를 매번 파괴했다가 재생성하지 않고 풀링 시스템을 통해 Text를 더 효율적으로 관리했습니다.</br>

```
private List<DamageUI> DamagePool = new List<DamageUI>();

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
Text를 풀링으로 관리하기 위해 DamageUI 스크립트를 적용했으며, 부모인 Canvas의 위치를 변수로 갖고 있습니다.</br>
UI를 담당하는 UIController에서 DamageUI의 리스트를 갖고 있으며 몬스터가 피해를 입을때마다 DamageUI를 세팅하는 함수를 호출합니다.</BR>
DamageUI를 세팅하는 함수는 풀에 남아있는 DamageUI가 있나 체크하고 있으면 제일 첫번째 DamageUI를 활성화하고 없다면 새로운 DamageUI를 Canvas의 자식으로 생성합니다.</br>
Text가 일정시간동안 데미지를 출력하면 파괴하지 않고 다시 풀에 넣고 비활성화시킴으로써 다음에 사용할 시 다시 재생성되지 않도록했습니다.</BR></BR>

### 무기 구현
<strong>FireBall</strong></br>
![fireball](https://github.com/user-attachments/assets/f24b9939-5734-4eaf-951e-c3a25c120ed1)
</br>
FireBall 무기는 플레이어 주변을 회전하는 화염구로 무기를 강화할수록 회전하는 화염구의 개수가 늘어납니다.</br>
화염구가 다른 화염구와 일정한 각도를 맞추기 위해 화염구를 갖고 있는 Holder를 따로 두어 관리하도록 했습니다.</br>

```
holder.rotation = Quaternion.Euler(0f, 0f, holder.rotation.eulerAngles.z + (rotateSpeed * Time.deltaTime * stats[weaponLevel].speed));
spawnCounter -= Time.deltaTime;
if (spawnCounter <= 0)
{
    spawnCounter = spawnInterval;
    for(int i = 0; i < stats[weaponLevel].amount; i++)
    {
        float rot = 360f / stats[weaponLevel].amount * i;
        Instantiate(fireballToSpawn, fireballToSpawn.position, Quaternion.Euler(0f, 0f, rot), holder).gameObject.SetActive(true);
    }
}
```
화염구는 Holder에 배치되며 Holder는 현재 무기의 강화상태에 따른 화염구의 개수에 따라 각도를 계산해 화염구들이 일정한 각도로 퍼지도록 합니다.</br>
퍼뜨린 화염구가 회전하는것이 아니라 Holder 자체를 회전시켜 화염구들이 일정한 각도를 유지한채로 플레이어 주변을 회전하도록 했습니다.</br></br>

<strong>HolyZone</strong></br>
![holyzone](https://github.com/user-attachments/assets/d52663a3-2957-4c75-88e0-d04706c94cb7)
</br>

HolyZone는 몬스터가 플레이어한테 일정 거리이내에 들어오면 지속적인 피해를 입는 무기입니다.</br>
무기에 원형 콜라이더를 붙여 OnTriggerEnter 함수를 통해 가까이 있는 모든 몬스터들을 리스트에 담았습니다.</br>

```
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
```
지속 피해를 구현하기 위해 카운터를 통해 몇 틱마다 데미지를 입힐지 damageCounter를 통해 계산했으며, 카운터가 0이 될 때마다 리스트에 담긴 몬스터들을 순회하면서 데미지를 입혔습니다.</br>
몬스터가 무기 구역을 벗어나면 리스트에서 제거해 더 이상 피해를 입지 않게 했습니다.</br></br>

<strong>Dagger</strong></br>
![dagger](https://github.com/user-attachments/assets/f42e1c41-1cd9-4fc8-80f4-e44afd4bab42)
</br>
Dagger 무기는 자동으로 몬스터의 위치를 포착하여 빠르게 날아가는 무기입니다.</br>

```
public LayerMask hitEnemy;

Collider2D[] enemies = Physics2D.OverlapCircleAll(transform.position, weaponRange * stats[weaponLevel].range, hitEnemy);
if(enemies.Length > 0 )
{
    for(int i = 0; i < stats[weaponLevel].amount; i++)
    {
        Vector3 targetPos = enemies[Random.Range(0, enemies.Length)].transform.position;
        
        Vector3 dir = targetPos - transform.position;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        projectile.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        Instantiate(projectile, projectile.transform.position, projectile.transform.rotation).gameObject.SetActive(true);
    }
}
```
Dagger가 몬스터를 향해 날아가기 위해서는 몬스터의 위치를 파악해야합니다.</br>
몬스터의 위치는 몬스터에게 Enemy 레이어를 적용해서 무기가 몬스터를 탐지하는 범위 안에 있는 몬스터를 배열에 넣습니다.</br>
배열에 있는 몬스터 중 랜덤하게 1개를 뽑아 플레이어의 위치와 몬스터의 위치를 통해 각도를 구한 후 해당 방향으로 무기를 생성했습니다.</br></br>

<strong>Axe</strong></br>
![axe](https://github.com/user-attachments/assets/0503d903-1091-4d83-896d-674822cfa47f)
</br>

Axe 무기는 현실에서 도끼를 던지는 것처럼 포물선을 그리며 아래로 떨어지는 무기입니다.</br>

```
rb.linearVelocity = new Vector2(Random.Range(-throwPower, throwPower), throwPower);
transform.rotation = Quaternion.Euler(0f, 0f, transform.rotation.eulerAngles.z + (rotateSpeed * 360f * Time.deltaTime * Mathf.Sign(rb.linearVelocityX)));
```
무기가 아래로 떨어지는 효과를 주기 위해 RigidBody를 붙여 중력 효과를 입혔으며 던지는 힘을 RigidBody에 적용해 포물선을 그리며 나아가도록 했습니다.</br>
또한 무기가 회전하면서 떨어뜨리기 위해 Sign함수를 통해 rotation을 조정해 도끼가 회전하면서 떨어지도록 했습니다.</br>



### 무기 업그레이드

![upgrade](https://github.com/user-attachments/assets/7cb079e0-22d3-4851-a86d-d908f669a397)
</br>
플레이어는 가지고 있는 무기를 레벨업을 할 때마다 강화할 수 있습니다.</br>
플레이어가 레벨업을 하면 강화창이 뜨며, 강화창에는 보유하고 있는 무기를 강화하거나 새로운 무기를 취득할 수 있습니다.</br>
기본적으로 주어지는 무기칸은 3칸이며 스탯을 업그레이드해 총 5개까지 무기칸을 늘릴 수 있습니다.</br>
하지만 무기칸을 업그레이드하기 전에 플레이어의 무기칸이 꽉 찼다면 새로운 무기를 습득하는 것을 방지하기 위해 창이 무기 강화 창이 안뜨도록 했습니다.</br>

```
List<Weapon> availableWeapons = new List<Weapon>();
availableWeapons.AddRange(PlayerController.instance.activeWeapons);

if(availableWeapons.Count > 0)
{
    int selected = Random.Range(0, availableWeapons.Count);
    upgradeWeapons.Add(availableWeapons[selected]);
    availableWeapons.RemoveAt(selected);
}

if(activeWeapons.Count + maxLevelWeapons.Count < maxWeaponsNum)
    availableWeapons.AddRange(PlayerController.instance.inactiveWeapons);

for(int i = upgradeWeapons.Count; i < 3; i++)
{
    if (availableWeapons.Count > 0)
    {
        int selected = Random.Range(0, availableWeapons.Count);
        upgradeWeapons.Add(availableWeapons[selected]);
        availableWeapons.RemoveAt(selected);
    }
}
```

플레이어는 3가지 상태의 무기를 가지고 있습니다.</br>
1. 무기를 보유하고 있으며 강화가 최대치인 무기 -> maxLevelWeapons
2. 무기를 보유하고 있으나 강화가 덜 된 무기 -> activeWeapons
3. 보유하고 있지 않은 무기 -> inactiveWeapons

무기 강화 칸은 3개이며 해당 칸에는 우선적으로 보유하고 있는 무기를 채워줍니다.</br>
하지만 이미 강화가 최대치인 무기는 무기 강화칸에 뜰 필요가 없으므로 activeWeapons만 리스트에 담습니다.</br>
리스트에 담은 무기들을 랜덤하게 뽑아 무기 강화칸에 강화할 무기가 무작위적으로 출력되도록 했습니다.</br></br>

![upgrade2](https://github.com/user-attachments/assets/549cc7a8-6b7a-43ee-b41d-90c8044a5ba4)
</br>
만약 보유한 무기가 3개가 아닐 시 inactiveWeapons 상태인 무기를 모두 리스트에 담아 랜덤하게 뽑아 무기 강화칸에 뜨도록 했습니다.</br>
보유하지 않은 무기가 무기 강화칸에 뜰 시, 무기를 업그레이드하는 것이 아닌 획득하는 것으로 보일 수 있게 텍스트가 다르게 출력됩니다.</br></br>

### 결과창 구현
![end](https://github.com/user-attachments/assets/22c9d340-0d59-4b62-b4f1-f76cf9519bd4)
</br>

플레이어가 몬스터에게 죽으면 결과창이 뜨며 생존한 시간을 나타내도록 했습니다.</br>
플레이어가 체력이 0이 되면 캐릭터에게 죽는 이펙트를 출력한 후에 결과창이 활성화되도록 하기 위해 코루틴을 사용했습니다.</br>

```
public void EndLevel()
{
    gameActive = false;
    StartCoroutine(EndLevelCor());
}

IEnumerator EndLevelCor()
{
    yield return new WaitForSeconds(waitEndScreen);

    float minutes = Mathf.FloorToInt(timer / 60f);
    float seconds = Mathf.FloorToInt(timer % 60f);

    UIController.instance.endTimeText.text = minutes.ToString() + " : " + seconds.ToString("00");
    UIController.instance.levelEndPanel.SetActive(true);
}
```

플레이어가 죽으면 캐릭터가 비활성화되면서 EndLevel을 호출하고 지정해놓은 죽는 이펙트를 화면에 생성합니다.</br>
EndLevel은 게임을 정지시키고 코루틴을 실행해 1초 뒤에 결과창을 띄움으로써 죽는 이펙트가 스킵되지 않도록 했습니다.</br>
