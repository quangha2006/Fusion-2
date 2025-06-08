using UnityEngine;
using Fusion;
using System;

[RequireComponent(typeof(NetworkPlayerMoveController))]
public class Player : NetworkBehaviour
{
    [SerializeField] private NetworkPlayerMoveController characterController;
    [SerializeField] private CameraHandle cameraHandle;
    [SerializeField] private SkinnedMeshRenderer characterSkinmeshRenderer;
    [SerializeField] private Animator animator;

    [Networked, OnChangedRender(nameof(OnPlayerNameChanged))] public string networkedPlayerName {  get; set; }
    [Networked, OnChangedRender(nameof(OnPlayerColorChanged))] private Vector3 networkedPlayerColor { get; set; }
    [Networked] private Vector3 lastMoveDirection { get; set; }
    private float moveSpeed;
    private bool allowJump = true;
    public Action<string> onPlayerNameChanged;

    public Color playerColor 
    { 
        get { return new Color(networkedPlayerColor.x, networkedPlayerColor.y, networkedPlayerColor.z, 1f); } 
        set { networkedPlayerColor = new Vector3(value.r, value.g, value.b); }
    }
    public string playerName 
    { 
        get { return networkedPlayerName; } 
        set { networkedPlayerName = (value); }
    }
    public override void Spawned()
    {
        if (Object.HasStateAuthority)
        {
            cameraHandle.SetupCamera();
            moveSpeed = characterController.maxSpeed;
            UIManager.Instance.SetActiveLoadingScreen(false);
        }
        OnPlayerColorChanged();
        UIManager.Instance.PlayerHUDController.RegisterPlayer(this);
    }
    public override void FixedUpdateNetwork()
    {
        if(!Object.HasInputAuthority)
        {
            return;
        }
        var moveDirection = Vector3.zero;
        lastMoveDirection = Vector3.zero;
        if (GetInput(out NetworkInputData input))
        {
            if (input.moveDirection != Vector2.zero)
            {
                var lookRotation = Quaternion.Euler(0f, input.lookRotation.y, 0f);
                lastMoveDirection = new Vector3(input.moveDirection.x, 0f, input.moveDirection.y);
                moveDirection = lookRotation * new Vector3(input.moveDirection.x, 0f, input.moveDirection.y);
                if (input.speedUp)
                {
                    characterController.maxSpeed = moveSpeed * 2;
                }
                else
                {
                    characterController.maxSpeed = moveSpeed;
                }
                characterController.Move(moveDirection, input.lookRotation.y);
            }
            if (input.jumpPressed && characterController.Grounded && allowJump)
            {
                characterController.Jump();
                allowJump = false;
            }
            if (!input.jumpPressed && characterController.Grounded)
            {
                allowJump = true;
            }

            cameraHandle.transform.rotation = Quaternion.Euler(input.lookRotation.x, input.lookRotation.y, 0f);
        }
        characterController.Move(moveDirection, input.lookRotation.y);
    }

    private void Update()
    {
        animator.SetFloat("ForwardBackward", lastMoveDirection.z);
        animator.SetFloat("LeftRight", lastMoveDirection.x);
        //Animation
        if (lastMoveDirection != Vector3.zero)
        {
            animator.SetLayerWeight(0, 0);
            animator.SetLayerWeight(1, 1);
        }
        else
        {
            animator.SetLayerWeight(0, 1);
            animator.SetLayerWeight(1, 0);
        }
    }

    public override void Despawned(NetworkRunner runner, bool hasState)
    {
        UIManager.Instance.PlayerHUDController.UnregisterPlayer(this);
        base.Despawned(runner, hasState);
    }
    public void OnPlayerColorChanged()
    {
        characterSkinmeshRenderer.material.color = playerColor;
    }
    public void OnPlayerNameChanged()
    {
        onPlayerNameChanged?.Invoke(playerName);
    }
}

