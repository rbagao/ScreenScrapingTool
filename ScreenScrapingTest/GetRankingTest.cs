using System;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ScreenScrapingTool;


namespace ScreenScrapingTest
{
    [TestClass]
    public class GetRankingTest
    {
        [TestMethod]
        public async Task GetRankingWithoutKeywords()
        {
            var mvViewModel = new ScreenScrapingTool.ViewModel.MainViewViewModel();
            int ranking = await mvViewModel.GetRankingAsync(null, "www.smokeball.com.au");
            Assert.AreEqual(0, ranking);
        }

        [TestMethod]
        public async Task GetRankingWithoutUrl()
        {
            var mvViewModel = new ScreenScrapingTool.ViewModel.MainViewViewModel();
            int ranking = await mvViewModel.GetRankingAsync("conveyancing software", null);
            Assert.AreEqual(0, ranking);
        }

        [TestMethod]
        public async Task GetRankingWithoutKeywordsAndUrl()
        {
            var mvViewModel = new ScreenScrapingTool.ViewModel.MainViewViewModel();
            int ranking = await mvViewModel.GetRankingAsync(null, null);
            Assert.AreEqual(0, ranking);
        }
    }
}
