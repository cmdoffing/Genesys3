module MasterViews

open Giraffe.ViewEngine
open Urls

let aboutText =
    Text """
            Genesys (GENErating SYStem) is an F# web app that generates 
            F# / Giraffe / Giraffe.ViewEngine CRUD web apps. The Genesys 
            approach to web app design is guided by Parts 1 and 2 of Scott 
            Wlaschin's book "Domain Modeling Made Functional". Genesys 
            departs greatly from the implementation described in Part 3."""

let mainPageHtml =
    div [_class "jumbotron"] [
        h1 [] [Text "Genesys"]
        p  [] [aboutText]
    ]

let navView =
    nav [_class "navbar navbar-default"] [
        div [_class "container-fluid"] [
            div [_class "navbar-header"] [
                a [_class "navbar-brand"; _href mainPageUrl] [Text "Genesys"]
            ]
            ul [_class "nav navbar-nav"] [
                li [_class "active"] [ a [_href mainPageUrl] [Text "Home"] ]
                li [_class ""] [ a [_href domainIndexUrl; _class ""] [Text "Domains"] ]
                li [_class ""] [ a [_href domainNewUrl; _class ""]   [Text "New Domain"] ]
                li [_class ""] [ a [_href aboutUrl; _class ""]       [Text "About"] ]
            ]
        ]
    ]

let headHtml =
    head [] [
        title [] [Text "Genesys"]
        meta [_charset "utf-8"]
        meta [_name "viewport"; _content "width=device-width, initial-scale=1"]
        link [_rel         "stylesheet"
              _crossorigin "anonymous"
              _href        "https://maxcdn.bootstrapcdn.com/bootstrap/3.4.1/css/bootstrap.min.css"
        ]
    ]

let documentView bodyContent =
    html [_lang "en-US"] [  
        headHtml
        body [_class "container"] [
            navView
            bodyContent        
        ]
        script [_src "https://ajax.googleapis.com/ajax/libs/jquery/3.7.1/jquery.min.js"] []
        script [_src "https://maxcdn.bootstrapcdn.com/bootstrap/3.4.1/js/bootstrap.min.js"] []
        script [_src "/_framework/aspnetcore-browser-refresh.js"] []
    ]

let aboutPage =
    documentView (p [] [aboutText])

let mainPage =
    documentView mainPageHtml
