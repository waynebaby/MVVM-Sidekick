using System;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using MVVMSidekick.Collections;
using System.Linq;
using Windows.UI.Xaml.Data;
using System.Threading.Tasks;

namespace MVVMSidekick_UAP10.Test
{
    [TestClass]
    public class CollectionViewTest
    {
        //[TestMethod]
        [Microsoft.VisualStudio.TestPlatform.UnitTestFramework.AppContainer.UITestMethod]
        public void AddTest()
        {
            var cw = new DependencyCollectionView();
            cw.Add("a");
            cw.Add("b");
            Assert.AreEqual(cw.Count, 2);

        }

        //[TestMethod]
        [Microsoft.VisualStudio.TestPlatform.UnitTestFramework.AppContainer.UITestMethod]
        public void AddRemoveTest()
        {
            var cw = new DependencyCollectionView();
            cw.Add("a");
            cw.Add("b");
            cw.Remove("b");
            Assert.AreEqual(cw.Count, 1);

        }


        [Microsoft.VisualStudio.TestPlatform.UnitTestFramework.AppContainer.UITestMethod]
        public void AutoGroupTest()
        {
            var cw = new DependencyCollectionView();
            CreateGroups(cw);

            Enumerable.Range(0, 100)
                .ToList()
                .ForEach(i => cw.Add(i));

            Assert.AreEqual(cw.CollectionGroups.Count, 5);
            foreach (ICollectionViewGroup item in cw.CollectionGroups)
            {
                Assert.AreEqual(20, item.GroupItems.Count);
            }

            cw.Remove(13);
            Assert.AreEqual(
                19,
                cw
                    .CollectionGroups
                    .OfType<SelfServiceDependencyCollectionViewGroupBase>()
                    .Where(g => (int)g.Group == 3)
                    .Select(g => g.GroupItems.Count).First());


            Assert.AreEqual(
                99,
                cw.Count);


            var incFac = new DependencyCollectionViewDelegateIncrementalLoader(
                (v, inc) => true,
                async (v, inc, c) =>
                {
                    Enumerable.Range((int)v[v.Count - 1] + 1, c)
                    .ToList().ForEach(val =>
                        v.Add(val));
                    await Task.Yield();
                    return new LoadMoreItemsResult { Count = (uint)c };
                }
                );

            cw.IncrementalLoader = incFac;

var t=            cw.LoadMoreItemsAsync(10);
          
            Assert.AreEqual(
                 109,
                 cw.Count);

            Assert.AreEqual(
                21,
                cw
                    .CollectionGroups
                    .OfType<SelfServiceDependencyCollectionViewGroupBase>()
                    .Where(g => (int)g.Group == 3)
                    .Select(g => g.GroupItems.Count).First());

        }

        private static void CreateGroups(DependencyCollectionView cw)
        {
            Func<int, SelfServiceDependencyDelegateCollectionViewGroup> fac =
                n =>
                {
                    return new SelfServiceDependencyDelegateCollectionViewGroup(n, cw,
                    (c, o) =>
                    {
                        if (o is int)
                        {
                            var v = (int)o;
                            var moded = v % 5;
                            if (moded == n)
                            {
                                c.GroupItems.Add(o);
                                return true;
                            }

                        }
                        return false;
                    },
                    (c, o) =>
                    {
                        if (o is int)
                        {
                            return c.GroupItems.Remove(o);
                        }
                        return false;

                    });
                };


            cw.CollectionGroups.Add(fac(0));
            cw.CollectionGroups.Add(fac(1));
            cw.CollectionGroups.Add(fac(2));
            cw.CollectionGroups.Add(fac(3));
            cw.CollectionGroups.Add(fac(4));
        }
    }
}
