module MasterViews

open Giraffe.ViewEngine
open Step.Models
open Step.Views

let formCss = """
        form  { display: table;      }
        p     { display: table-row;  }
        label { display: table-cell; }
        input { display: table-cell; }

        fieldset {
            border: 2px solid black;
            padding: 10px;
            background-color: #f9f9f9;
            margin: 8px;    
            border-radius: 4px;
        }

        legend {
            font-weight : bold;
            padding     : 2px;
        }
    """

let navView =
    nav [] [
        a [ _href "/About"   ] [Text "About"]
        a [ _href "/domains" ] [Text "Domains"] 
    ]

let bodyView bodyContent =
    body []
         [
            navView
            h1 [] [Text "Welcome:"]
            stepInputView stepInput
            bodyContent
            script [ _src "https://cdn.jsdelivr.net/npm/bootstrap@5.3.3/dist/js/bootstrap.bundle.min.js" ] []
            script [ _src "/_framework/aspnetcore-browser-refresh.js" ] []
         ]

let headView =
    head [] 
         [ title [] [Text "Genesys"]
           style [_rel "stylesheet"] [Text formCss]
           link [ _href "https://cdn.jsdelivr.net/npm/bootstrap@5.3.3/dist/css/bootstrap.min.css"
                  _rel  "stylesheet"
                  _integrity "sha384-QWTKZyjpPEjISv5WaRU9OFeRpok6YctnYmDr5pNlyT2bRjXh0JMhjY6hW+ALEwIH"
                  _crossorigin "anonymous"
                ]
         ]

let documentView bodyContent =
    html [_lang "en-US"]
         [  
           headView
           bodyView bodyContent
         ]

let aboutView =
    p [] [Text "About page"]
    
