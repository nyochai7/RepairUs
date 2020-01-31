using System;
public class ConditionalTask :GeneralTask{

    public Func<bool> conditionFunc;  
    public Task? trueTask;
    public Task? falseTask;
    public OurEvent? trueEvent;
    public OurEvent? falseEvent;
    public ConditionalTask(Func<bool> conditionFunc, Task? trueTask, Task? falseTask, OurEvent? trueEvent, OurEvent? falseEvent)
    {
        this.conditionFunc = conditionFunc;
        this.trueEvent = trueEvent;
        this.falseEvent = falseEvent;
        this.falseTask = falseTask;
        this.trueTask = trueTask;    
    }
}

public class GeneralTask {

    public GeneralTask(){
        
    }
}