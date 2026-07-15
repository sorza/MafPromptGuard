# MafPromptGuard

Middleware para **Microsoft Agent Framework (MAF)** que protege agentes contra **prompt injection** e registra logs de segurança e auditoria.

##  Visão Geral

O **MafPromptGuard** foi desenvolvido para aumentar a segurança de agentes construídos com MAF.  
Ele intercepta cada entrada do usuário antes de ser processada pelo agente, aplicando regras de validação e registrando tentativas suspeitas.

## ✨ Funcionalidades

-  **Proteção contra prompt injection**: bloqueia instruções maliciosas ou não autorizadas.
-  **Registro de logs**: mantém histórico estruturado de entradas e bloqueios.
-  **Integração com MAF**: utiliza o middleware `RunAsync` para interceptar prompts.
-  **Monitoramento contínuo**: fornece rastreabilidade para auditoria e análise.

// Exemplo de log estruturado
{
  "timestamp": "2026-07-14T22:40:00",
  "agent": "SupportAgent",
  "input": "delete all files",
  "blocked": true,
  "reason": "Prompt injection detectado"
}
