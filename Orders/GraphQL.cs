using System.Threading.Tasks;
using HotChocolate;
using HotChocolate.Subscriptions;
using HotChocolate.Types;

namespace Orders
{
    public static class GraphQL
    {
        public static string Message = "Hello from orders secret";
        
        public class AddMessage
        {
            public string Message { get; set; }
        }
        
        public class Query
        {
            public string GetMessage()
            {
                return Message;
            }
        }
        
        public class GraphQLTypes : ObjectType<AddMessage>  
        {  
  
        } 
        
        public class Mutation
        {
            private readonly ITopicEventSender sender;

            public Mutation(ITopicEventSender sender)
            {
                this.sender = sender;
            }

            public async Task<AddMessage> AddMessage(AddMessage message)
            {
                await sender.SendAsync("MessageAdded", message);
                return message;
            }
        }

        public class Subscription
        {
            [Subscribe]
            public AddMessage MessageAdded([EventMessage] AddMessage message)
            {
                return message;
            }
        }
    }
}