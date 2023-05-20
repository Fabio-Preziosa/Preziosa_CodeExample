public class Skill
{   
    // This is the general Skill class which is always characterized by all the attributes and function used
    // to characterize the skill itself. This is the father class of all the different skills class
    
    // General Infos
    public SkillName Name { set; get;}
    public int index;
    
    // PreConditions
    public ArrayList preConditions = new ArrayList();

    // PostConditions
    public ArrayList postConditions = new ArrayList();

    // This method is used to store the object that is involved in the skill
    public virtual ManipulatedObject GetActiveObjectHuman()
    {
        Debug.Log("Called get active object");
        return null;
    }
    
  
    public virtual void UpdateDuring(ref LeftHandController lc, ref RightHandController lr)
    {
        Debug.Log("Called update during Skill");
    }
    
    
    public virtual void UpdateAtEnd(ref LeftHandController lc, ref RightHandController lr)
    {
        Debug.Log("Called update at end Skill");
    }
    
    // This method sets the post-conditions for each object considered active
    // to understand how an object is considered an interacting Object so an object considered active
    // for the skill itself, please take a look on the documentation
    public void SetPostConditions(ref List<ManipulatedObject> interactingObjectsSM)
    {
        for(int idx = 0; idx < interactingObjectsSM.Count; idx++)
        {
            ManipulatedObject interactingObj = interactingObjectsSM[idx];
            ArrayList postCond_ithObject = interactingObj.EvaluateFinalState(); // Evaluate the post conditions of the i-th object
            postConditions.AddRange(postCond_ithObject); // setting the post-conditions for the skill
        }
    }
    
}


public class PickSkill : Skill
{
    public ManipulatedObject pickedObject {private set; get;}
    public List<ManipulatedObject> pickedObjectsPassive = new List<ManipulatedObject>();
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                            
    public PickSkill()
    {
        Name = SkillName.Pick;
        pickedObject = null;
    }

    public override ManipulatedObject GetActiveObjectHuman()
    {
        return pickedObject;
    }

    public override void UpdateAtEnd(ref LeftHandController lc, ref RightHandController lr)
    {
        pickedObject = lefthandControllerc.objInHand;
    }

}

public class ReleaseSkill : Skill
{
    public ManipulatedObject releasedObject_Human {private set; get;}

    public Vector3 objReleasePositionHuman; 
    public Quaternion objReleaseOrientationHuman; 

    public ReleaseSkill()
    {
        Name = SkillName.Release;
    }

    public override ManipulatedObject GetActiveObjectHuman()
    {
        return releasedObject_Human;
    }

    public override void UpdateDuring(ref LeftHandController lc, ref RightHandController lr)
    {
        releasedObject_Human = lefthandControllercc.objInHand;
    }

    public override void UpdateAtEnd(ref LeftHandController lc, ref RightHandController lr)
    {
        objReleasePositionHuman = releasedObject_Human.transform.position;
        objReleaseOrientationHuman = releasedObject_Human.transform.rotation;
    }

}

public class FinalSkill : Skill
{
    // This skill is used just to segment the human actions
    public FinalSkill()
    {
        Name = SkillName.Final;
    }

}

