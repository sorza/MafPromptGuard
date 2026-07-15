using Microsoft.Agents.AI;
using Microsoft.Extensions.AI;

namespace MafPromptGuard
{
    public class AuditMiddleware(AIAgent innerAgent, string logPath) : DelegatingAIAgent(innerAgent)
    {
        private readonly AIAgent _inner = innerAgent;
        private readonly string _logPath = logPath;

        protected override async Task<AgentResponse> RunCoreAsync(
            IEnumerable<ChatMessage> messages,
            AgentSession? session = null,
            AgentRunOptions? options = null,
            CancellationToken cancellationToken = default)
        {
            var response = await _inner.RunAsync(messages, session, options, cancellationToken);

            var lastResponse = response.Messages.LastOrDefault()?.Text ?? string.Empty;

            // Grava no arquivo de auditoria apenas quando o guard bloqueou a requisição
            if (lastResponse.Contains("Esta mensagem foi bloqueada por conter padrões de prompt injection"))
            {
                var userInput = messages.LastOrDefault()?.Text ?? string.Empty;
                var entry = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] BLOQUEADO | Mensagem: {userInput}";

                await File.AppendAllTextAsync(_logPath, entry + Environment.NewLine, cancellationToken);
                Console.WriteLine($"[AUDIT] Tentativa registrada em {_logPath}");
            }

            return response;
        }
    }
}
