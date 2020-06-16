namespace Roro.Flows.Execution
{
    public enum ExecutionResult
    {
        NotRun,

        Running,

        Completed,

        EvaluatedToTrue,

        EvaluatedToFalse,

        Failed
    }
}
