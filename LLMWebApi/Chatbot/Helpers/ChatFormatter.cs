using System.Runtime.CompilerServices;
using LLMWebApi.Models;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;

namespace LLMWebApi.Chatbot.Helpers
{
    public static class ChatFormatter
    {

        private static List<ChatMessage>? messages;
        private static ChatHistory? chatHistory;

        public enum ChatMessageRoles
        {
            System,
            User,
            Assistant
        }

        public static List<ChatMessage>? Messages { get => messages; set => messages = value; }
        public static ChatHistory? ChatHistory { get => chatHistory; set => chatHistory = value; }

        public static ChatHistory? FormatToChatHistory(List<ChatMessage> messages)
        {
            ChatFormatter.messages = messages;

            if (messages != null)
            {
                ChatHistory chatHistory = [];

                foreach (var message in messages)
                {
                    if (Enum.TryParse(message.Role, out ChatMessageRoles role))
                    {
                        switch (role)
                        {
                            case ChatMessageRoles.System:
                                chatHistory.AddMessage(AuthorRole.System, message.Content!);
                                break;
                            case ChatMessageRoles.User:
                                chatHistory.AddMessage(AuthorRole.User, message.Content!);
                                break;
                            case ChatMessageRoles.Assistant:
                                chatHistory.AddMessage(AuthorRole.Assistant, message.Content!);
                                break;
                            default:
                                break;
                        }
                    }
                }

                return chatHistory;
            }

            return null;
        }

        public static List<ChatMessage>? FormatToChatMeesages(ChatHistory chatHistory)
        {
            ChatFormatter.chatHistory = chatHistory;
            List<ChatMessage> chatMessages = [];

            if (chatHistory != null)
            {
                foreach (ChatMessageContent message in chatHistory)
                {
                    Console.WriteLine($"message role as a string: {message.Role.ToString()}");
                    
                    chatMessages.Add(
                        new ChatMessage
                        {
                            Role = message.Role.ToString(),
                            Content = message.Content
                        }
                    );
                }

                return chatMessages;
            }

            return null;
        }
    }
}