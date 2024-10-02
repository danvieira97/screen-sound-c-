using System;
using Microsoft.AspNetCore.Mvc;
using ScreenSound.API.Request;
using ScreenSound.Banco;
using ScreenSound.Modelos;

namespace ScreenSound.API.Endpoints;

public static class ArtistaExtensions
{
    public static void AddEndpointArtistas(this WebApplication app)
    {
        #region Endpoint Artistas
        app.MapGet("/artistas", ([FromServices] DAL<Artista> dal) =>
        {
            var artistas = dal.Listar();
            return Results.Ok(artistas);
        });

        app.MapGet("/artistas/{nome}", (string nome, [FromServices] DAL<Artista> dal) =>
        {
            var artista = dal.RecuperarPor(a => a.Nome.Equals(nome, StringComparison.CurrentCultureIgnoreCase));
            if (artista is null)
            {
                return Results.NotFound("Artista não encontrado");
            }
            return Results.Ok(artista);
        });

        app.MapPost("/artistas", ([FromBody] ArtistaRequest artistaRequest, [FromServices] DAL<Artista> dal) =>
        {
            var artista = new Artista(artistaRequest.Nome, artistaRequest.Bio);
            dal.Adicionar(artista);
            // return Results.Created($"/artistas/{artista.Id}", artista);
            return Results.Ok($"Artista: {artista.Nome} adicionado com sucesso");
        });

        app.MapDelete("/artistas/{id}", (int id, [FromServices] DAL<Artista> dal) =>
        {
            var artista = dal.RecuperarPor(a => a.Id == id);
            if (artista is null)
            {
                return Results.NotFound("Artista não encontrado");
            }
            // return Results.Created($"/artistas/{artista.Id}", artista);
            return Results.NoContent();
        });

        app.MapPut("/artistas", ([FromBody] Artista artista, [FromServices] DAL<Artista> dal) =>
        {
            var artistaAtualizar = dal.RecuperarPor(a => a.Id == artista.Id);
            if (artistaAtualizar is null)
            {
                return Results.NotFound("Artista não encontrado");
            }
            artistaAtualizar.Nome = artista.Nome;
            artistaAtualizar.Bio = artista.Bio;
            artistaAtualizar.FotoPerfil = artista.FotoPerfil;

            dal.Atualizar(artistaAtualizar);
            return Results.Ok($"Artista: {artista.Nome} atualizado com sucesso");
        });
        #endregion
    }
}
