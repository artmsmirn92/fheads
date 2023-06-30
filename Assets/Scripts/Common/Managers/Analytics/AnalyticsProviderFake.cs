using System.Collections.Generic;
using mazing.common.Runtime.Helpers;
using mazing.common.Runtime.Managers;

namespace Common.Managers.Analytics
{
    public class AnalyticsProviderFake : InitBase, IAnalyticsProvider
    {
        public void SendAnalytic(
            string                      _AnalyticId,
            IDictionary<string, object> _EventData = null) 
        { }
    }
}