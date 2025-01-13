using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;

public class AnimatorControllerCreator : MonoBehaviour
{
    [MenuItem("Tools/Create Functional Animator Controller")]
    public static void CreateFunctionalAnimatorController()
    {
        string folderPath = "Assets/Thinklib/Platformer/Movement/Animations";

        if (!AssetDatabase.IsValidFolder(folderPath))
        {
            AssetDatabase.CreateFolder("Assets/Thinklib/Platformer/Movement", "Animations");
        }

        string controllerPath = $"{folderPath}/PlayerAnimatorController.controller";
        var controller = AnimatorController.CreateAnimatorControllerAtPath(controllerPath);

        controller.AddParameter("IsMoving", AnimatorControllerParameterType.Bool);
        controller.AddParameter("IsJumping", AnimatorControllerParameterType.Bool);
        controller.AddParameter("IsFalling", AnimatorControllerParameterType.Bool);
        controller.AddParameter("MoveSpeed", AnimatorControllerParameterType.Float);

        var rootStateMachine = controller.layers[0].stateMachine;
        var idleState = rootStateMachine.AddState("Idle");
        var walkState = rootStateMachine.AddState("Walking");
        var jumpState = rootStateMachine.AddState("Jumping");
        var fallState = rootStateMachine.AddState("Falling");

        rootStateMachine.defaultState = idleState;

        var idleToWalk = idleState.AddTransition(walkState);
        idleToWalk.AddCondition(AnimatorConditionMode.If, 0, "IsMoving");
        idleToWalk.hasExitTime = false;

        var walkToIdle = walkState.AddTransition(idleState);
        walkToIdle.AddCondition(AnimatorConditionMode.IfNot, 0, "IsMoving");
        walkToIdle.hasExitTime = false;

        var walkToJump = walkState.AddTransition(jumpState);
        walkToJump.AddCondition(AnimatorConditionMode.If, 0, "IsJumping");
        walkToJump.hasExitTime = false;

        var idleToJump = idleState.AddTransition(jumpState);
        idleToJump.AddCondition(AnimatorConditionMode.If, 0, "IsJumping");
        idleToJump.hasExitTime = false;

        var jumpToFall = jumpState.AddTransition(fallState);
        jumpToFall.AddCondition(AnimatorConditionMode.If, 0, "IsFalling");
        jumpToFall.hasExitTime = false;

        var fallToIdle = fallState.AddTransition(idleState);
        fallToIdle.AddCondition(AnimatorConditionMode.IfNot, 0, "IsFalling");
        fallToIdle.AddCondition(AnimatorConditionMode.IfNot, 0, "IsJumping");
        fallToIdle.hasExitTime = false;

        Debug.Log("Animator Controller criado com sucesso em: " + controllerPath);
    }
}
