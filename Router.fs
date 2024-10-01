module Router

open System.Globalization
open Microsoft.AspNetCore.Http
open Giraffe
open Giraffe.ViewEngine
open Giraffe.EndpointRouting
open MasterViews
open Step.Types
open Domain

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
          route  "/"       (htmlView (documentView (Text "Main page")))
          route  "/about"  (htmlView aboutView)
      ]
      POST [
          route "/"                (htmlView (documentView (Text "Main page posted")))
          route "/submitStepInput" submitStepInput
      ]
      domainGetEndpoints

      // Not specifying a http verb means it will listen to all verbs
      subRoute "/sub" [ 
          route "/test" handler1 ] 
      ]