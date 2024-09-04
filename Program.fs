module Genesys

open Microsoft.AspNetCore.Builder
open Microsoft.Extensions.DependencyInjection
open Microsoft.Extensions.Hosting
open Giraffe
open Giraffe.EndpointRouting
open Router

let notFoundHandler = RequestErrors.notFound (text "Not Found") 

let configureApp (appBuilder: IApplicationBuilder) =
    appBuilder.UseRouting()
              .UseGiraffe( endpoints )
              .UseGiraffe( notFoundHandler )

let configureServices (services: IServiceCollection) =
    services.AddRouting()
            .AddGiraffe() |> ignore

[<EntryPoint>]
let main args =
    let builder = WebApplication.CreateBuilder( args )
    configureServices builder.Services

    let app = builder.Build()

    if  app.Environment.IsDevelopment() then
        app.UseDeveloperExceptionPage() |> ignore

    configureApp app
    app.Run()
    0