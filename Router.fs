module Router

open System.Globalization
open Microsoft.AspNetCore.Http
open Giraffe
open Giraffe.EndpointRouting
open MasterViews
open Step.Types

let handler1: HttpHandler =
    fun (_: HttpFunc) (ctx: HttpContext) ->
        ctx.WriteTextAsync "Hello World"

let submitStepInput : HttpHandler =
    fun (next : HttpFunc) (ctx : HttpContext) ->
        task {
            // Binds a form payload to a StepInput object with CultureInfo
            let  culture   = CultureInfo.CreateSpecificCulture( "en-US" )
            let! stepInput = ctx.BindFormAsync<StepInput>( culture )
            let returnStr  = $"stepInputName is {stepInput.StepInputName} and Doc is {stepInput.StepInputDoc}"

            // Sends the object back to the client
            return! ctx.WriteStringAsync returnStr
        }

let endpoints = [
      GET [
          route  "/"      (htmlView documentView)
          route  "/about" (htmlView aboutView)
      ]
      POST [
          route "/"                (htmlView documentView)
          route "/submitStepInput" submitStepInput
      ]

      // Not specifying a http verb means it will listen to all verbs
      subRoute "/sub" [ 
          route "/test" handler1 ] 
      ]