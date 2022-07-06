using UnityEngine;

[CreateAssetMenu(menuName = "PluggableAI/Actions/Chase")]
public class ChaseAction : Action
{

    public override void Act(StateController controller)
    {
		Chase(controller);
    }

	private void Chase(StateController controller)
	{
		controller.tankShooting.setChaseUI();
		controller.navMeshAgent.destination = controller.chaseTarget.position;
		controller.navMeshAgent.isStopped = false;
	}

}