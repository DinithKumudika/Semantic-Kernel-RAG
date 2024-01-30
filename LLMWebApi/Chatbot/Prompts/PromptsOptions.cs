using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;

namespace LLMWebApi.Chatbot.Prompts 
{
    public class PromptsOptions 
    {
        internal static string[] UserIntentPromptComponents => [];

        internal static string UserIntentExtraction => string.Join("\n", UserIntentPromptComponents);

        /// <summary>
        /// Copy the options in case they need to be modified per chat.
        /// </summary>
        /// <returns>A shallow copy of the options.</returns>
        internal PromptsOptions Copy() => (PromptsOptions)this.MemberwiseClone();

        public static List<string> extractIntentChoices = [
            "ContinueConversation",
            "CreditCardTerms&ConditionsInquiry",
            "TeenPlusAccountTerms&ConditionsInquiry"
        ];

        public static ChatMessageContent masterPrompt = new ChatMessageContent(
            AuthorRole.System,
            @"You are a helpful AI assistant that has expert knowledge on financial/banking domain. 
            You have access to the knowledge base of financial and bank related documentations. 
            Answer the user's queries based on that domain specific knowledge and the knowledge you already have."
        );

        public static List<ChatHistory> extractIntentFewShot = [
            [
                new ChatMessageContent(AuthorRole.User, "what are the terms and conditions for applying for a credit card?"),
                new ChatMessageContent(AuthorRole.System, "Intent:"),
                new ChatMessageContent(AuthorRole.Assistant, "CreditCardTerms&ConditionsInquiry")
            ],
            [
                new ChatMessageContent(AuthorRole.User, "should i pay an annual fee for the credit card?"),
                new ChatMessageContent(AuthorRole.System, "Intent:"),
                new ChatMessageContent(AuthorRole.Assistant, "CreditCardTerms&ConditionsInquiry")
            ],
            [
                new ChatMessageContent(AuthorRole.User, "can i use the card overseas without informing the bank?"),
                new ChatMessageContent(AuthorRole.System, "Intent:"),
                new ChatMessageContent(AuthorRole.Assistant, "CreditCardTerms&ConditionsInquiry")
            ],
            [
                new ChatMessageContent(AuthorRole.User, "when will i receive the statement of accounts?"),
                new ChatMessageContent(AuthorRole.System, "Intent:"),
                new ChatMessageContent(AuthorRole.Assistant, "CreditCardTerms&ConditionsInquiry")
            ],
            [
                new ChatMessageContent(AuthorRole.User, "I am 14 years old. can i open a teen plus account by myself?"),
                new ChatMessageContent(AuthorRole.System, "Intent:"),
                new ChatMessageContent(AuthorRole.Assistant, "TeenPlusAccountTerms&ConditionsInquiry")
            ],
            [
                new ChatMessageContent(AuthorRole.User, "How much should i deposit to open a teen plus account?"),
                new ChatMessageContent(AuthorRole.System, "Intent:"),
                new ChatMessageContent(AuthorRole.Assistant, "TeenPlusAccountTerms&ConditionsInquiry")
            ],
            [
                new ChatMessageContent(AuthorRole.User, "can i have a debit card for my teen plus account?"),
                new ChatMessageContent(AuthorRole.System, "Intent:"),
                new ChatMessageContent(AuthorRole.Assistant, "TeenPlusAccountTerms&ConditionsInquiry")
            ],
            [
                new ChatMessageContent(AuthorRole.User, "what is the maximum amount i can withdraw from my teen plus account?"),
                new ChatMessageContent(AuthorRole.System, "Intent:"),
                new ChatMessageContent(AuthorRole.Assistant, "TeenPlusAccountTerms&ConditionsInquiry")
            ]
        ];
    }
}