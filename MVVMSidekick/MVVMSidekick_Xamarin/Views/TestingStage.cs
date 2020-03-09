using System.Threading.Tasks;

namespace MVVMSidekick.Views
{
    public class TestingStage : IStage
    {
        public string BeaconKey { get; set; }

        public bool CanGoBack { get; set; }

        public bool CanGoForward { get; set; }

        public bool IsGoBackSupported { get; set; }

        public bool IsGoForwardSupported { get; set; }

        public object Target { get; set; }

        Task<TTarget> IStage.Show<TTarget>(TTarget targetViewModel, string viewMappingKey, bool isWaitingForDispose, bool autoDisposeWhenViewUnload)
        {
            return Task.FromResult(targetViewModel);

        }
    }
}
