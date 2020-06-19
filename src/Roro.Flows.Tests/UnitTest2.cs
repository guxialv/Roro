using Microsoft.VisualStudio.TestTools.UnitTesting;
using Roro.Flows.Steps;
using System;
using System.Threading.Tasks;

namespace Roro.Flows.Tests
{
    [TestClass]
    public class UnitTest2
    {
        [TestMethod]
        public async Task TestMethod1()
        {
            var app = new FlowApp();
            var flow1 = await app.Flows.AddNewAsync();
            flow1.Steps.AddNew<ActionStep>();
            flow1.Steps.AddNew<IfStep>();
            flow1.Steps.AddNew<ElseIfStep>();
            flow1.Steps.AddNew<ElseStep>();
            flow1.Steps.AddNew<ForStep>().Steps!.AddNew<BreakStep>();
            flow1.Steps.AddNew<WhileStep>().Steps!.AddNew<ContinueStep>();
            flow1.Steps.AddNew<TryStep>().Steps!.AddNew<ThrowStep>();
            flow1.Steps.AddNew<CatchStep>();
            flow1.Steps.AddNew<CommentStep>();
            flow1.Steps.AddNew<FlowStep>();
            await app.RunAsync();
            Console.WriteLine(flow1.ToJson());
        }
    }
}
