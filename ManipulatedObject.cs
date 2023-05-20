public class ManipulatedObject : Object
{
    // This class contains all the atributes gives to each object in the scene the necessary attributes and fuctions for the application
    
    //.....
    
    // Used to store the initial state defined by the evaluation of predicates
    public ArrayList objInitialState;
    
    public void EvaluateInitialState(List<ManipulatedObject> humanObjects)
    {
        // Takes as input the objects that have been considered active during the skill demonstration
        // ad fullfill the attribute objInitialState
        
        objInitialState = new ArrayList();

        ClosePredicate_1 closePredicate_1 = new ClosePredicate_1();
        ClosePredicate_2 closePredicate_2 = new ClosePredicate_2();

        // Adding and evalueting IsGrasped
        closePredicate_1.name = "IsGrasped";
        closePredicate_1.expression = OpenPredicates.IsGrasped;
        closePredicate_1.param1 = this;
        closePredicate_1.result = OpenPredicates.IsGrasped(this);
        objInitialState.Add(closePredicate_1);

        // Adding and evaluating IsFirstAboveSecond
        for(int idx = 0; idx < humanObjects.Count; idx++)
        {
            closePredicate_2.name = "IsFirstAboveSecond";
            closePredicate_2.expression = OpenPredicates.IsFirstAboveSecond;
            closePredicate_2.param1 = this;
            closePredicate_2.param2 = humanObjects[idx];
            closePredicate_2.result = OpenPredicates.IsFirstAboveSecond(this, humanObjects[idx]);                           
            objInitialState.Add(closePredicate_2);
        }

        // Adding and evaluating IsFirstInTouchWithSecond
        for(int idx = 0; idx < humanObjects.Count; idx++)
        {
            closePredicate_2.name = "IsFirstInTouchWithSecond";
            closePredicate_2.expression = OpenPredicates.IsFirstInTouchWithSecond;
            closePredicate_2.param1 = this;
            closePredicate_2.param2 = humanObjects[idx];
            closePredicate_2.result = OpenPredicates.IsFirstInTouchWithSecond(this, humanObjects[idx]);                          
            objInitialState.Add(closePredicate_2);
        }

        // Adding and evaluating IsFirstNearSecond
        for(int idx = 0; idx < humanObjects.Count; idx++)
        {
            closePredicate_2.name = "IsFirstNearSecond";
            closePredicate_2.expression = OpenPredicates.IsFirstNearSecond;
            closePredicate_2.param1 = this;
            closePredicate_2.param2 = humanObjects[idx];
            closePredicate_2.result = OpenPredicates.IsFirstNearSecond(this, humanObjects[idx]);
            objInitialState.Add(closePredicate_2);
        }
    }
    
    public ArrayList EvaluateFinalState()
    {
        // Differently from the EvaluateInitialState function, this function is used to understand which have been
        // the post conditions. In this case there is not an attribute of the object to fullfill since the
        // postconditions are intrinsic to the skill and not of the object
        
        ArrayList postConditions = new ArrayList();
        for(int idx = 0; idx < objInitialState.Count; idx++)
        {
            var elem = objInitialState[idx];
            
            // we are going to check if we are dealing with a predicate of the first or second type
            // then we are evaluating it and we are taking only the predicates that have changed their value
            // comparing them to the inital state
            
            if(elem.GetType().Equals(typeof(ClosePredicate_1)))
            {
                ClosePredicate_1 info = (ClosePredicate_1) elem;
                Object param1 = info.param1;
                if(info.result != info.expression.Invoke(param1))
                {
                    // Chaning parametes with witch evaluate predicate Human -> Robot and its returned value
                    string param1RobotName = info.param1.name.Substring(0, info.param1.name.Length - 5) + "Robot";
                    // Looking in the scene for that specific object
                    info.param1 = GameObject.Find(param1RobotName).GetComponent<Object>();
            
                    if (info.param1 == null)
                        throw new Exception ("Object type not found for param1!");
                    
                    info.result = !info.result;
                    postConditions.Add(info);
                }
            }
            else if(elem.GetType().Equals(typeof(ClosePredicate_2)))
            {
                ClosePredicate_2 info = (ClosePredicate_2) elem;
                Object param1 = info.param1;
                Object param2 = info.param2;

                if(info.result != info.expression.Invoke(param1, param2))
                {
                    // Chaning parametes with witch evaluate predicate Human -> Robot and its returned value
                    string param1RobotName = info.param1.name.Substring(0, info.param1.name.Length - 5) + "Robot";
                    info.param1 = GameObject.Find(param1RobotName).GetComponent<Object>();
                    string param2RobotName = info.param2.name.Substring(0, info.param2.name.Length - 5) + "Robot";
                    info.param2 = GameObject.Find(param2RobotName).GetComponent<Object>();

                    info.result = !info.result;
                    
                    if (info.param1 == null)
                        throw new Exception ("Object type not found for param1!");
                    if (info.param2 == null)
                        throw new Exception ("Object type not found for param2!");

                    postConditions.Add(info);
                }
            }
            else
                throw new Exception("Unrecognized information type");
        }
        return postConditions;
    }
