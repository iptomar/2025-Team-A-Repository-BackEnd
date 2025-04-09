﻿using Microsoft.AspNetCore.SignalR;
namespace GP_Backend.Hubs
{
    public class HorarioHub : Hub
    {
        public async Task NotificarAtualizacao(int id, string horaInicio, string dia)
        {
            await Clients.Others.SendAsync("AulaAtualizada", new
            {
                id,
                horaInicio,
                dia
            });
        }
    }
}
