module MasterViews

open Giraffe.ViewEngine
open Step.Models
open Step.Views

let navView =
    nav [] 
        [ a [ _href "/About" ] [Text "About"] ]

let bodyView =
    body []
         [
            navView
            h1 [] [Text "Welcome:"]
            stepInputView stepInput
            script [ _src "/_framework/aspnetcore-browser-refresh.js" ] []
         ]

let headView =
    head [] [ title [] [Text "Genesys"] ]

let documentView =
    html [_lang "en-US"]
         [  
           headView
           bodyView
         ]

let aboutView =
    p [] [Text "About page"]
    
