using Microsoft.Samples.Eventing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests
{
    [TestClass]
    public class EventSourceEwt
    {

        sealed class BasicEventSource : EventSource
        {
            public void Write(string message) { WriteEvent(1, message); }
            public static BasicEventSource Instance = new BasicEventSource();
        } 

        [TestMethod]
        public void WriteOneEvent()
        {
            using (var watcher = new EventTraceWatcher(BasicEventSource.Instance.Name, BasicEventSource.Instance.Guid))
            {
                List<EventArrivedEventArgs> events = new List<EventArrivedEventArgs>();
                watcher.EventArrived += delegate (object source, EventArrivedEventArgs args)
                    {
                        events.Add(args);
                    };
                BasicEventSource.Instance.Write("A message");
                Assert.AreEqual(1, events.Count);
            }
        }
    }
}
