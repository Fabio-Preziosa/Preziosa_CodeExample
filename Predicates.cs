public class Object : MonoBehaviour
{ 
    //In this class, two types of predicates are defined: "ClosePredicate_1" and "ClosePredicate_2." 
    //The only difference between them is that the former accepts one parameter, while the latter accepts two parameters. 
    //Each predicate is characterized by a name, a function, the involved parameters, and the result. 
    //The predicates become "close" when they are evaluated.
     public struct ClosePredicate_1
    {
        public string name;
        public Func<Object, bool> expression;
        public Object param1;
        public bool result;
    }

    public struct ClosePredicate_2
    {
        public string name;
        public Func<Object, Object, bool> expression;
        public Object param1;
        public Object param2;
        public bool result;
    }
}


public class OpenPredicates : MonoBehaviour
{
    // This class is used to define the functions associated to each predicate
    // Each function starts with the dichiaration of the parameters using the ManipulatedObject class which
    // gives to each object in the scene the necessary attributes and fuctions for the application
    
    public static bool IsGrasped(Object obj) // The function returns true if the object is in the left hand or if it is in the robot's gripper
    {
        ManipulatedObject obj1 = (ManipulatedObject) obj;
        if (obj1.inLeftHand || obj1.isGrasped_robot)
            return true;
        return false;
    }

    public static bool IsFirstAboveSecond(Object obj1_in, Object obj2_in) // The function returns true if obj1 is above obj2
    {   
        ManipulatedObject obj1 = (ManipulatedObject) obj1_in;
        ManipulatedObject obj2 = (ManipulatedObject) obj2_in;

        if(!OpenPredicates.IsFirstInTouchWithSecond(obj1, obj2))
            return false;

        int rayCastLayer = 1 << 7;

        // --- From here there are physical functions are called to detect the obj2 starting from obj1 ---
        
        // Setting the gameobjects near the considered one in raycast layer
        for(int idx = 0; idx < obj1.activeObjects.Count; idx++)
            obj1.activeObjects[idx].layer = 7;

        // Setting max Raycast distance to the size of the object + an offset
        float maxRayDistance = obj1.gameObject.GetComponent<Renderer>().bounds.size.y + 0.05f; 
        Vector3 direction = Vector3.down;
        Vector3 rayCastOrigin = obj1.gameObject.transform.position;
        RaycastHit hitInfo;

        if(Physics.Raycast(rayCastOrigin, direction, out hitInfo, maxRayDistance, rayCastLayer))
        {
            if(hitInfo.collider.gameObject.name.Equals(obj2.name))
                return true;
        }

        // Resetting the default layer
        for(int idx = 0; idx < obj1.activeObjects.Count; idx++)
            obj1.activeObjects[idx].layer = 0;

        return false;
    }

    public static bool IsFirstInTouchWithSecond(Object obj1_in, Object obj2_in) // The function returns true if obj1 is in contact with obj2
    {
        ManipulatedObject obj1 = (ManipulatedObject) obj1_in;
        ManipulatedObject obj2 = (ManipulatedObject) obj2_in;

        if (obj1.activeObjects.Contains(obj2.gameObject))
                return true;
        return false;
    }

    public static bool IsFirstNearSecond(Object obj1_in, Object obj2_in) // The function returns true if obj1 is near obj2
    {
        ManipulatedObject obj1 = (ManipulatedObject) obj1_in;
        ManipulatedObject obj2 = (ManipulatedObject) obj2_in;
        
        Physics.SyncTransforms();
        Collider[] colliders = Physics.OverlapSphere(obj1_in.transform.position,obj1.maxDistanceNeighbours);
        foreach (Collider collider in colliders)
        {
            if (collider.gameObject.name == obj2.objName )
                return true;
        }
        return false;
    }
}
