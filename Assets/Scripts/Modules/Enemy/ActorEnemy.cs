﻿using ActorsECS.Modules.Common;
using ActorsECS.Modules.Enemy.Components;
using Pixeye.Actors;

namespace ActorsECS.Modules.Enemy
{
  public class ActorEnemy : Actor
  {
    [FoldoutGroup("Components", true)] public ComponentEnemy componentEnemy;
    public ComponentStats componentStats;

    protected override void Setup()
    {
      componentStats.statSystem.Init(entity);

      entity.Set(componentEnemy);
      entity.Set(componentStats);
    }
  }
}