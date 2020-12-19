using ActorsECS.Core.Modules.Common;
using ActorsECS.Core.Modules.Enemy.Components;
using ActorsECS.Data.Items;
using CleverCrow.Fluid.BTs.Tasks;
using CleverCrow.Fluid.BTs.Trees;
using Pixeye.Actors;
using UnityEngine;
using UnityEngine.AI;
using Random = Pixeye.Actors.Random;

namespace ActorsECS.Core.Modules.Enemy
{
  public class ActorEnemy : Actor
  {
    public Vector3 targetPosition;

    public BehaviorTree tree;

    public WeaponItem weapon;

    [FoldoutGroup("Components", true)]
    public ComponentEnemy componentEnemy;
    public ComponentAI componentAI;
    public ComponentStats componentStats;
    public ComponentEquipment componentEquipment;
    public ComponentWeapon componentWeapon;

    protected override void Setup()
    {
      componentStats.statSystem.Init(entity);
      componentEquipment.equipmentSystem.Init(entity);
      componentEquipment.equipmentSystem.Equip(weapon);

      componentWeapon.currentAmmo = 9999;

      componentAI.behavior = CreateAI();
      tree = CreateAI();
      
      entity.Set(componentEnemy);
      entity.Set(componentAI);
      entity.Set(componentStats);
      entity.Set(componentEquipment);
      entity.Set(componentWeapon);
    }

    private BehaviorTree CreateAI()
    {
      var agent = entity.GetMono<NavMeshAgent>();
      var anim = entity.GetMono<Animator>();

      var lineOfSightTimer = 2.5f;
      var lineOfSightLastSeen = UnityEngine.Time.time;

      var attackRange = weapon.stats.range;
      
      var originPos = transform.position;
      var point = originPos;

      return new BehaviorTreeBuilder(gameObject)
        .Selector()
          .Decorator("Player in line of sight", child =>
          {
            child.Update();
                
            var colliders = new Collider[1];
            var hits = Physics.OverlapSphereNonAlloc(transform.position, 16f, colliders, LayerMask.GetMask("Player"));

            if (hits == 0)
            {
              if (lineOfSightLastSeen + lineOfSightTimer < UnityEngine.Time.time)
              {
                targetPosition = default;
                  
                return TaskStatus.Failure;
              }

              return TaskStatus.Success;
            };

            lineOfSightLastSeen = UnityEngine.Time.time;
            
            targetPosition = colliders[0].transform.position;

            return TaskStatus.Success;
          })
            .Sequence("Chase and attack")
              .Do("Move to", () =>
                {
                  anim.SetBool("IsShooting", false);
                  
                  agent.isStopped = false;
                  
                  agent.SetDestination(targetPosition);

                  return TaskStatus.Success;
                })
              .Condition("Player in attack range", () => targetPosition != default && Vector3.Distance(transform.position, targetPosition) <= attackRange)
              .Do(() =>
              {
                agent.isStopped = true;
                
                transform.LookAt(targetPosition);
                
                entity.ComponentEquipment().equipmentSystem.Weapon.projectileBehaviour.Launch(entity);

                anim.SetBool("IsShooting", true);

                return TaskStatus.Success;
              })
            .End()
          .End()
          .Sequence("Patrol")
            .Do("Find random patrol", () =>
            {
              var randomDirection = UnityEngine.Random.insideUnitSphere * 4f;
                
              randomDirection += originPos;

              var finalPosition = Vector3.zero;
                
              if (NavMesh.SamplePosition(randomDirection, out var hit, 4f, 1)) {
                finalPosition = hit.position;            
              }
                
              point = finalPosition;

              return TaskStatus.Success;
            })
            .Do("Move to", () =>
            {
              anim.SetBool("IsShooting", false);

              agent.isStopped = false;
              
              agent.SetDestination(point);

              return TaskStatus.Success;
            })
            .WaitTime(Random.Range(4, 12))
          .End()
          .WaitTime(0.5f)
        .Build();
    }
  }
}