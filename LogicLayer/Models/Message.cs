using LogicLayer.Interfaces;

namespace LogicLayer.Models
{
    public class Message : IModelDebug
    {
        public string MessageValue { get; set; } = string.Empty;

        public string SingleTextOutput()
        {
            return MessageValue;
        }
    }
}
