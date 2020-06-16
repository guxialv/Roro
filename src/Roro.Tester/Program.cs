using Roro.Flows;
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
            var flow = await app.Flows.AddNewAsync();
            flow.Steps.AddAction();
            var tryStep = flow.Steps.AddTry();
            tryStep.Steps.AddAction();
            tryStep.Steps.AddAction();
            tryStep.Steps.AddThrow();
            tryStep.Steps.AddAction();
            var catchStep = flow.Steps.AddCatch();
            catchStep.Steps.AddAction();
            catchStep.Steps.AddAction();
            flow.Steps.AddAction();
            await app.RunAsync();
        }
    }
}
