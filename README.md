# 3D Idle

## 프로젝트 소개
기간 - 2025.06.05 ~ 2025.06.11

3D 방치형 게임 프로젝트입니다. 플레이어는 몬스터를 자동으로 타겟팅하고 전투하며 얻은 재화를 통해 아이템을 구매하고 업그레이드 하며 캐릭터를 성장시키는 게임입니다. **FSM(상태 머신)** 과 **ScriptableObject** 기반의 데이터 관리 시스템을 사용하였습니다.

---
##  영상
### 아이템 구매 및 장착
![3D_Idle_Item_purchase](https://github.com/user-attachments/assets/eb44e1b4-958a-4120-aa7b-9337c21e8245)


### 아이템 업그레이드
![3D_Idle_Item_upgrade](https://github.com/user-attachments/assets/de209d92-7796-40e7-8507-6a314185135f)


### 전투
![3D_Idle_battle](https://github.com/user-attachments/assets/dff5d431-f866-4534-9857-5cb52b1c0994)



##  주요 기능

### 1. 상태 머신 (FSM)

* **StateManager / IState**: 플레이어와 몬스터가 공유하는 상태 머신 구조
* **Idle / Move / Attack / Die 상태**: 각 상태 클래스는 `IState` 인터페이스를 구현하여 독립적인 로직 처리
* **플레이어와 몬스터의 공통 FSM 사용**: 유연하게 상태 전환을 관리하며, 확장성 있는 구조 설계

---

### 2. 플레이어 시스템 (Player.cs 및 관련 클래스)

* **상태 기반 행동 처리**: FSM 기반으로 이동, 공격, 죽음 등의 상태를 전환하며 플레이어 행동을 처리
* **스탯 계산**: 기본 스탯과 장비 보너스, 버프 등을 계산하여 총합 스탯을 산출
    * **(Ex: public float TotalAttackDamage => (Stat.attackDamage + bonusAttackDamage) * attackDamageMultiplier;)**
* **몬스터 타겟팅 및 자동 전투**: 범위 내 가장 가까운 몬스터를 찾아 타겟으로 설정 후 자동 전투
* **아이템 효과 적용**: 장비 및 소모품 아이템 사용 시 스탯 변경 및 회복 효과 적용
* **경험치와 레벨업**: 몬스터 처치 시 경험치 획득, 레벨업 시 능력치 자동 상승 및 이벤트 발생

---

### 3. 몬스터 시스템 (Monster.cs 및 상태 관리)

* **FSM을 통한 상태 전환**: Idle, Chase, Attack, Die 상태로 구성
* **플레이어 추적 및 공격**: 일정 범위 내의 플레이어를 추적 및 공격
* **피격 처리 및 죽음 이벤트**: 데미지를 받으면 피격 및 죽을 시 상태전환
* **보상 제공**: 몬스터 사망 시 골드와 경험치를 플레이어에게 전달

---

### 4. 스테이지 & 웨이브 시스템 (StageManager.cs)

* **스테이지 기반 진행**: 각 스테이지는 여러 개의 웨이브로 구성되어 있으며 순차 진행
* **웨이브 별 몬스터 구성**: ScriptableObject 형태의 데이터로 몬스터 종류 및 수량 정의
* **보스 처리**: 마지막 웨이브에 보스 여부 설정 가능
* **스테이지 완료 처리**: 선택한 스테이지 계속 반복

---

### 5. 데이터 매니저 (DataManager.cs)

* **모든 게임 데이터 관리 총괄**
* `StageData`, `Monster`, `ItemData` 등을 `Resources.LoadAll`로 로드하여 딕셔너리로 캐싱하여 관리
* Key 값을 기준으로 데이터에 빠르게 접근 가능

**사용 예시:**

```csharp
StageData stage = dataManager.GetStageDataByStageKey(stageKey);
Monster slime = dataManager.GetMonsterByType("Slime");
ItemInstance potion = dataManager.GetItemByKey(1001);
```

---

## 사용 기술

* Unity 2022.3.17f1
* C#
* ScriptableObject
* 상태 머신 (FSM)

---

## 폴더 구조

```
Assets/
├── Scripts/
│   ├── Player
│   ├── Monster
│   ├── Stage
│   ├── Data
│   └── UI
└── Resources/
    └── Data/
        ├── Stage
        ├── Monster
        └── Item
```

---


## 조작법
- **마우스 좌클릭**

---
