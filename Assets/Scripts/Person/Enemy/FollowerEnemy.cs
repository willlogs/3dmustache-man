﻿using System.Collections;
using System.Collections.Generic;
using System.Security.AccessControl;
using UnityEngine;

public class FollowerEnemy : EnemyController
{
    public Vector3 targetPos;
    public Transform target;

    private bool _isSuspiciousAboutPosition, _followingPath;
    private bool _isGoing, _shouldBeGoing, _shouldInteract;

    private List<Vector3> _path;

    // TODO:
    // assign pathfinder dynamically
    [SerializeField]
    private Pathfinder _pathfinder;

    private void Start()
    {
        Invoke(nameof(GotoTarget), 1);
    }

    private void GotoTarget()
    {
        _path = _pathfinder.FindAPath(transform.position, target.position);
        // TODO:
        // bring this line to pathfinder
        _path.Reverse();
        if(_path.Count < 1)
        {
            print("no path");
        }
        else
        {
            FollowPath();
        }
    }

    private void FollowPath()
    {
        GotoPosition(_path[0]);
        _path.RemoveAt(0);
        
        _followingPath = _path.Count > 0;
        
    }

    private void GotoPosition(Vector3 position)
    {
        _shouldBeGoing = true;
        _shouldInteract = false;
        targetPos = position;
    }

    private void GotoAndInteract(Vector3 position)
    {
        _shouldInteract = true;
        GotoPosition(position);
    }

    private void Update()
    {
        Vector3 diff = targetPos - transform.position;
        if (_shouldBeGoing && !_isGoing)
        {
            _character.AimAt(targetPos);
            if(diff.x > 0)
            {
                _character.StartMovingRight();
            }
            else
            {
                _character.StartMovingLeft();
            }
            _isGoing = true;
            _shouldBeGoing = false;
        }

        if (_isGoing)
        {
            if (Mathf.Abs(diff.x) > _character.HandReach)
                _character.Move((int)Mathf.Sign(diff.x));
            else
            {
                _isGoing = false;
                

                if (_shouldInteract)
                {
                    TryToStopMoving(diff);
                    _character.InteractWithNearby();
                }

                if (_followingPath)
                {
                    FollowPath();
                }
                else
                {
                    TryToStopMoving(diff);
                }
            }
        }
    }

    private void TryToStopMoving(Vector3 diff)
    {
        if (diff.x > 0)
        {
            _character.StopMovingRight();
        }
        else
        {
            _character.StopMovingLeft();
        }
    }
}
