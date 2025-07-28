using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Cainos.PixelArtMonster_Dungeon
{
    public class AnimationEventReceiver : MonoBehaviour
    {
        [SerializeField] private MonsterFSM fsm;
        
        public UnityEvent onFootstep;
        public UnityEvent onAttack;
        public UnityEvent onDieFx;
        
        public void OnFootstep()
        {
            onFootstep?.Invoke();
        }

        public void OnAttack()
        {
            onAttack?.Invoke();
            Debug.Log("고옹격");
        }

        public void OnDieFx()
        {
            onDieFx?.Invoke();
        }
        
        
        public void OnAnimationEnd()
        {
            Debug.Log("공격이 끝나고 Idle로 진입");
            fsm.OnAnimationEnd();
        }
    }
}
