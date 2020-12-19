using CleverCrow.Fluid.BTs.Tasks;
using CleverCrow.Fluid.BTs.Tasks.Actions;
using UnityEngine;
using UnityEngine.AI;

namespace ActorsECS.AI.EnemyShooter
{
  public class FindPlayerInRadius : ConditionBase
  {
    private LayerMask mask;
    
    // Triggers only the first time this node is run (great for caching data)
    protected override void OnInit () {
      mask = LayerMask.NameToLayer("Player");
    }

    // Triggers every time this node starts running. Does not trigger if TaskStatus.Continue was last returned by this node
    protected override void OnStart () {
    }

    // Triggers every time `Tick()` is called on the tree and this node is run
    protected override bool OnUpdate () {
      var hitColliders = new Collider[] {};
          
      var hits = Physics.OverlapSphereNonAlloc(Owner.transform.position, 5f, hitColliders, LayerMask.NameToLayer("Player"));

      return hits != 0;
    }

    // Triggers whenever this node exits after running
    protected override void OnExit () {
    }
  }
  
  public class GoToAction : ActionBase {
    private NavMeshAgent _agent;
    
    public Transform target;

    protected override void OnInit () {
      _agent = Owner.GetComponent<NavMeshAgent>();
    }

    protected override TaskStatus OnUpdate () {
      _agent.SetDestination(target.position);
      
      return TaskStatus.Success;
    }
  }
}