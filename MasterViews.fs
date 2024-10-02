module MasterViews

open Giraffe.ViewEngine
open Urls

let navView =
    nav [] [
        a [ _href "/About" ] [Text "About"]
        br []
        a [ _href domainNewUrl ] [Text "New Domain"]
    ]

let bodyView bodyContent =
    body [] [
        navView
        bodyContent
        script [ _src "/_framework/aspnetcore-browser-refresh.js" ] []
    ]

let headView =
    head [] [
        title [] [Text "Genesys"] 
        link [
            _href        "https://cdn.jsdelivr.net/npm/bootstrap@5.3.3/dist/css/bootstrap.min.css"
            _rel         "stylesheet"
            _integrity   "sha384-QWTKZyjpPEjISv5WaRU9OFeRpok6YctnYmDr5pNlyT2bRjXh0JMhjY6hW+ALEwIH"
            _crossorigin "anonymous"
        ]
    ]

let documentView bodyContent =
    html [_lang "en-US"] [  
        headView
        bodyView bodyContent
    ]

let aboutView =
    p [] [Text "About page"]
    
