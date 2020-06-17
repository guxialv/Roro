using Roro.Flows;
using Roro.Flows.Steps;
using System;
using System.Threading.Tasks;

namespace Roro.Tester
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            var app = new FlowApp();
            var flow1 = await app.Flows.AddNewAsync();
            var flow2 = await app.Flows.AddNewAsync();

            flow1.Steps.AddNew<ActionStep>();
            flow1.Steps.AddNew<CallStep>().FlowPath = flow2.Path;
            flow1.Steps.AddNew<ContinueStep>();
            flow1.Steps.AddNew<ActionStep>();

            flow2.Steps.AddNew<ActionStep>();
            flow2.Steps.AddNew<IfStep>().Steps.AddNew<ActionStep>();
            flow2.Steps.AddNew<ElseIfStep>().Steps.AddNew<ActionStep>();
            flow2.Steps.AddNew<ElseStep>().Steps.AddNew<ActionStep>();
            flow2.Steps.AddNew<ActionStep>();

            Console.WriteLine($"Count: {app.Flows.Count}");
            await app.RunAsync();
            Console.WriteLine($"Count: {app.Flows.Count}");
        }
    }
}
