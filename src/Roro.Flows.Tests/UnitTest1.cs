using Microsoft.VisualStudio.TestTools.UnitTesting;
using Roro.Flows.Steps;
using System.Threading.Tasks;

namespace Roro.Flows.Tests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public async Task TestMethod1()
        {
            var app = new FlowApp();
            var flow1 = await app.Flows.AddNewAsync();
            var flow2 = await app.Flows.AddNewAsync();

            flow1.Steps.AddNew<ActionStep>();
            flow1.Steps.AddNew<FlowStep>().SubType = flow2.Path;
            flow1.Steps.AddNew<CommentStep>();
            flow1.Steps.AddNew<ActionStep>();

            flow2.Steps.AddNew<ActionStep>();
            flow2.Steps.AddNew<IfStep>().Steps!.AddNew<ActionStep>();
            flow2.Steps.AddNew<ElseIfStep>().Steps!.AddNew<ActionStep>();
            flow2.Steps.AddNew<ElseStep>().Steps!.AddNew<ActionStep>();
            flow2.Steps.AddNew<ActionStep>();

            var countBeforeRun = app.Flows.Count;
            await app.RunAsync();
            var countAfterRun = app.Flows.Count;
            Assert.AreEqual(countBeforeRun, countAfterRun);
        }
    }
}
