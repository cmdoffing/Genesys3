module Genesys

open Microsoft.AspNetCore.Builder
open Microsoft.Extensions.DependencyInjection
open Microsoft.Extensions.Hosting
open Giraffe
open Giraffe.EndpointRouting

let notFoundHandler = RequestErrors.notFound (text "Not Found") 

let configureApp (appBuilder: IApplicationBuilder) =
    appBuilder.UseRouting()
              .UseGiraffe( Router.endpoints )
              .UseGiraffe( notFoundHandler  )

let configureServices (services: IServiceCollection) =
    services.AddRouting()
            .AddGiraffe() |> ignore

[<EntryPoint>]
let main cmdLineParams =
    let builder = WebApplication.CreateBuilder( cmdLineParams )
    configureServices builder.Services

    let app = builder.Build()

    if   app.Environment.IsDevelopment()
    then app.UseDeveloperExceptionPage() |> ignore

    configureApp app
    app.Run()
    0