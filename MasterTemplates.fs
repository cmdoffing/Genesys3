module MasterTemplates

open Giraffe.ViewEngine

let navTemplate =
    nav [] 
        [ a [ _href "/About" ] [str "About"] ]

let bodyTemplate =
    let namelist = ["Mike"; "Yana"; "Carolyn"; "Scott"] 
    body []
         [ 
           navTemplate
           h1 [] [str "Welcome:"]
           ol [] (namelist 
                  |> List.map (fun x -> li [] [ Text x ]))
           script [ _src "/_framework/aspnetcore-browser-refresh.js" ] []
         ]

let headTemplate =
    head [] [ title [] [str "Genesys"] ]

let documentTemplate =
    html [_lang "en-US"]
         [  
           headTemplate
           bodyTemplate
         ]

let aboutTemplate =
    p [] [str "About page"]
