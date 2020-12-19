using ActorsECS.Modules.Enemy.Components;
using Pixeye.Actors;
using UnityEngine;
using UnityEngine.AI;

namespace ActorsECS.Modules.Enemy.Processors
{
  internal sealed class ProcessorEnemyAI : Processor, ITick
  {
    private readonly Group<ComponentEnemy, ComponentAI> _agents = default;

    public void Tick(float delta)
    {
      foreach (var agent in _agents)
      {
        ref var cAgent = ref agent.ComponentAI();

        var animator = agent.GetMono<Animator>();
        var navMeshAgent = agent.GetMono<NavMeshAgent>();
        
        animator.SetBool("IsMoving", navMeshAgent.velocity != Vector3.zero);
        animator.SetBool("IsIdle", navMeshAgent.velocity == Vector3.zero);
        
        cAgent.behavior.Tick();
      }
    }
  }
}