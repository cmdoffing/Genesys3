module repos.App

open System
open System.IO
open Microsoft.AspNetCore.Builder
open Microsoft.AspNetCore.Cors.Infrastructure
open Microsoft.AspNetCore.Hosting
open Microsoft.Extensions.Hosting
open Microsoft.Extensions.Logging
open Microsoft.Extensions.DependencyInjection
open FSharp.Data
open Giraffe
open Step.Types
open DotLiquid


// ---------------------------------
// Models
// ---------------------------------

type Spec =
    {
        Name : string
    }

// ---------------------------------
// Views
// ---------------------------------

module Views =
    open Giraffe.ViewEngine

    let layout (content: XmlNode list) =
        html [] [
            head [] [
                title []  [ str "repos" ]
                link [ _rel  "stylesheet"
                       _type "text/css"
                       _href "/main.css" ]
            ]
            body [] content
        ]

    let partial () =
        h1 [] [ str "Genesys" ]

    let specForm () =
        label [_for "fname"] [str "First name:"]
        //<input type="text" id="fname" name="fname" value="John"><br>

    let index (model : Spec) =
        [
            partial()
            p [] [ str model.Name ]
            specForm ()
        ] |> layout

// ---------------------------------
// Web app
// ---------------------------------

let indexHandler (name : string) =
    let greetings = sprintf "Hello %s, from Giraffe!" name
    let model     = { Name = greetings }
    let view      = Views.index model
    htmlView view

let stepTemplateString = File.ReadAllText( @"C:\Users\miked\source\repos\WebRoot\step.html" )
let stepTemplate = Template.Parse( stepTemplateString )

let webApp =
    choose [
        GET >=>
            choose [
                route  "/"         >=> indexHandler "world"
                routef "/hello/%s" indexHandler
            ]
        setStatusCode 404 >=> text "Not Found" ]

// ---------------------------------
// Error handler
// ---------------------------------

let errorHandler (ex : Exception) (logger : ILogger) =
    logger.LogError(ex, "An unhandled exception has occurred while executing the request.")
    clearResponse >=> setStatusCode 500 >=> text ex.Message

// ---------------------------------
// Config and Main
// ---------------------------------

let configureCors (builder : CorsPolicyBuilder) =
    builder
        .WithOrigins(
            "http://localhost:5000",
            "https://localhost:5001")
       .AllowAnyMethod()
       .AllowAnyHeader()
       |> ignore

let configureApp (app : IApplicationBuilder) =
    let env = app.ApplicationServices.GetService<IWebHostEnvironment>()
    (match env.IsDevelopment() with
    | true  ->
        app.UseDeveloperExceptionPage()
    | false ->
        app .UseGiraffeErrorHandler(errorHandler)
            .UseHttpsRedirection())
            .UseCors(configureCors)
            .UseStaticFiles()
            .UseGiraffe( webApp )

let configureServices (services : IServiceCollection) =
    services.AddCors()    |> ignore
    services.AddGiraffe() |> ignore

let configureLogging (builder : ILoggingBuilder) =
    builder.AddConsole()
           .AddDebug() |> ignore

[<EntryPoint>]
let main args =
    // let jsonPath   =  @"..\..\..\json\Genesys.json"
    // let jsonString = """ {"abs": "asdf"} """
    // let jsonString = File.ReadAllText( jsonPath )

    // let Genesys = FSharp.Data.JsonProvider<jsonString>
    // printfn "jsonString = %s" jsonString

    let contentRoot = Directory.GetCurrentDirectory()
    let webRoot     = Path.Combine(contentRoot, "WebRoot")
    Host.CreateDefaultBuilder(args)
        .ConfigureWebHostDefaults(
            fun webHostBuilder ->
                webHostBuilder
                    .UseContentRoot(contentRoot)
                    .UseWebRoot(webRoot)
                    .Configure(Action<IApplicationBuilder> configureApp)
                    .ConfigureServices(configureServices)
                    .ConfigureLogging(configureLogging)
                    |> ignore)
        .Build()
        .Run()
    exit 0