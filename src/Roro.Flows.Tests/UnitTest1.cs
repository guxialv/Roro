using Microsoft.VisualStudio.TestTools.UnitTesting;
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
            await app.Flows.AddNewAsync();
            await app.Flows.AddNewAsync();
            await app.Flows.AddNewAsync();
            Assert.AreEqual(3, app.Flows.Count);
        }
    }
}
