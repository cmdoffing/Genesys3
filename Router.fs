module Router

open System.Globalization
open Microsoft.AspNetCore.Http
open Giraffe
open Giraffe.EndpointRouting
open MasterViews
open Step.Types

let handler1: HttpHandler =
    fun (next: HttpFunc) (ctx: HttpContext) ->
       // ctx.WriteTextAsync "Hello World"
       RequestErrors.BAD_REQUEST "URL route not found" next ctx
       

let submitStepInput : HttpHandler =
    fun (next : HttpFunc) (ctx : HttpContext) ->
        task {
            // Bind a form payload to a StepInput object with CultureInfo
            let  culture   = CultureInfo.CreateSpecificCulture( "en-US" )
            let! stepInput = ctx.BindFormAsync<StepInput>( culture )
            let  returnStr = $"stepInputName is {stepInput.StepInputName} and Doc is {stepInput.StepInputDoc}"
            return! ctx.WriteStringAsync returnStr   // Sends the object back to the client
        }

let endpoints = [
      GET [
          route  "/"       (htmlView documentView)
          route  "/about"  (htmlView aboutView)
          // route  "/domain" (text Database.domainName)
      ]
      POST [
          route "/"                (htmlView documentView)
          route "/submitStepInput" submitStepInput
      ]

      // Not specifying a http verb means it will listen to all verbs
      subRoute "/sub" [ 
          route "/test" handler1 ] 
      ]