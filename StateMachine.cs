public class StateMachine : MonoBehaviour
{
    // This class is used to define the methos and the attributes for the state machine.
    // 
    
    // Skill Object
    public string objectSkill = " ";

    // Hands interactors 
    private LeftHandController leftHandController;
    private RightHandController rightHandController;

    public List<ManipulatedObject> interactingObjectsSM = new List<ManipulatedObject>();
    
    // Possible states and current one
    public SkillChain skillChain;
    public Skill currentSkill { get; private set; }
    public Skill previousSkill { get; private set; }

    private void Start() // Unity method wich runs when the scene starts
    {
        // Adding left and right hand controller 
        leftHandController = GameObject.Find("LeftHand Controller").GetComponent<LeftHandController>();
        rightHandController = GameObject.Find("RightHand Controller").GetComponent<RightHandController>();
    
        // Creating the state machine 
        currentSkill = new FinalSkill();
        previousSkill = currentSkill;
    }

    private void FixedUpdate() // Unity method wich runs each frame
    {
        // At each frame we are going to see in which part of the state machine we are, and we are doing this with
        // EvaluateSkill() function that is used only to moves in that state machine
        currentSkill = EvaluateSkill(); 
        CheckInteractingObjects();
        
        // Following the scheme of the state machine in the documentation, each time we are in the Initial Skill we have
        // just concluded a Skill, so it is time to characterize it
        if (currentSkill.Name == SkillName.Final)
        {
            previousSkill.UpdateAtEnd(ref leftHandController, ref rightHandController);

            // Update the last state foreach active object and add the list of the post-conditions to the skill
            previousSkill.SetPostConditions(ref interactingObjectsSM);
            
            // Cleaning the array of obj with which operator interacted
            interactingObjectsSM.Clear();
            
            // Sending haptic impulse to operator
            leftHandController.controller.SendHapticImpulse(0.7f, 2.0f);
        }
        
        previousSkill = currentSkill;
        previousSkill.UpdateDuring(ref leftHandController, ref rightHandController); // Setting of all the elements during execution
    }

    private void CheckInteractingObjects() 
    {
        //This function is checking if, during the skill execution, an object is considered as an "interacting object" for the skill itslef
        //Please take a look on the documentation.
        for(int idx=0; idx < leftHandController.interactingObjects.Count; idx++)
        {
            ManipulatedObject consideredObj = leftHandController.interactingObjects[idx].gameObject.GetComponent<ManipulatedObject>();
            interactingObjectsSM.Add(consideredObj);
        }
    }

}
