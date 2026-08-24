using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    private PlayerReferences _refs;

    private bool _isMoving;
    
    private void Awake()
    {
        _refs = GetComponent<PlayerReferences>();
    }

    private void Update()
    {
        if (_isMoving) return;
        if (_refs.input.BlockMovement) return;
        
        if (_refs.input.MovementVector.x >= 1)
        {
            StartCoroutine(_refs.input.Strafing ? Move(Coordinate.East) : Rotate(Coordinate.East));
        }
        if (_refs.input.MovementVector.x <= -1)
        {
            StartCoroutine(_refs.input.Strafing ? Move(Coordinate.West) : Rotate(Coordinate.West));
        }
        if (_refs.input.MovementVector.y >= 1)
        {
            StartCoroutine(Move(Coordinate.North));
        }
        if (_refs.input.MovementVector.y <= -1)
        {
            StartCoroutine(Move(Coordinate.South));
        }
    }

    private IEnumerator Rotate(Coordinate direction)
    {
        if (direction == Coordinate.North || direction == Coordinate.South) yield break;
        _isMoving = true;
        
        var startRot = transform.eulerAngles;
        var targetRot = startRot;
        switch (direction)
        {
            case Coordinate.East:
                targetRot = startRot + new Vector3(0f, 90f, 0f);
                break;
            case Coordinate.West:
                targetRot = startRot + new Vector3(0f, -90f, 0f);
                break;
        }
        
        var progress = 0f;
        while (progress < 1f)
        {
            progress += Time.deltaTime * 3f;
            yield return null;
            transform.eulerAngles = Vector3.Lerp(startRot, targetRot, progress);
        }
        transform.eulerAngles = targetRot;
        yield return null;
        _isMoving = false;
    }

    private IEnumerator Move(Coordinate direction)
    {
        var exitInDirection = CheckForExit(direction);
        if (CheckForWall(direction) && !exitInDirection) yield break;

        if (exitInDirection)
        {
            _refs.cam.transform.SetParent(null);
        }
        
        _isMoving = true;
        _refs.animations.SetMovement(true);
        
        var startPos = transform.position;
        var targetPos = Vector3.zero;
        switch (direction)
        {
            case Coordinate.North:
                targetPos = startPos + transform.forward * 10f;
                break;
            case Coordinate.East:
                targetPos = startPos + transform.right * 10f;
                break;
            case Coordinate.South:
                targetPos = startPos + transform.forward * -10f;
                break;
            case Coordinate.West:
                targetPos = startPos + transform.right * -10f;
                break;
        }

        var progress = 0f;
        while (progress < 1f)
        {
            progress += Time.deltaTime * 3f;
            yield return null;
            transform.position = Vector3.Lerp(startPos, targetPos, progress);
        }
        transform.position = targetPos;
        yield return null;
        
        _refs.animations.SetMovement(false);
        _isMoving = false;

        if (exitInDirection)
        {
            _refs.input.SetBlockMovement(true);
            TransitionManager.Instance.Fade(()=>{SceneManager.LoadScene(SceneManager.GetActiveScene().name);},()=>{});
        }
    }

    private bool CheckForWall(Coordinate direction)
    {
        var dir = transform.forward;
        switch (direction)
        {
            case Coordinate.North:
                dir = transform.forward;
                break;
            case Coordinate.East:
                dir = transform.right;
                break;
            case Coordinate.South:
                dir = -transform.forward;
                break;
            case Coordinate.West:
                dir = -transform.right;
                break;
        }
        Physics.SphereCast(transform.position, 1f, dir, out var hit, 10f, LayerMask.GetMask("Wall"));
        return hit.collider != null;
    }
    
    private bool CheckForExit(Coordinate direction)
    {
        var dir = transform.forward;
        switch (direction)
        {
            case Coordinate.North:
                dir = transform.forward;
                break;
            case Coordinate.East:
                dir = transform.right;
                break;
            case Coordinate.South:
                dir = -transform.forward;
                break;
            case Coordinate.West:
                dir = -transform.right;
                break;
        }
        Physics.SphereCast(transform.position, 1f, dir, out var hit, 10f, LayerMask.GetMask("Door"));
        return hit.collider != null;
    }
}
