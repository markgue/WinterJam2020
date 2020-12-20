using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class Character : MonoBehaviour, ISelectable
{
    [SerializeField]
    private float movementSpeed = 1.0f;
    [SerializeField]
    private Material[] materials;

    private Renderer visual = null;
    public Queue<Command> CommandQueue = new Queue<Command>();
    private bool isCurrentTaskDone;
    private Command currentCommand;

    private bool isSelected;
    public bool IsSelected
    {
        get
        {
            return isSelected;
        }
        set
        {
            // Apply tint accordingly
            if (value)
            {
                visual.material = materials[1];
            }
            else
            {
                visual.material = materials[0];
            }
            isSelected = value;
        }
    }

    private void Start()
    {
        visual = gameObject.GetComponent<Renderer>();
        IsSelected = false;
        isCurrentTaskDone = true;
    }

    private void Update()
    {
        // Keep popping orders from the queue
        if (isCurrentTaskDone && CommandQueue.Count > 0)
        {
            isCurrentTaskDone = false;
            currentCommand = CommandQueue.Dequeue();
        }

        ExecuteCommand(currentCommand);
        CheckCompletion(currentCommand);
    }

    private void ExecuteCommand(Command command)
    {
        if (command is MovementOrder)
        {
            MovementOrder mvt = command as MovementOrder;
            Vector3 dest = new Vector3(mvt.Destination.x, transform.position.y, mvt.Destination.y);
            transform.position += Time.deltaTime * movementSpeed * (dest - transform.position);
        }
    }

    private void CheckCompletion(Command command)
    {
        if (command == null)
        {
            isCurrentTaskDone = true;
        }
        if (command is MovementOrder)
        {
            MovementOrder mvt = command as MovementOrder;
            if (Vector2.Distance(
                new Vector2(transform.position.x, transform.position.z), mvt.Destination) <= mvt.Tolerance)
            {
                isCurrentTaskDone = true;
                Debug.Log("Movement finished");
            }
        }
    }
}
