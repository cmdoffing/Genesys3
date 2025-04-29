module MasterViews

open Giraffe.ViewEngine
open Urls

let private aboutText =
    Text """
             Genesys (GENErating SYStem) is an F# ASP.Net Core web application
             that manages domain models. Interrupted work in progress.
         """

let mainPageHtml =
    div [_class "jumbotron"]  [
        h1 [] [Text "Genesys"]
        p  [] [aboutText]
    ]

let navView =
    nav [_class "navbar navbar-expand-sm"]  [
        div [_class "container-fluid"]  [
            a [_class "navbar-brand"; _href mainPageUrl]  [Text "Genesys"]
            button [_class "navbar-toggler"; _type "button";
                    _data "bs-toggle" "collapse"; _data "bs-target" "mynavbar"]  [
                span [_class "navbar-toggler-icon"]  []
            ]
            div [ _class "collapse navbar-collapse"; _id "mynavbar"]  [
                ul [_class "navbar-nav me-auto"] [
                    li [_class "nav-item"]  [ a [_href mainPageUrl;    _class "nav-link"]  [Text "Home"]]
                    li [_class "nav-item"]  [ a [_href domainIndexUrl; _class "nav-link"]  [Text "Domains"]]
                    li [_class "nav-item"]  [ a [_href domainNewUrl;   _class "nav-link"]  [Text "New Domain"]]
                    li [_class "nav-item"]  [ a [_href aboutUrl;       _class "nav-link"]  [Text "About"]]
                ]                
            ]
        ]
    ]

let headHtml =
    head []  [
        title  [] [Text "Genesys"]
        meta   [_charset "utf-8"]
        meta   [_name "viewport"; _content "width=device-width, initial-scale=1"]
        link   [_href "https://cdn.jsdelivr.net/npm/bootstrap@5.3.3/dist/css/bootstrap.min.css"; _rel "stylesheet"]
        script [_src "https://cdn.jsdelivr.net/npm/bootstrap@5.3.3/dist/js/bootstrap.bundle.min.js"]  []
    ]

let documentView bodyContent =
    html [_lang "en-US"]  [  
        headHtml
        body [_class "container"]  [
            navView
            bodyContent
            script [_src "https://ajax.googleapis.com/ajax/libs/jquery/3.7.1/jquery.min.js"]  []
            script [_src "/_framework/aspnetcore-browser-refresh.js"]  []
        ]
    ]

let aboutPage = documentView (p [] [aboutText])
let mainPage  = documentView mainPageHtml
