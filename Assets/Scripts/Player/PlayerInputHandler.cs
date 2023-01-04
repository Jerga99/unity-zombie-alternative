using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Eincode.ZombieSurvival.Player
{
    public class PlayerInputHandler : MonoBehaviour
    {
        [SerializeField]
        private InputReader _inputReader;

        public void OnMove(InputValue value)
        {
            MoveInput(value.Get<Vector2>());
        }

        public void OnRoll(InputValue value)
        {
            RollInput(value.isPressed);
        }

        public void RollInput(bool isRoll)
        {
            _inputReader.OnRoll(isRoll);
        }

        public void MoveInput(Vector2 newMoveDirection)
        {
            _inputReader.OnMove(newMoveDirection);
        }
    }

}