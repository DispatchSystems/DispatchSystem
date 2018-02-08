using System;

namespace Dispatch.Common.DataHolders.Storage
{
    [Serializable]
    public class Assignment : IDataHolder, IEventInfo
    {
        public BareGuid Id { get; }
        public string Summary { get; }
        public DateTime Creation { get; }

        public Assignment(string summary)
        {
            Summary = summary;
            Creation = DateTime.Now;
            Id = BareGuid.NewBareGuid();
        }

        public EventArgument[] ToArray()
        {
            return new EventArgument[]
            {
                Summary,
                new EventArgument[] {Id.ToString(), Creation.Ticks}
            };
        }
    }
}
