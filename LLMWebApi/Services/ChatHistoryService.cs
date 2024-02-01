using LLMWebApi.Models;
using SQLitePCL;

namespace LLMWebApi.Services
{
    public class ChatHistoryService
    {
        private readonly ChatHistoryContext? db;

        public ChatHistoryService()
        {
            db = new ChatHistoryContext();
        }

        public ChatHistoryContext Db
        {
            get
            {
                return db!;
            }
        }

        // Add new chat message to the chatHistory.db
        public async Task<bool> AddMessageAsync(ChatMessage chatMessage)
        {
            Console.WriteLine("Addin new message to the chat history...");
            await db!.AddAsync(chatMessage);

            try
            {
                int records = await db.SaveChangesAsync();

                if (records < 1)
                {
                    return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw new Exception();
            }
        }

        // Get all the chat messages from chatHistory.db
        public List<ChatMessage>? GetAllMessages()
        {
            Console.WriteLine("Getting all messages from chat history...");
            List<ChatMessage> messages = [.. db!.ChatMessages];

            if (messages.Count == 0)
            {
                return null;
            }
            return messages;
        }

        public ChatMessage? GetMessageById(int id)
        {
            ChatMessage message = db!.ChatMessages.Find(id)!;

            if (message == null)
            {
                return null;
            }
            return message;
        }

        public List<ChatMessage>? GetMessagesByRole(string role)
        {
            try
            {
                List<ChatMessage> messages = [.. db!.ChatMessages.Where(message => message.Role == role)];

                if (messages.Count > 0)
                {
                    return messages;
                }
                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw new Exception();
            }
        }
    }
}