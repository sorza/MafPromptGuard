using Microsoft.Agents.AI;
using Microsoft.Extensions.AI;

namespace MafPromptGuard
{
    public class PromptInjectionGuardMiddleware(AIAgent innerAgent) : DelegatingAIAgent(innerAgent)
    {
        private readonly AIAgent _inner = innerAgent;

        // Padrões que indicam tentativa de subverter o comportamento do modelo
        private readonly string[] _injectionPatterns =
        [
            "ignore as instruções anteriores",
            "ignore all previous instructions",
            "você agora é",
            "you are now",
            "finja que é",
            "pretend you are",
            "seu novo papel é",
            "esqueça tudo que foi dito"
        ];

        protected override async Task<AgentResponse> RunCoreAsync(
            IEnumerable<ChatMessage> messages,
            AgentSession? session = null,
            AgentRunOptions? options = null,
            CancellationToken cancellationToken = default)
        {
            var userInput = messages.LastOrDefault()?.Text ?? string.Empty;

            var detected = _injectionPatterns
                .FirstOrDefault(p => userInput.Contains(p, StringComparison.OrdinalIgnoreCase));

            if (detected is not null)
            {
                Console.WriteLine($"[INJECTION GUARD] Bloqueado. Padrão detectado: \"{detected}\"");

                // Retorna resposta de bloqueio sem chamar o modelo
                return new AgentResponse(new ChatMessage(
                    ChatRole.Assistant,
                    "Esta mensagem foi bloqueada por conter padrões de prompt injection."));
            }

            return await _inner.RunAsync(messages, session, options, cancellationToken);
        }
    }
}
