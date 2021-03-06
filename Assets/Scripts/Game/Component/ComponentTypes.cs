﻿using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace UnityMMO
{
namespace Component
{
    public struct SceneObjectTypeData : IComponentData
    {
        public SceneObjectType Value;
    }

    //Role类型的话对应RoleID，Monster和NPC类型对应TypeID
    public struct TypeID : IComponentData
    {
        public long Value;
    }

    public struct TimelineState : IComponentData
    {
        public enum NewState
        {
            Allow,//允许新Timeline加进队列
            Forbid,//禁止新Timeline加进队列
        }
        public NewState NewStatus;

        public enum InterruptState
        {
            Allow,//可以播放队列里新的Timeline了
            Forbid,//禁止播放新的Timeline
        }
        public InterruptState InterruptStatus;
    }

    public struct LocomotionState : IComponentData
    {
        public enum State
        {
            Idle,
            Run,
            Sprint,
            Jump,
            DoubleJump,
            TrebleJump,
            InAir,
            BeHit,
            Dead,
            Dizzy,
            StateNum,
        }
        public enum EndType {
            PlayAnimationOnce,//播放完动作就结束当前状态
            EndTime,//依据状态结束时间
            None,
        }
        // public State LocoState;
        private State locoState;
        public State LocoState
        {
            get => locoState;
            set
            {
                locoState = value;
                StateEndType = EndType.None;
                StartTime = TimeEx.ServerTime;
                EndTime = 0;
                // Debug.Log("set locostate : "+value+" track:"+new System.Diagnostics.StackTrace().ToString());
            }
        }
        public EndType StateEndType;

        public bool IsOnGround()
        {
            return LocoState == LocomotionState.State.Idle || LocoState == LocomotionState.State.Run || LocoState == LocomotionState.State.Sprint || LocoState == LocomotionState.State.BeHit || LocoState == LocomotionState.State.Dizzy;
        }
        public bool IsInJump()
        {
            return LocoState == LocomotionState.State.Jump || LocoState == LocomotionState.State.DoubleJump || LocoState == LocomotionState.State.TrebleJump || LocoState == LocomotionState.State.InAir;
        }
        public long StartTime;
        public long EndTime;
    }

    public struct JumpData : IComponentData
    {
        public int JumpCount;
    }

    public struct ActionData : IComponentData
    {
        public int Jump;
        public static ActionData Empty = new ActionData{Jump=0};
    }

    public struct PosOffset : IComponentData
    {
        public float3 Value;
    }

    // public struct JumpState : IComponentData
    // {
    //     public enum State
    //     {
    //         None = 0,
    //         StartJump,
    //         InAir,
    //         EndJump,
    //     }
    //     public State JumpStatus;
    //     public int JumpCount;
    //     public float OriginYPos;
    //     public float AscentHeight;
    //     // public float StartTime;
    // }

    public struct ActionInfo : IComponentData
    {
        public enum Type
        {
            None = 0,
            Skill1 = 1,
            Skill2 = 2,
            Skill3 = 3,
            Skill4 = 4,
        }
        public Type Value;
    }

    public struct MoveSpeed : IComponentData
    {
        public float Value;//实际使用的，变动的
        public float BaseValue;//基础速度，所有速度加成都以此值为因子
    }
    
    public struct TargetPosition : IComponentData
    {
        public float3 Value;
    }

    public struct PosSynchInfo : IComponentData
    {
        public float3 LastUploadPos;
    }

    public struct LooksInfo : IComponentData
    {
        public enum State
        {
            None = 0,
            Loading = 1,
            Loaded = 2,
        }
        public Entity LooksEntity;
        public State CurState;
        public void Destroy()
        {
            if (CurState == State.Loaded && SceneMgr.Instance.EntityManager.HasComponent<Transform>(LooksEntity))
            {
                var trans = SceneMgr.Instance.EntityManager.GetComponentObject<Transform>(LooksEntity);
                SceneMgr.Instance.World.RequestDespawn(trans.gameObject);
            }
        }
    }

    public struct GroundInfo : IComponentData
    {
        // public Collider groundCollider;
        public Vector3 GroundNormal;
        public float Altitude; 
        // public GroundInfo(){}
    }

    public struct HealthStateData : IComponentData      
    {
        public float CurHp;
        public float MaxHp;     
        public int DeathTick;
        public Entity KilledBy;
    }
  
}
}
