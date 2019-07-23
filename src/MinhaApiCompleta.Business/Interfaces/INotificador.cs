using MinhaApiCompleta.Business.Notifications;
using System.Collections.Generic;

namespace MinhaApiCompleta.Business.Interfaces
{
    public interface INotificador
    {
        void Handle(Notificacao notificacao);

        List<Notificacao> ObterNotificacoes();

        bool TemNotificacoes();
    }
}