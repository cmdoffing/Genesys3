module Router

open Microsoft.AspNetCore.Http
open Giraffe
open Giraffe.EndpointRouting
open MasterTemplates

let handler1: HttpHandler =
    fun (_: HttpFunc) (ctx: HttpContext) ->
        ctx.WriteTextAsync "Hello World"

let handler2 (firstName: string, age: int) : HttpHandler =
    fun (_: HttpFunc) (ctx: HttpContext) ->
        sprintf "Hello %s, you are %i years old." firstName age 
        |> ctx.WriteTextAsync
          
let handler3 (a: string, b: string, c: string, d: int) : HttpHandler =
    fun (_: HttpFunc) (ctx: HttpContext) -> 
        sprintf "Hello %s %s %s %i" a b c d |> ctx.WriteTextAsync

let endpoints = [
      GET   
          [ route  "/"            (htmlView documentTemplate)
            route  "/about"       (htmlView aboutTemplate)
            routef "/%s/%i"       handler2
            routef "/%s/%s/%s/%i" handler3 
          ]
      GET_HEAD
          [ route "/foo" (text "Bar")
            route "/x"   (text "y")
            route "/abc" (text "def")
            route "/123" (text "456") 
          ]
      // Not specifying a http verb means it will listen to all verbs
      subRoute "/sub" [ route "/test" handler1 ] ]