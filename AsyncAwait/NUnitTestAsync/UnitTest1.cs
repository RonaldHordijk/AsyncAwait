using NUnit.Framework;
using System.Threading.Tasks;

namespace Tests
{
    public class Tests
    {
        [Test]
        public async Task TestAsync()
        {
            var result = await DoActionAsync().ConfigureAwait(false);
            Assert.AreEqual(42, result);
        }

        private async Task<int> DoActionAsync()
        {
            await Task.Delay(1000).ConfigureAwait(false);
            return 42;
        }
    }
}