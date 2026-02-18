using UnityEngine;
using UnityEngine.InputSystem;

public class MultiplayerMovement : MonoBehaviour
{
    public PlayerController player1;
    public PlayerController player2;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void OnMove_Player1(InputValue inputValue)
    {
        player1.OnMove(inputValue);
    }

    public void OnMove_Player2(InputValue inputValue)
    {
        player2.OnMove(inputValue);
    }

    public void OnJump_Player1()
    {
        player1.OnJump();
    }

    public void OnJump_Player2()
    {
        player2.OnJump();
    }
}
