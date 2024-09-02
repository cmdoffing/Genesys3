module MasterViews

open Giraffe.ViewEngine

let navView =
    nav [] 
        [ a [ _href "/About" ] [str "About"] ]

let bodyView =
    let namelist = ["Mike"; "Yana"; "Carolyn"; "Scott"] 
    body []
         [
           navView
           h1 [] [str "Welcome:"]
           ol [] (namelist 
                  |> List.map (fun x -> li [] [ Text x ]))
           script [ _src "/_framework/aspnetcore-browser-refresh.js" ] []
         ]

let headView =
    head [] [ title [] [str "Genesys"] ]

let documentView =
    html [_lang "en-US"]
         [  
           headView
           bodyView
         ]

let aboutView =
    p [] [str "About page"]

let stepInputView =
    
