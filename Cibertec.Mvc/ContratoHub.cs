using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace Cibertec.Mvc
{
    public class ContratoHub : Hub
    {
        static List<string> contratoIds = new List<string>();

        public void AddContratoId(string id)
        {
            if (!contratoIds.Contains(id)) contratoIds.Add(id);
            Clients.All.contratoStatus(contratoIds);
        }

        public void RemoveContratoId(string id)
        {
            if (contratoIds.Contains(id)) contratoIds.Remove(id);
            Clients.All.contratoStatus(contratoIds);
        }

        public override Task OnConnected()
        {
            return Clients.All.contratoStatus(contratoIds);
        }

        public void Message(string message)
        {
            Clients.All.getMessage(message);
        }
    }
}